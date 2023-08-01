using ValueTypes;
using Atomic_AST;
using System;
using System.Threading;
using Atomic_debugger;
using static Atomic.expr;
using static Atomic.statement;
using static Atomic_AST.AST;


namespace Atomic;

public class interpreter
{
	//for now interpreter cant detect where is errors i need this fixed asap
	private static int column = 1;
	private static int line = 1;
	private static void error(string message)
	{
		Console.BackgroundColor = ConsoleColor.Red;
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine(message + "\nat => line:{0}, column:{1}", line, column);
		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine("press anything to exit");
		Console.ReadKey();
		Thread.CurrentThread.Interrupt();
	}


	public static VT.RuntimeVal evaluate(AST.Statement Statement, Enviroment env)
	{
		switch (Statement.type.ToString())
		{
			case "NumericLiteral":
				VT.NumValue num = new VT.NumValue();
				num.value = (Statement as AST.NumericLiteral).value;
				return num;
			case "NullLiteral":
				return new VT.NullVal();
			case "Identifier":
			    return eval_id(Statement as Identifier, env);
			case "VarDeclaration":
			    return eval_var_declaration(Statement as AST.VarDeclaration, env);
			case "ObjectLiteral":
				return eval_object_expr(Statement as AST.ObjectLiteral,env);
			case "AssignmentExpr":
				return eval_assignment(Statement as AssignmentExpr,env);
			case "BinaryExpr":
				return eval_binary_expr(Statement as AST.BinaryExpression, env);
			case "Program":
				return eval_program(Statement as AST.Program, env);
			default:
				error("unknown error, please report this! error: unknown_01?" + Statement.type.ToString());
				return new VT.NullVal();
		}
	}

  
}