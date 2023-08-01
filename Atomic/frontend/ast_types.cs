using System;
using System.Collections.Generic;




namespace Atomic_AST {

public class AST {

    public static string NodeType = "Program,NumericLiteral,NullLiteral,Identifier,BinaryExpr,VarDeclaration,AssignmentExpr";


    public class Statement {
          public string type = NodeType;    
    }



    // a program contains many statements
    public class Program : Statement {
		public Program() {
			//modify inherited items
			type = "Program";
		}
        
        public List<Statement> body {get; set;}
    }




    public class Expression : Statement {}


	public class AssignmentExpr : Expression {
		public AssignmentExpr() {
			type = "AssignmentExpr";
		}

		public Expression assigne {get; set;}
		public Expression value {get; set;}
	}


    public class BinaryExpression : Expression {
		public BinaryExpression() {
			type = "BinaryExpr";
		}
        
        public  Expression left {get; set;}
        public  Expression right {get; set;}
        public  string Operator {get; set;}
    }

    
	
	public class VarDeclaration : Statement {
		public VarDeclaration() {
			type = "VarDeclaration";
		}
		public bool locked = false;
		public string Id {get; set;}
		public Expression? value {get; set;}
	}


    public class Identifier : Expression {
        public Identifier() {
			type = "Identifier";
		}
        public string symbol {get; set;}
		public bool locked = false;
    }


    public class NumericLiteral : Expression {
        public NumericLiteral() {
			type = "NumericLiteral";
		}
        public int value {get; set;}
    }
	
	public class NullLiteral : Expression {
		public NullLiteral() {
			type = "NullLiteral";
		}
		public string value = "null";
	}
}
}
