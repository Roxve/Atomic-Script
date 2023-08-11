﻿using ValueTypes;
using Atomic_AST;
using System;
using System.Threading;
using Atomic_debugger;
using Atomic_AST;
using System.Drawing;
using Pastel;

namespace Atomic_lang;

public partial class interpreter
{
	//for now interpreter cant detect where is errors i need this fixed asap
	private static int column = 1;
	private static int line = 1;
	private static void error(string message, Statement stmt)
	{
		
		Console.WriteLine(($"runtime error: " + message + $"\nat => line:{stmt.line}, column:{stmt.column}").Pastel(Color.Yellow).PastelBg(Color.Red));
		
		if (!(Vars.mode == "repl"))
		{
			Console.WriteLine("press anything to exit".Pastel(Color.Red));
			Console.ReadKey();
			Thread.CurrentThread.Interrupt();
		}
	}


	public static RuntimeVal evaluate(Statement Statement, Enviroment env)
	{
		switch (Statement.type.ToString())
		{
			case "NumericLiteral":
				NumValue num = new NumValue();
				num.value = (Statement as NumericLiteral).value;
				if(Vars.repl) {
					Console.WriteLine(num.value);
				}
				return num;
			case "StringLiteral":
				StringVal str = new StringVal();
				str.value = (Statement as StringLiteral).value;
				if(Vars.repl) {
					Console.WriteLine(str.value);
				}
				return str;
			case "NullLiteral":
				return new NullVal();
			case "Bool":
				BooleanVal Bool = new BooleanVal();
				Bool.value = (Statement as Bool).value;
				return Bool;
			//this should be a parser error?
			case "elseStmt":
				error("found else stmt without being in a if else block,excepted 'if' keyword before 'else' keyword!", Statement);
				return VT.MK_NULL();
			/*case "ifElseBlock":
				return eval_if_else_block(Statement as ifElseBlock, env);*/
			case "Identifier":
				return eval_id(Statement as Identifier, env);
			case "VarDeclaration":
				return eval_var_declaration(Statement as VarDeclaration, env);
			case "FuncDeclarartion":
				return eval_func_declaration(Statement as FuncDeclarartion, env);
			case "ReturnStmt":
				return eval_return_stmt(Statement as ReturnStmt, env);
			case "CallExpr":
				return eval_call_expr(Statement as CallExpr, env);
			case "MemberExpr":
				return eval_member_expr(Statement as MemberExpr, env);
			case "ObjectLiteral":
				return eval_object_expr(Statement as ObjectLiteral, env);
			case "AssignmentExpr":
				return eval_assignment(Statement as AssignmentExpr, env);
			case "BinaryExpr":
				return eval_binary_expr(Statement as BinaryExpression, env);
			case "Program":
				return eval_program(Statement as Program, env);
			default:
				var dump = ObjectDumper.Dump(Statement);
				Console.WriteLine("\ndump info:\n" + dump);
				error("unknown error, please report this! error: unknown_01?" + Statement.type.ToString(), Statement);
				return new NullVal();
		}
	}


}