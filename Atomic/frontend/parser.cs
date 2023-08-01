using System;
using Ttype;
using System.Collections.Generic;
using Atomic_AST;
using System.Threading;
using static Atomic_AST.AST;
using ValueTypes;
namespace Atomic;

// producting a vaild AST from atoms

public class Parser
{
	private static int column = 1;
	private static int line = 1;
	public void error(string message)
	{
		Console.BackgroundColor = ConsoleColor.Red;
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine(message + "\nat => line:{0}, column:{1}", line, column);
		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine("press anything to exit");
		Console.ReadKey();
		Thread.CurrentThread.Interrupt();
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
		if(ions.Count <= 0) {
			return false;
		}
		return this.ions[0].type != TokenType.EOF;
	}


	private TokenType current_token_type()
	{
		if(ions.Count <= 0) {
			return TokenType.EOF;
		}
		return ions[0].type;
	}
	private string current_token_value()
	{
		if(ions.Count <= 0) {
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
			error("Parser error, excepting " + correct_type + " " + "got => " + prev.type);
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
		switch(this.current_token_type()) {
			case TokenType.set:
				return this.parse_var_declaration();
			default:
				return this.parse_expr();
		}
	}
	
	
	private AST.Statement parse_var_declaration() {
		 move();
		 string id;
		 bool locked = false;
		 if(current_token_type() == TokenType.locked) {
		 	move();
			 id = except(TokenType.id).value;
			 locked = true;
		 }
		 else {
		 	id = except(TokenType.id).value;
		 }
		 
		 AST.VarDeclaration declare = new AST.VarDeclaration();
		 
		 // also TODO: instead of checking id types for everything we can do (parse id) but this is an expirement
		 declare.locked = locked;
		 declare.Id = id;
		 
		 // TODO: request ';'
		 if(current_token_type() != TokenType.setter) {
		 	if(locked) {
			 	error("must asinge value to locked vars");
			 }
			 
			 //TODO request type for vars like this
			 declare.value = null;
			 return declare;
		 }
		 
		 else {
		 	move();
			 
			 declare.value = this.parse_expr();
			 
			 return declare;
		 }
		 
	}
	

	private AST.Expression parse_obj_expr() {
		if(this.current_token_type() != TokenType.OpenBrace) {
			return this.parse_additive_expr();
		}

		move();
		var properties = new List<AST.Property>();
		while(this.NotEOF() && this.current_token_type() != TokenType.CloseBrace) {
			var key = this.except(TokenType.id).value;


			AST.Property property = new AST.Property();
			if(this.current_token_type() == TokenType.Comma) {
				move();
				property.key = key;
				properties.Add(property);
				continue;
			}
			
			else if(this.current_token_type() == TokenType.CloseBrace) {
				property.key = key;
				properties.Add(property);
				continue;
			}

			this.except(TokenType.Colon);

			var value = this.parse_expr();

			property.key = key; property.value = value;

			properties.Add(property);

			if(this.current_token_type() != TokenType.CloseBrace) {
				this.except(TokenType.Comma);
			}
		}
		this.except(TokenType.CloseBrace);

		AST.ObjectLiteral Obj = new AST.ObjectLiteral();

		Obj.properties = properties;
		return Obj;
	}


	private AST.Expression parse_expr()
	{
		return this.parse_assigment_expr();
	}

	private AST.Expression parse_assigment_expr() {
		var left = this.parse_obj_expr();

		if(this.current_token_type() == TokenType.setter) {
			this.move();
			var value = this.parse_assigment_expr();

			AST.AssignmentExpr expr = new AST.AssignmentExpr();
			expr.value = value;
			expr.assigne = left;
			return expr;
		}
		return left;
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
		var left = this.parse_primary_expr();
		while (current_token_value() == "/" || current_token_value() == "*" || current_token_value() == "%")
		{
			var ooperator = move().value;
			var right = this.parse_primary_expr();
			AST.BinaryExpression BE = new AST.BinaryExpression();
			BE.left = left;
			BE.right = right;
			BE.Operator = ooperator;
			left = BE;
		}
		return left;
	}

	private dynamic parse_primary_expr()
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
			case TokenType.OpenParen:
				move();
				var value = parse_expr();
				except(TokenType.CloseParen);
				return value;
			default:
				error("Unexpected token found during parsing! " + current_token_value() + " " + current_token_type());
				return 1;
		}
	}


}
