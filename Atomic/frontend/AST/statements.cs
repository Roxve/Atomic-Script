using System;
using System.Linq;
using System.Collections.Generic;

namespace Atomic_AST
{
	public partial class AST
	{
		public class Statement
		{
			public string type = "Statement";
		}



		// a program contains many statements
		public class Program : Statement
		{
			public Program()
			{
				//modify inherited items
				type = "Program";
			}

			public List<Statement> body { get; set; }
		}


		public class VarDeclaration : Statement
		{
			public VarDeclaration()
			{
				type = "VarDeclaration";
			}
			public bool locked = false;
			public string Id { get; set; }
			public Expression? value { get; set; }
		}

		public class FuncDeclarartion : Statement
		{
			public FuncDeclarartion()
			{
				type = "FuncDeclarartion";
				this.Body = new List<Statement>();
			}
			public string name { get; set; }
			public List<string> parameters { get; set; }

			private List<Statement> Body;

			public List<Statement> body
			{
				get
				{
					return this.Body;
				}
				set
				{
					this.Body = value;
				}
			}
		}
    
	
		//return [expr];
		
		public class ReturnStmt : Statement {
			public ReturnStmt() {
				type = "ReturnStmt";
			}
			public Expression value {get; set;}
		}
		
		
		
		
		public class ifStmt : Statement {
			public ifStmt() {
				type = "ifStmt";
				this.body = new List<Statement>();
			}
			public Expression condition { get; set; }
			public List<Statement> body {get; set;}
		}
		
		//else {} || else if {}
		public class elseStmt : Statement {
			public elseStmt() {
				type = "elseStmt";
				this.body = new List<Statement>();
			}
			public ifStmt elseIfStmt {get; set;}
			public List<Statement> body {get; set;}
		}
		
		// a block containing many if / else if / else statments
		public class ifElseBlock : Statement {
	       public ifElseBlock() {
				type = "ifElseBlock";
				elseStmts = new List<elseStmt>();
		   }
		   public ifStmt mainIfStmt {get; set;}
		   public List<elseStmt> elseStmts {get; set;}
		}
	}
}