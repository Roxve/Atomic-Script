﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace Atomic_AST;

#nullable enable annotations

public class Identifier : Expression
{
	public Identifier()
	{
		type = "Identifier";
	}
	public string symbol { get; set; }
	public bool locked = false;
}

public class Property : Expression
{
	public Property()
	{
		type = "Property";
	}
	public string key { get; set; }
	public Expression? value { get; set; }
}

public class ObjectLiteral : Expression
{
	public ObjectLiteral()
	{
		type = "ObjectLiteral";
	}
	public List<Property> properties { get; set; }
}


public class NumericLiteral : Expression
{
	public NumericLiteral()
	{
		type = "NumericLiteral";
	}
	public int value { get; set; }
}

public class Bool : Expression
{
	public Bool()
	{
		type = "Bool";
	}

	public bool value { get; set; }
}
public class StringLiteral : Expression
{
	public StringLiteral()
	{
		type = "StringLiteral";
	}
	public string value { get; set; }
}
public class NullLiteral : Expression
{
	public NullLiteral()
	{
		type = "NullLiteral";
	}
	public string value = "null";
}

public class BinaryOperator : Statement {
	public BinaryOperator() {
		type = "BinaryOp";
	}
	public string value {get; set;}
}