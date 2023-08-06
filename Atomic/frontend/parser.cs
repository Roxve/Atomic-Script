using System;
using Ttype;
using System.Collections.Generic;
using Atomic_AST;
using System.Threading;
using static Atomic_AST.AST;
using ValueTypes;
using static Atomic.Global;
namespace Atomic;

// producting a vaild AST from atoms

public class Parser
{
	private static int column = 1;
	private static int line = 1;
	private void error(string message)
	{
		Global.Var.error = true;
		Console.BackgroundColor = ConsoleColor.Red;
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine(message + "\nat => line:{0}, ion:{1}", line, column);
		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.White;
	}



	private List<(string value, TokenType
	type)> ions;
	public Parser(List<(string value, TokenType type)> ions)
	{
		this.ions = ions;
	}


	//Checking if we reached didnt reach end of file yet?
	private bool NotEOF()
	{
		if (ions.Count <= 0)
		{
			return false;
		}
		return this.ions[0].type != TokenType.EOF;
	}


	private TokenType current_token_type()
	{
		if (ions.Count <= 0)
		{
			return TokenType.EOF;
		}
		return ions[0].type;
	}
	private string current_token_value()
	{
		if (ions.Count <= 0)
		{
			return "END";
		}
		return ions[0].value;
	}

	//removes first ion and return it
	private (string value, TokenType type) move()
	{
		var prev = ions[0];

		ions.RemoveAt(0);
		column++;

		this.prev = prev;
		return prev;
	}


	//removes first ion return it and check if its correct
	(string value, TokenType type) prev;
	private (string value, TokenType type) except(TokenType correct_type)
	{
		var prev = ions[0];
		ions.RemoveAt(0);
		if (prev.type != correct_type)
		{
			this.error("Parser error, excepting " + correct_type + " " + "got => " + prev.type);
		}
		column++;
		this.prev = prev;
		return prev;
	}

	public AST.Program productAST()
	{

		AST.Program program = new AST.Program();
		program.body = new List<AST.Statement>();
		while (NotEOF())
		{
			program.body.Add(parse_statement());
		}

		return program;
	}
	
	private AST.Statement parse_statement()
	{
		switch (this.current_token_type())
		{
			case TokenType.set:
				return this.parse_var_declaration();
			case TokenType.func:
				return this.parse_func_declaration();
			case TokenType.return_kw:
				return this.parse_return_stmt();
			default:
				return this.parse_expr();
		}
	}

	private AST.Statement parse_func_declaration() {
		this.move();
		
		var name = this.except(TokenType.id).value;
		var args = this.parse_args();
		
		List<string> parameters = new List<string>();
		foreach(AST.Expression arg in args) {
			if(arg.type != "Identifier") {
				this.error("inside func declaration parameters has to be identifiers\ngot => " + arg.type);
			}
			parameters.Add((arg as AST.Identifier).symbol);
		}
		
		this.except(TokenType.OpenBrace);
		List<AST.Statement> body = new List<AST.Statement>();
		
		while(current_token_type() != TokenType.EOF && current_token_type() != TokenType.CloseBrace) {
			body.Add(this.parse_statement());
		}
		
		this.except(TokenType.CloseBrace);
		
		AST.FuncDeclarartion func = new AST.FuncDeclarartion();
		func.name = name; func.parameters = parameters; func.body = body;
		
		return func;
	}
	
	private AST.Statement parse_var_declaration()
	{
		move();
		string id;
		bool locked = false;
		if (current_token_type() == TokenType.locked)
		{
			move();
			id = except(TokenType.id).value;
			locked = true;
		}
		else
		{
			id = except(TokenType.id).value;
		}

		AST.VarDeclaration declare = new AST.VarDeclaration();

		// also TODO: instead of checking id types for everything we can do (parse id) but this is an expirement
		declare.locked = locked;
		declare.Id = id;

		// TODO: request ';'
		if (current_token_type() != TokenType.setter)
		{
			if (locked)
			{
				error("must asinge value to locked vars");
			}

			//TODO request type for vars like this
			declare.value = null;
			return declare;
		}

		else
		{
			move();

			declare.value = this.parse_expr();

			return declare;
		}

	}
	
	private AST.Statement parse_return_stmt() {
		move();
		
		AST.ReturnStmt stmt = new AST.ReturnStmt();
		
		stmt.value = this.parse_expr();
		
		return stmt;
	}


	private AST.Expression parse_expr()
	{
		return this.parse_assigment_expr();
	}

	private AST.Expression parse_assigment_expr()
	{
		var left = this.parse_obj_expr();

		if (this.current_token_type() == TokenType.setter)
		{
			this.move();
			var value = this.parse_assigment_expr();

			AST.AssignmentExpr expr = new AST.AssignmentExpr();
			expr.value = value;
			expr.assigne = left;
			return expr;
		}
		return left;
	}



