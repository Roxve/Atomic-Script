using System;
using System.Linq;
using System.Collections.Generic;
using ValueTypes;
using static ValueTypes.VT;
using Atomic_debugger;
using Atomic_AST;

namespace Atomic_lang;

public partial class Interpreter
{
	public static RuntimeVal eval_program(Program program, Enviroment env)
	{
		RuntimeVal lastEvaluated = new NullVal();
		foreach (Statement statement in program.body)
		{
			lastEvaluated = evaluate(statement, env);
			if (Vars.test)
			{
				Console.WriteLine("current evaluated:");
				Console.WriteLine(ObjectDumper.Dump(lastEvaluated));
			}
		}
		return lastEvaluated;
	}

	public static RuntimeVal eval_var_declaration(VarDeclaration declaration, Enviroment env)
	{
		RuntimeVal value;

		if (declaration.value != null)
		{
			value = evaluate(declaration.value, env);
		}
		else
		{
			value = MK_NULL();
		}

		return env.declareVar(declaration.Id, value, declaration.locked, declaration.value);
	}
	public static RuntimeVal eval_func_declaration(FuncDeclarartion declaration, Enviroment env) {
		FuncVal fn = new FuncVal();
		fn.name = declaration.name;fn.parameters = declaration.parameters; fn.body = declaration.body; fn.env = env;
		
		return env.declareVar(declaration.name, fn ,true,declaration);
	}
	
	public static RuntimeVal eval_return_stmt(ReturnStmt stmt, Enviroment env) {
		ReturnVal toReturn = new ReturnVal();
		toReturn.value = evaluate(stmt.value, env);
		return toReturn;
	}
	
}
