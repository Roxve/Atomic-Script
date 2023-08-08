using System;
using System.Linq;
using System.Collections.Generic;
using ValueTypes;
using Atomic_debugger;
using Atomic_AST;

namespace Atomic;

public partial class interpreter
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
	public static VT.RuntimeVal eval_func_declaration(AST.FuncDeclarartion declaration, Enviroment env) {
		VT.FuncVal fn = new VT.FuncVal();
		fn.name = declaration.name;fn.parameters = declaration.parameters; fn.body = declaration.body; fn.env = env;
		
		return env.declareVar(declaration.name, fn ,true);
	}
	
	public static VT.RuntimeVal eval_return_stmt(AST.ReturnStmt stmt, Enviroment env) {
		VT.ReturnVal toReturn = new VT.ReturnVal();
		toReturn.value = evaluate(stmt.value, env);
		return toReturn;
	}
	public static VT.RuntimeVal eval_if_stmt(AST.ifStmt Stmt, Enviroment env) {
		VT.RuntimeVal results = VT.MK_NULL();
		foreach(AST.Statement stmt in Stmt.body) {
					results = evaluate(stmt, env);
	    }
		return results;
	}
	
	public static VT.RuntimeVal eval_else_stmts(List<AST.elseStmt> Stmts,Enviroment env) {
		VT.RuntimeVal results = VT.MK_NULL();
		foreach(AST.elseStmt Else in Stmts) {
			if(Else.body.Count > 0) {
				foreach(AST.Statement stmt in Else.body) {
					results = evaluate(stmt, env);
				}
				return results;
			}
			else {
				var Condition = evaluate(Else.elseIfStmt.condition, env);
				if(Condition.type != "bool") {
					error($"excepted bool in if condition?\ngot => {Condition.type}");
				}
				switch((Condition as VT.BooleanVal).value) {
					case true:
						results = eval_if_stmt(Else.elseIfStmt,env);
						return results;
					case false:
						continue;
				}
			}
		}
		return results;
	}
	public static VT.RuntimeVal eval_if_else_block(AST.ifElseBlock Block, Enviroment env) {
		var mainCondition = evaluate(Block.mainIfStmt.condition, env);
		
		if(mainCondition.type != "bool") {
			error($"excepted bool in if condition?\ngot => {mainCondition.type}");
		}
		VT.RuntimeVal results = VT.MK_NULL();
		switch((mainCondition as VT.BooleanVal).value) {
			case true:
				results = eval_if_stmt(Block.mainIfStmt,env);
				break;
			case false:
				 results = eval_else_stmts(Block.elseStmts, env);
				 break;
		}
		return results;
	}
}