	private AST.Expression parse_obj_expr()
	{
		if (this.current_token_type() != TokenType.OpenBrace)
		{
			return this.parse_additive_expr();
		}

		this.move();
		var properties = new List<AST.Property>();
		while (this.NotEOF() && this.current_token_type() != TokenType.CloseBrace)
		{
			var key = this.except(TokenType.id).value;


			AST.Property property = new AST.Property();
			if (this.current_token_type() == TokenType.Comma)
			{
				this.move();
				property.key = key;
				properties.Add(property);
				continue;
			}

			else if (this.current_token_type() == TokenType.CloseBrace)
			{
				property.key = key;
				properties.Add(property);
				continue;
			}

			this.except(TokenType.Colon);

			var value = this.parse_expr();

			property.key = key; property.value = value;

			properties.Add(property);

			if (this.current_token_type() != TokenType.CloseBrace)
			{
				this.except(TokenType.Comma);
			}
		}
		this.except(TokenType.CloseBrace);

		AST.ObjectLiteral Obj = new AST.ObjectLiteral();

		Obj.properties = properties;
		return Obj;
	}



	// handels additive ane subtraction operations
	private AST.Expression parse_additive_expr()
	{
		var left = this.parse_multiplicitave_expr();
		while (current_token_value() == "+" || current_token_value() == "-")
		{
			var ooperator = move().value;
			var right = this.parse_multiplicitave_expr();
			AST.BinaryExpression BE = new AST.BinaryExpression();
			BE.left = left;
			BE.right = right;
			BE.Operator = ooperator;
			left = BE;
		}
		return left;
	}



	// handels multiplicitave and divison operations
	private AST.Expression parse_multiplicitave_expr()
	{
		var left = this.parse_call_member_expr();
		while (current_token_value() == "/" || current_token_value() == "*" || current_token_value() == "%")
		{
			var ooperator = move().value;
			var right = this.parse_call_member_expr();
			AST.BinaryExpression BE = new AST.BinaryExpression();
			BE.left = left;
			BE.right = right;
			BE.Operator = ooperator;
			left = BE;
		}
		return left;
	}
	
	
	private AST.Expression parse_call_member_expr() {
		var member = this.parse_member_expr();
		
		if(this.current_token_type() == TokenType.OpenParen) {
			return this.parse_call_expr(member);
		}
		return member;
	}

	
	private AST.Expression parse_call_expr(AST.Expression caller) {
		AST.CallExpr call_expr = new AST.CallExpr();
		
		call_expr.caller = caller; call_expr.args = this.parse_args();
		
		if(this.current_token_type() == TokenType.OpenParen) {
			call_expr = this.parse_call_expr(call_expr) as AST.CallExpr;
		}
		return call_expr;
	}
	
	
	private List<AST.Expression> parse_args() {
		this.except(TokenType.OpenParen);
		List<AST.Expression> args = new List<AST.Expression>();
		if(this.current_token_type() != TokenType.CloseParen) {
			args = this.parse_args_list();
		}
		
		this.except(TokenType.CloseParen);
		return args;
	}
	
	private List<AST.Expression> parse_args_list() {
		List<AST.Expression> args = new List<AST.Expression>();
		args.Add(this.parse_assigment_expr());
		
		while(this.current_token_type() == TokenType.Comma) {
			this.move();
			args.Add(this.parse_assigment_expr());
		}
		return args;
	}
	
	
	private AST.Expression parse_member_expr() {
		var obj = this.parse_primary_expr();
		AST.MemberExpr memberExpr = new AST.MemberExpr();
		while(this.current_token_type() == TokenType.Dot || this.current_token_type() == TokenType.OpenBracket) {
			var ooperator = this.move();
			AST.Expression property;
			bool computed; // !computed => obj.expr,computed obj[]
			
			if(ooperator.type == TokenType.Dot) {
				computed = false;
				property = this.parse_primary_expr();
				
				if(property.type != "Identifier") {
					error("excepted Identifier after dot");
				}
			}
			
			else {
				computed = true;
				property = this.parse_expr();
				
				this.except(TokenType.CloseBracket);
			}
			memberExpr.Object = obj; memberExpr.computed = computed; memberExpr.property = property;
			
			obj = memberExpr;
		}
		
		return obj;
	}
	// Assignment
	// Object
	// AdditiveExpr
	// MultiplicitaveExpr
	// Call
	// Member
	// PrimaryExpr

	private AST.Expression parse_primary_expr()
	{
		TokenType token = current_token_type();
		switch (token)
		{
			case TokenType.id:
				AST.Identifier id = new AST.Identifier();
				id.symbol = move().value;
				return id;
			case TokenType.num:
				AST.NumericLiteral num = new AST.NumericLiteral();
				num.value = Convert.ToInt32(move().value);

				return num;
			case TokenType.Null:
				AST.NullLiteral Null = new AST.NullLiteral();
				move();
				return Null;
			case TokenType.str:
				AST.StringLiteral str = new AST.StringLiteral();
				
				str.value = move().value;
				
				return str;
			case TokenType.Bool:
				AST.Bool Bool = new AST.Bool();
				Bool.value = Convert.ToBoolean(this.move().value);
				return Bool;
			case TokenType.OpenParen:
				move();
				var value = parse_expr();
				except(TokenType.CloseParen);
				return value;
			case TokenType.line:
				line++;
				column = 1;
				move();
				AST.Line Line = new AST.Line();
			    Line.line = line;
				return Line;
			default:
				error("Unexpected token found during parsing! " + current_token_value() + " " + current_token_type());
				move();
				return new AST.NullLiteral();
		}
	}


}
