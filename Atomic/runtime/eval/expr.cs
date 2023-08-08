using System;
using System.Linq;
using System.Collections.Generic;
using ValueTypes;
using Atomic_AST;
using System.Threading;

namespace Atomic;

public partial class interpreter
{
	public static VT.RuntimeVal eval_id(AST.Identifier id, Enviroment env)
	{
		var value = env.findVar(id.symbol);
		return value;
	}



	//acts as a return to the right
	public static VT.RuntimeVal eval_binary_expr(AST.BinaryExpression binary, Enviroment env)
	{
		var lhs = evaluate(binary.left, env); var rhs = evaluate(binary.right, env);
		if (lhs.type.ToString() == "num" && rhs.type.ToString() == "num")
		{
			return eval_numeric_binary_expr(lhs as VT.NumValue, rhs as VT.NumValue, binary.Operator);
		}
		
		else if(lhs.type == "str" && rhs.type == "str") {
			return eval_string_binary_expr(lhs as VT.StringVal,rhs as VT.StringVal, binary.Operator);
		}
		
		else if(lhs.type == "bool" && rhs.type == "bool") {
			return eval_bool_binary_expr(lhs as VT.BooleanVal, rhs as VT.BooleanVal,binary.Operator);
		}
		
		return new VT.NullVal();
	}
	

	public static VT.RuntimeVal eval_assignment(AST.AssignmentExpr node, Enviroment env)
	{
		
		
	
		if (node.assigne.type == "Identifier")
		{
			var name = (node.assigne as AST.Identifier).symbol;
			return env.setVar(name, evaluate(node.value, env));
		}
		
		else if (node.assigne.type == "MemberExpr")
		{
			 
			var expr = (node.assigne as AST.MemberExpr);

			var Obj = eval_id(expr.Object as AST.Identifier, env);

			if (Obj.type != "obj")
			{
				error($"excepted 'obj'\ngot => {expr.Object.type}");
				return new VT.NullVal();
			}
			 

			VT.ObjectVal obj = Obj as VT.ObjectVal;
			if (!expr.computed)
			{
				 
				AST.Identifier property = (expr.property as AST.Identifier);


				if (!obj.properties.ContainsKey(property.symbol))
				{
					error($"{(expr.Object as AST.Identifier).symbol} doesnt contain property {property.symbol}");
					return new VT.NullVal();
				}

                 
				obj.properties[property.symbol] = evaluate(node.value,env);
				 
				return obj.properties[property.symbol];
			}
			else
			{
				bool isNum = expr.property.type == "NumericLiteral";
				if (!isNum)
				{
					error($"excepeted index of property in object {(expr.Object as AST.Identifier).symbol}");
					return new VT.NullVal();
				}

				int Index = (expr.property as AST.NumericLiteral).value;

				bool IsVaildIndex = obj.properties.Count - 1 >= Index;
				if (!IsVaildIndex)
				{
					error($"{(expr.Object as AST.Identifier).symbol} doesnt contains an index of {Index}");
					return new VT.NullVal();
				}
				var name = obj.properties.ElementAt(Index).Key;
				obj.properties[name] = evaluate(node.value,env);
				
				return obj.properties[name];
			}
		}
		else
		{
			error($"invaild token type in assignment\ngot type => {node.assigne.type}");
			return new VT.NullVal();
		}
	}

	public static VT.RuntimeVal eval_call_expr(AST.CallExpr expr, Enviroment env)
	{
		var args = expr.args.ConvertAll<VT.RuntimeVal>(arg => evaluate(arg, env)).ToArray();

		var fn = evaluate(expr.caller, env);

		switch(fn.type) {
			case "native-fn":
				var results = (fn as VT.NativeFnVal).call(args, env);
				return results;
			case "func":
				var func = (fn as VT.FuncVal);
				var funcEnv = new Enviroment(func.env);
				
				for(int x = 0; x < func.parameters.Count; x++) {
					var name = func.parameters[x];
					
					funcEnv.declareVar(name, args[x], false);
				}
				VT.RuntimeVal result = VT.MK_NULL();
				VT.RuntimeVal last;
				foreach(AST.Statement stmt in func.body) {
					//TODO add unreachable code warning
					last = evaluate(stmt, funcEnv);
					if(last.type == "return") {
						result = (last as VT.ReturnVal).value;
					}
				}
				return result;
			default:
				error("Cannot call a value that is not a Function \ngot => " + fn.type);
				return new VT.NullVal();
		}
	}

