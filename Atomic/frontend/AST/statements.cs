using System;
using System.Linq;
using System.Collections.Generic;

namespace Atomic_AST;

#nullable enable
#nullable disable warnings
public class Statement
{
	public string type = "Statement";
	public int line { get; set; }
	public int column { get; set; }
}



// a program contains many statements
public class Program : Statement
{
	public Program()
	{
		//modify inherited items
		type = "Program";
		this.body = new List<Statement>();
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

public class ReturnStmt : Statement
{
	public ReturnStmt()
	{
		type = "ReturnStmt";
	}
	public Expression value { get; set; }
}

//use [string.file_path] || using [moudle]

public class useStmt : Statement
{
	public useStmt() {
		type = "useStmt";
	}
	public string path {get; set;}
	public bool isModule {get; set;}
}
