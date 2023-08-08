using ValueTypes;
using Atomic_AST;
using System;
using System.Threading;
using Atomic_debugger;
using static Atomic_AST.AST;


namespace Atomic;

public partial class interpreter
{
	//for now interpreter cant detect where is errors i need this fixed asap
	private static int column = 1;
	private static int line = 1;
	private static void error(string message)
	{
		Console.BackgroundColor = ConsoleColor.Red;
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine("runtime error: " + message + "\nat => line:{0}, column:{1}", line, column);
		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.White;
		if (!(Global.Var.mode == "repl"))
		{
			Console.WriteLine("press anything to exit");
			Console.ReadKey();
			Thread.CurrentThread.Interrupt();
		}
	}


	public static VT.RuntimeVal evaluate(AST.Statement Statement, Enviroment env)
	{
		switch (Statement.type.ToString())
		{
			case "NumericLiteral":
				VT.NumValue num = new VT.NumValue();
				num.value = (Statement as AST.NumericLiteral).value;
				return num;
			case "StringLiteral":
				VT.StringVal str = new VT.StringVal();
				str.value = (Statement as AST.StringLiteral).value;
				return str;
			case "NullLiteral":
				return new VT.NullVal();
			case "Bool":
				VT.BooleanVal Bool = new VT.BooleanVal();
				Bool.value = (Statement as AST.Bool).value;
				return Bool;
			case "line":
				VT.LineNum Line = new VT.LineNum();
				line++;
				Line.num = (Statement as AST.Line).line;
				return Line;
			case "elseStmt":
				error("found else stmt without being in a if else block,excepted 'if' keyword before 'else' keyword!");
				return VT.MK_NULL();
			case "ifElseBlock":
				return eval_if_else_block(Statement as AST.ifElseBlock, env);
			case "Identifier":
				return eval_id(Statement as Identifier, env);
			case "VarDeclaration":
				return eval_var_declaration(Statement as AST.VarDeclaration, env);
			case "FuncDeclarartion":
				return eval_func_declaration(Statement as AST.FuncDeclarartion, env);
			case "ReturnStmt":
				return eval_return_stmt(Statement as AST.ReturnStmt, env);
			case "CallExpr":
				return eval_call_expr(Statement as AST.CallExpr, env);
			case "MemberExpr":
				return eval_member_expr(Statement as AST.MemberExpr, env);
			case "ObjectLiteral":
				return eval_object_expr(Statement as AST.ObjectLiteral, env);
			case "AssignmentExpr":
				return eval_assignment(Statement as AssignmentExpr, env);
			case "BinaryExpr":
				return eval_binary_expr(Statement as AST.BinaryExpression, env);
			case "Program":
				return eval_program(Statement as AST.Program, env);
			default:
				var dump = ObjectDumper.Dump(Statement);
				Console.WriteLine("\ndump info:\n" + dump);
				error("unknown error, please report this! error: unknown_01?" + Statement.type.ToString());
				return new VT.NullVal();
		}
	}


}