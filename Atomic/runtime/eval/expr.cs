using System;
using System.Linq;
using System.Collections.Generic;
using ValueTypes;
using Atomic_AST;
using System.Threading;
using static Atomic.statement;
using static Atomic.interpreter;
namespace Atomic;

public class expr
{
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
	public static VT.RuntimeVal eval_id(AST.Identifier id, Enviroment env)
	{
		var value = env.findVar(id.symbol);
		return value;
	}



	//acts as a return to the right
	public static VT.RuntimeVal eval_binary_expr(AST.BinaryExpression binary, Enviroment env)
	{
		var lhs = evaluate(binary.left, env); var rhs = evaluate(binary.right, env);
		if (lhs.type.ToString() == "number" && rhs.type.ToString() == "number")
		{
			return eval_numeric_binary_expr(lhs as VT.NumValue, rhs as VT.NumValue, binary.Operator);
		}
		return new VT.NullVal();
	}


	public static VT.RuntimeVal eval_assignment(AST.AssignmentExpr node, Enviroment env) {
		if(node.assigne.type != "Identifier") {
			error("invaild token in assignment");
		}
		
	}

	public static VT.NumValue eval_numeric_binary_expr(VT.NumValue lhs, VT.NumValue rhs, string ooperator)
	{

		//our lang will only support 64bit
		int results;

		switch (ooperator)
		{
			case "+":
				results = lhs.value + rhs.value;
				break;
			case "*":
				results = lhs.value * rhs.value;
				break;
			case "/":
				results = lhs.value / rhs.value;
				break;
			case "-":
				results = lhs.value - rhs.value;
				break;
			default:
				results = lhs.value % rhs.value;
				break;
		}
		VT.NumValue Results = new VT.NumValue();

		Results.value = results;
		return Results;

	}
}
