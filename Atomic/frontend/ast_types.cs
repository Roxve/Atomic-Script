using System;
using System.Collections.Generic;




namespace Atomic_AST {

public class AST {


    public class Statement {
          public string type = "Statement";    
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

    
	
	public class CallExpr : Expression {
		public CallExpr() {
			type = "CallExpr";
		}
		public List<Expression> args {get; set;}
		
		public Expression caller {get; set;}
	}
	
	public class MemberExpr : Expression {
		public MemberExpr() {
			type = "MemberExpr";
		}
		
		public Expression Object {get; set;}
		
		public Expression property {get; set;}
		
		public bool computed {get; set;}
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

	public class Property : Expression {
		public Property() {
			type = "Property";
		}
		public string key {get; set;}
		public Expression? value {get; set;}	
	}

	public class ObjectLiteral : Expression {
		public ObjectLiteral() {
			type = "ObjectLiteral";
		}
		public List<Property> properties  {get; set;}
	}


    public class NumericLiteral : Expression {
        public NumericLiteral() {
			type = "NumericLiteral";
		}
        public int value {get; set;}
    }
	public class StringLiteral : Expression {
		public StringLiteral() {
			type = "StringLiteral";
		}
		public string value {get; set;}
	}
	public class NullLiteral : Expression {
		public NullLiteral() {
			type = "NullLiteral";
		}
		public string value = "null";
	}
}
}
