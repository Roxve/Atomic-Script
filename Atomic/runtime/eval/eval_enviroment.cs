using ValueTypes;
using Atomic_AST;
using System;
using System.Threading;
using Atomic_debugger;
using System.Drawing;
using Pastel;

namespace Atomic_lang;

public partial class Interpreter
{
	public static Enviroment eval_enviroment(Program prog) {
			Enviroment env = Enviroment.createEnv();
			RuntimeVal lastEvaluated = new NullVal();
			foreach(Statement statement in prog.body) {
				lastEvaluated = evaluate_enviroment(statement,env);
			}
			return env;
	}
	public static RuntimeVal evaluate_enviroment(Statement Statement, Enviroment env)
	{
		switch (Statement.type.ToString())
		{
			case "VarDeclaration":
				return eval_var_declaration(Statement as VarDeclaration, env);
			case "FuncDeclarartion":
				return eval_func_declaration(Statement as FuncDeclarartion, env);
			default:
				return new NullVal();
		}
	}
}
