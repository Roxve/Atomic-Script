using System;
using System.Linq;
using System.Collections.Generic;
using ValueTypes;
using static ValueTypes.VT;
using Atomic_AST;
using System.Threading;

namespace Atomic_lang;

public partial class interpreter
{
	public static RuntimeVal eval_id(Identifier id, Enviroment env)
	{
		var value = env.findVar(id.symbol);
		return value;
	}



	//acts as a return to the right
	public static RuntimeVal eval_binary_expr(BinaryExpression binary, Enviroment env)
	{
		var lhs = evaluate(binary.left, env); var rhs = evaluate(binary.right, env);
		if (lhs.type.ToString() == "num" && rhs.type.ToString() == "num")
		{
			return eval_numeric_binary_expr(lhs as NumValue, rhs as NumValue, binary.Operator.value, binary);
		}
		
		else if(lhs.type == "str" && rhs.type == "str") {
			return eval_string_binary_expr(lhs as StringVal,rhs as StringVal, binary.Operator.value, binary);
		}
		
		else if(lhs.type == "bool" && rhs.type == "bool") {
			return eval_bool_binary_expr(lhs as BooleanVal, rhs as BooleanVal,binary.Operator.value, binary);
		}
		
		return new NullVal();
	}
	

	public static RuntimeVal eval_assignment(AssignmentExpr node, Enviroment env)
	{
		
		
	
		if (node.assigne.type == "Identifier")
		{
			var name = (node.assigne as Identifier).symbol;
			return env.setVar(name, evaluate(node.value, env));
		}
		
		else if (node.assigne.type == "MemberExpr")
		{
			 
			var expr = (node.assigne as MemberExpr);

			var Obj = eval_id(expr.Object as Identifier, env);

			if (Obj.type != "obj")
			{
				error($"excepted 'obj'\ngot => {expr.Object.type}", expr.Object);
				return new NullVal();
			}
			 

			ObjectVal obj = Obj as ObjectVal;
			if (!expr.computed)
			{
				 
				Identifier property = (expr.property as Identifier);


				if (!obj.properties.ContainsKey(property.symbol))
				{
					error($"{(expr.Object as Identifier).symbol} doesnt contain property {property.symbol}", expr.property);
					return new NullVal();
				}

                 
				obj.properties[property.symbol] = evaluate(node.value,env);
				 
				return obj.properties[property.symbol];
			}
			else
			{
				bool isNum = expr.property.type == "NumericLiteral";
				if (!isNum)
				{
					error($"excepeted index of property in object {(expr.Object as Identifier).symbol}", expr.Object);
					return new NullVal();
				}

				int Index = (expr.property as NumericLiteral).value;

				bool IsVaildIndex = obj.properties.Count - 1 >= Index;
				if (!IsVaildIndex)
				{
					error($"{(expr.Object as Identifier).symbol} doesnt contains an index of {Index}", expr.Object);
					return new NullVal();
				}
				var name = obj.properties.ElementAt(Index).Key;
				obj.properties[name] = evaluate(node.value,env);
				
				return obj.properties[name];
			}
		}
		else
		{
			error($"invaild token type in assignment\ngot type => {node.assigne.type}", node.assigne);
			return new NullVal();
		}
	}

	public static RuntimeVal eval_call_expr(CallExpr expr, Enviroment env)
	{
		var args = expr.args.ConvertAll<RuntimeVal>(arg => evaluate(arg, env)).ToArray();

		var fn = evaluate(expr.caller, env);

		switch(fn.type) {
			case "native-fn":
				var results = (fn as NativeFnVal).call(args, env);
				return results;
			case "func":
				var func = (fn as FuncVal);
				var funcEnv = new Enviroment(func.env);
				
				for(int x = 0; x < func.parameters.Count; x++) {
					var name = func.parameters[x];
					
					funcEnv.declareVar(name, args[x], false);
				}
				RuntimeVal result = MK_NULL();
				RuntimeVal last;
				foreach(Statement stmt in func.body) {
					//TODO add unreachable code warning
					last = evaluate(stmt, funcEnv);
					if(last.type == "return") {
						result = (last as ReturnVal).value;
						break;
					}
				}
				return result;
			default:
				error("Cannot call a value that is not a Function \ngot => " + fn.type,expr);
				return new NullVal();
		}
	}

	public static RuntimeVal eval_object_expr(ObjectLiteral obj, Enviroment env)
	{
		ObjectVal Obj = new ObjectVal(); Obj.properties = new Dictionary<string, RuntimeVal>();

		foreach (Property property in obj.properties)
		{
			RuntimeVal runtimeVal = new RuntimeVal();

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

	public static RuntimeVal eval_member_expr(MemberExpr expr, Enviroment env)
	{
		var Obj = eval_id(expr.Object as Identifier, env);

		if (Obj.type != "obj")
		{
			error($"excepted 'obj'\ngot => {expr.Object.type}", expr.Object);
			return new NullVal();
		}

		ObjectVal obj = Obj as ObjectVal;
		if (!expr.computed)
		{
			Identifier property = (expr.property as Identifier);


			if (!obj.properties.ContainsKey(property.symbol))
			{
				error($"{(expr.Object as Identifier).symbol} doesnt contain property {property.symbol}", expr.property);
				return new NullVal();
			}


			return obj.properties[property.symbol];
		}
		else
		{
			bool isNum = expr.property.type == "NumericLiteral";
			if (!isNum)
			{
				error($"excepeted index of property in object {(expr.Object as Identifier).symbol}",expr.Object);
				return new NullVal();
			}

			int Index = (expr.property as NumericLiteral).value;

			bool IsVaildIndex = obj.properties.Count - 1 >= Index;
			if (!IsVaildIndex)
			{
				error($"{(expr.Object as Identifier).symbol} doesnt contains an index of {Index}",expr.Object);
				return new NullVal();
			}
			return obj.properties.ElementAt(Index).Value;
		}
	}

	public static RuntimeVal eval_numeric_binary_expr(NumValue lhs, NumValue rhs, string ooperator, Expression expr)
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
				error($"Operation '{ooperator}' cannot be aplied on numbers\ngot => {rhs.value} {ooperator} {lhs.value}",expr);
				break;
		}
		
		
		if(results is int) {
			return VT.MK_NUM((int) results);
		}
		else if(results is bool) {
			return VT.MK_BOOL((bool) results);
		}
		else {
			return MK_NUM();
		}
	}

	public static RuntimeVal eval_bool_binary_expr(BooleanVal lhs, BooleanVal rhs, string ooperator, Expression expr) {
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
				error($"cannot do operation {ooperator} on booleans\ngot => {rhs.value} {ooperator} {lhs.value}", expr);
				break;
		}
		return MK_BOOL(results);
	}
	public static RuntimeVal eval_string_binary_expr(StringVal lhs,StringVal rhs, string ooperator,Expression expr) {
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
				error($"cannot peform operation {ooperator} on string\ngot => {lhs.value} {ooperator} {rhs.value}", expr);
				return new NullVal();
		}
		
		if(results is string) {
			return VT.MK_STR((string) results);
		}
		else if(results is bool) {
			return VT.MK_BOOL((bool) results);
		}
		else {
			return VT.MK_NULL();
		}
	}
}
