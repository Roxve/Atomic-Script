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
	//will need this code later
	/*public static RuntimeVal eval_if_stmt(ifStmt Stmt, Enviroment env) {
		RuntimeVal results = MK_NULL();
		foreach(Statement stmt in Stmt.body) {
					results = evaluate(stmt, env);
					if(results.type == "return") {
						break;
					}
	    }
		return results;
	}
	
	public static RuntimeVal eval_else_stmts(List<elseStmt> Stmts,Enviroment env) {
		RuntimeVal results = MK_NULL();
		foreach(elseStmt Else in Stmts) {
			if(Else.body.Count > 0) {
				foreach(Statement stmt in Else.body) {
					results = evaluate(stmt, env);
					if(results.type == "return") {
						break;
					}
				}
				return results;
			}
			else {
				var Condition = evaluate(Else.elseIfStmt.condition, env);
				if(Condition.type != "bool") {
					error($"excepted bool in if condition?\ngot => {Condition.type}", Else.elseIfStmt.condition);
				}
				switch((Condition as BooleanVal).value) {
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
	public static RuntimeVal eval_if_else_block(ifElseBlock Block, Enviroment env) {
		var mainCondition = evaluate(Block.mainIfStmt.condition, env);
		
		if(mainCondition.type != "bool") {
			error($"excepted bool in if condition?\ngot => {mainCondition.type}",Block.mainIfStmt.condition);
		}
		RuntimeVal results = MK_NULL();
		switch((mainCondition as BooleanVal).value) {
			case true:
				results = eval_if_stmt(Block.mainIfStmt,env);
				break;
			case false:
				 results = eval_else_stmts(Block.elseStmts, env);
				 break;
		}
		return results;
	}*/
}
