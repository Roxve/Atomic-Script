using Atomic_AST;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using Atomic;

namespace ValueTypes;

public class VT
{


	//just to remmber type names and we might need that later
	public static string ValueType = "null,num,str,bool,obj";


	public class RuntimeVal
	{
		public string type = ValueType;
	}
	public class NullVal : RuntimeVal
	{
		public NullVal()
		{
			type = "null";
		}
		public static string value = "null";
	}


	public class NumValue : RuntimeVal
	{
		public NumValue()
		{
			type = "num";
		}
		public int value { get; set; }

	}

	public class StringVal : RuntimeVal
	{
		public StringVal()
		{
			type = "str";
		}

		public string value { get; set; }
	}
	public class BooleanVal : RuntimeVal
	{
		public BooleanVal()
		{
			type = "bool";
		}
		public bool value { get; set; }
	}

	public class ObjectVal : RuntimeVal
	{
		public ObjectVal()
		{
			type = "obj";
		}
		public Dictionary<string, RuntimeVal> properties { get; set; }
	}

	public class functionCall : RuntimeVal
	{	
		public functionCall()
		{
			type = "functionCall";
		}
		public Func<RuntimeVal[],Enviroment?,RuntimeVal> execute;
		private RuntimeVal[] args;
		public RuntimeVal[] Args
		{
			get {return this.args;}
			set {
				this.args = value;
			}
		}
		public Enviroment? env { get; set; }


	}

	public class NativeFnVal : RuntimeVal
	{
		public NativeFnVal()
		{
			type = "native-fn";
		}

		public functionCall callVal {get; set;}
		
		public VT.RuntimeVal call(RuntimeVal[] args,Enviroment env) {
			callVal.Args = args;
			callVal.env = env;
			var results = callVal.execute(args,env);
		
			return results;
		}
	}
    
	public class FuncVal : RuntimeVal {
		public FuncVal() {
			type = "func";
		}
		public string name {get; set;}
		public string[] parameters {get; set;}
		
		public Enviroment env {get; set;}
		
		private List<AST.Statement> Body = new List<AST.Statement>();
		
		public List<AST.Statement> body {
			get {
				return this.Body;
			}
			set {
				this.Body = value;
			}
		}
	}

	public static NativeFnVal MK_NATIVE_FN(functionCall call)
	{
		NativeFnVal func = new NativeFnVal();

		func.callVal = call;

		return func;
	}

	public static BooleanVal MK_BOOL(bool value = false)
	{
		var Inew = new BooleanVal(); Inew.value = value;
		return Inew;
	}


	public static StringVal MK_STR(string value = "unknown")
	{
		var Inew = new StringVal(); Inew.value = value;
		return Inew;
	}

	public static NumValue MK_NUM(int num = 0)
	{
		var Inew = new NumValue(); Inew.value = num;

		return Inew;
	}

	public static NullVal MK_NULL()
	{
		return new NullVal();
	}
}