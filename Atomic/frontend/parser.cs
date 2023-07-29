using System;
using Ttype;
using System.Collections.Generic;
using Atomic_AST;
using System.Threading;

namespace Atomic;

// producting a vaild AST from atoms

public class Parser {
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
    public Parser(List<(string value, TokenType type)> ions) {
        this.ions = ions;
    }


    //Checking if we reached didnt reach end of file yet?
    private bool NotEOF() {
        return this.ions[0].type != TokenType.EOF;
    }


    private TokenType current_token_type() {
        return ions[0].type;
    }
    private string current_token_value() {
        return ions[0].value;
    }

    //removes first ion and return it
    private (string value, TokenType type) move() {
        var prev = ions[0];
        
        ions.RemoveAt(0);
		column++;

        return prev;
    }

    
    //removes first ion return it and check if its correct

    private (string value, TokenType type) except(TokenType correct_type)
    {
        var prev = ions[0];
		ions.RemoveAt(0);
        if(prev.type != correct_type) {
            error("Parser error, excepting " + correct_type + " " + "got => " + prev.type);
        }
		column++;
        return prev;
    }
      
    public AST.Program productAST() {
       
        AST.Program program = new AST.Program();
		program.body = new List<AST.Statement>();
        while(NotEOF()) {
            program.body.Add(parse_statement());
			Console.WriteLine(string.Join(',',program.body.ToArray));
        }
        
        return program;       
    }
    private AST.Statement parse_statement() {
        return parse_expr();
    }
    private AST.Expression parse_expr() {
        return this.parse_additive_expr();
    }
    
   
    // handels additive ane subtraction operations
    private AST.Expression parse_additive_expr()  {
      var left = this.parse_multiplicitave_expr();
      while(current_token_value() == "+" || current_token_value() == "-") {
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
    private AST.Expression parse_multiplicitave_expr() {
         var left = this.parse_primary_expr();
		 while(current_token_value() == "/" || current_token_value() == "*" || current_token_value() == "%") {
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
	
	private dynamic parse_primary_expr() {
		TokenType token = current_token_type();
		switch(token) {
			case TokenType.id:
				AST.Identifier id = new AST.Identifier();
				id.symbol = move().value;
				return id;
			case TokenType.num:
				AST.NumericLiteral num = new AST.NumericLiteral();
				num.value = Convert.ToInt32(move().value);
				Console.WriteLine(num.value);
				return num;
			case TokenType.line:
			    AST.Line Line = new AST.Line();
                line++;
				move();
				return Line;
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