	public static VT.RuntimeVal eval_object_expr(AST.ObjectLiteral obj, Enviroment env)
	{
		VT.ObjectVal Obj = new VT.ObjectVal(); Obj.properties = new Dictionary<string, VT.RuntimeVal>();

		foreach (AST.Property property in obj.properties)
		{
			VT.RuntimeVal runtimeVal = new VT.RuntimeVal();

			if (property.value == null)
			{
				runtimeVal = env.findVar(property.key);
			}
			else
			{
				runtimeVal = evaluate(property.value, env);
			}

			Obj.properties.Add(property.key, runtimeVal);
		}
		return Obj;
	}

	public static VT.RuntimeVal eval_member_expr(AST.MemberExpr expr, Enviroment env)
	{
		var Obj = eval_id(expr.Object as AST.Identifier, env);

		if (Obj.type != "obj")
		{
			error($"excepted 'obj'\ngot => {expr.Object.type}");
			return new VT.NullVal();
		}

		VT.ObjectVal obj = Obj as VT.ObjectVal;
		if (!expr.computed)
		{
			AST.Identifier property = (expr.property as AST.Identifier);


			if (!obj.properties.ContainsKey(property.symbol))
			{
				error($"{(expr.Object as AST.Identifier).symbol} doesnt contain property {property.symbol}");
				return new VT.NullVal();
			}


			return obj.properties[property.symbol];
		}
		else
		{
			bool isNum = expr.property.type == "NumericLiteral";
			if (!isNum)
			{
				error($"excepeted index of property in object {(expr.Object as AST.Identifier).symbol}");
				return new VT.NullVal();
			}

			int Index = (expr.property as AST.NumericLiteral).value;

			bool IsVaildIndex = obj.properties.Count - 1 >= Index;
			if (!IsVaildIndex)
			{
				error($"{(expr.Object as AST.Identifier).symbol} doesnt contains an index of {Index}");
				return new VT.NullVal();
			}
			return obj.properties.ElementAt(Index).Value;
		}
	}

	public static VT.RuntimeVal eval_numeric_binary_expr(VT.NumValue lhs, VT.NumValue rhs, string ooperator)
	{

        object? results = null;
		
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
			case "%":
				results = lhs.value % rhs.value;
				break;
			case "=":
				results = lhs.value == rhs.value;
				break;
			case ">":
				results = lhs.value > rhs.value;
				break;
			case "<":
				results = lhs.value < rhs.value;
				break;
			default:
				error($"Operation '{ooperator}' cannot be aplied on numbers\ngot => {rhs.value} {ooperator} {lhs.value}");
				break;
		}
		
		
		if(results is int) {
			return VT.MK_NUM((int) results);
		}
		else if(results is bool) {
			return VT.MK_BOOL((bool) results);
		}
		else {
			return VT.MK_NUM();
		}
	}

	public static VT.RuntimeVal eval_bool_binary_expr(VT.BooleanVal lhs, VT.BooleanVal rhs, string ooperator) {
		bool results = false;
		switch(ooperator) {
			case "&":
				results = lhs.value && rhs.value;
				break;
			case "&&":
				results = lhs.value & rhs.value;
				break;
			case "|":
				results = lhs.value || rhs.value;
				break;
			case "||":
				results = lhs.value | rhs.value;
				break;
			case "=":
				results = lhs.value == rhs.value;
				break;
			default:
				error($"cannot do operation {ooperator} on booleans\ngot => {rhs.value} {ooperator} {lhs.value}");
				break;
		}
		return VT.MK_BOOL(results);
	}
	public static VT.RuntimeVal eval_string_binary_expr(VT.StringVal lhs,VT.StringVal rhs, string ooperator) {
		object? results = null;
		
		switch(ooperator)
		{
			case "+":
				results = lhs.value + rhs.value;
				break;
			case "=":
				results = lhs.value == rhs.value;
				break;
			default:
				error($"cannot peform operation {ooperator} on string\ngot => {lhs.value} {ooperator} {rhs.value}");
				return new VT.NullVal();
		}
		
		if(results is string) {
			return VT.MK_NUM((int) results);
		}
		else if(results is bool) {
			return VT.MK_BOOL((bool) results);
		}
		else {
			return VT.MK_NULL();
		}
	}
}
