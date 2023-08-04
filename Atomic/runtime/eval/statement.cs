﻿using System;
using System.Linq;
using System.Collections.Generic;
using ValueTypes;
using Atomic_debugger;
using Atomic_AST;
using static Atomic.expr;
using static Atomic.interpreter;
namespace Atomic;

public class statement
{
	public static VT.RuntimeVal eval_program(AST.Program program, Enviroment env)
	{
		VT.RuntimeVal lastEvaluated = new VT.NullVal();
		foreach (AST.Statement statement in program.body)
		{
			lastEvaluated = evaluate(statement, env);
			if (vars.test)
			{
				Console.WriteLine("current evaluated:");
				Console.WriteLine(ObjectDumper.Dump(lastEvaluated));
			}
		}
		return lastEvaluated;
	}

	public static VT.RuntimeVal eval_var_declaration(AST.VarDeclaration declaration, Enviroment env)
	{
		VT.RuntimeVal value;

		if (declaration.value != null)
		{
			value = evaluate(declaration.value, env);
		}
		else
		{
			value = VT.MK_NULL();
		}

		return env.declareVar(declaration.Id, value, declaration.locked);
	}
}