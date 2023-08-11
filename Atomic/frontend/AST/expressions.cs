using System;
using System.Linq;
using System.Collections.Generic;

namespace Atomic_AST;

public class Expression : Statement { }


public class AssignmentExpr : Expression
{
	public AssignmentExpr()
	{
		type = "AssignmentExpr";
	}

	public Expression assigne { get; set; }
	public Expression value { get; set; }
}


//'+-/%'

public class BinaryExpression : Expression
{
	public BinaryExpression()
	{
		type = "BinaryExpr";
	}

	public Expression left { get; set; }
	public Expression right { get; set; }
	public BinaryOperator Operator { get; set; }
}

// a compare expr is a binaryExpr, but to make dev easier i made it a differnt type '<=>|&'
public class CompareExpr : BinaryExpression
{
	public CompareExpr()
	{
		type = "CompareExpr";
	}
}

public class CallExpr : Expression
{
	public CallExpr()
	{
		type = "CallExpr";
	}
	public List<Expression> args { get; set; }

	public Expression caller { get; set; }
}

public class MemberExpr : Expression
{
	public MemberExpr()
	{
		type = "MemberExpr";
	}

	public Expression Object { get; set; }

	public Expression property { get; set; }

	public bool computed { get; set; }
}


public class ifStmt : Expression
{
	public ifStmt()
	{
		type = "ifStmt";
		this.body = new List<Statement>();
	}
	public Expression condition { get; set; }
	public List<Statement> body { get; set; }
}

//else {} || else if {}
public class elseStmt : Expression
{
	public elseStmt()
	{
		type = "elseStmt";
		this.body = new List<Statement>();
	}
	public ifStmt IfStmt { get; set; }
	public List<Statement> body { get; set; }
}

// a block containing many if / else if / else statments
public class ifElseBlock : Expression
{
	public ifElseBlock()
	{
		type = "ifElseBlock";
		elseStmts = new List<elseStmt>();
	}
	public ifStmt mainIfStmt { get; set; }
	public List<elseStmt> elseStmts { get; set; }
}