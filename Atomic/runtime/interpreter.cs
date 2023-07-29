using ValueTypes;
using Atomic_AST;
using System;
using System.Threading;



namespace Atomic;

public class interpreter
{
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
	
	
	public static VT.RuntimeVal eval_program(AST.Program program)
	{
		VT.RuntimeVal lastEvaluated = new VT.NullVal();
		foreach (AST.Statement statement in program.body)
		{
			lastEvaluated = evaluate(statement);
		}
		return lastEvaluated;
	}


	public static VT.RuntimeVal evaluate(AST.Statement Statement)
	{
		switch (Statement.type.ToString())
		{
			case "NumericLiteral":
				VT.NumValue num = new VT.NumValue();
				num.value = (Statement as AST.NumericLiteral).value;
				return num;
			case "NullLiteral":
				return new VT.NullVal();
			case "BinaryExpr":
				return eval_binary_expr(Statement as AST.BinaryExpression);
			default:
				error("unknown error, please report this! error: unknown_01?" + Statement.type.ToString());
				return new VT.NullVal();
		}
	}
	
	
	//acts as a return to the right
	public static VT.RuntimeVal eval_binary_expr(AST.BinaryExpression binary) {
		var lhs = evaluate(binary.left); var rhs = evaluate(binary.right);
		if(lhs.type.ToString() == "NumericLiteral" && rhs.type.ToString() == "NumericLiteral") {
			return eval_numeric_binary_expr(lhs as VT.NumValue, rhs as VT.NumValue, binary.Operator);
		}
		return new VT.NullVal();
	}
	
	public static VT.NumValue eval_numeric_binary_expr(VT.NumValue lhs, VT.NumValue rhs, string ooperator) {
		
		//string because our lang doesnt have float double int etc its only a num
		string results;
		
		switch(ooperator) {
			case "+":
				results = Convert.ToString(long.Parse(lhs) + long.Parse(rhs));
		}
	}
}