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


public class ifExpr : Expression
{
	public ifExpr()
	{
		type = "ifExpr";
		this.body = new List<Statement>();
	}
	public Expression test { get; set; }
	public List<Statement> body { get; set; }
	public Expression alternative { get; set; }
}

//else {}, else if {} is considered an ifexpr
public class elseExpr : Expression
{
	public elseExpr()
	{
		type = "elseExpr";
		this.body = new List<Statement>();
	}
	public List<Statement> body { get; set; }
}

