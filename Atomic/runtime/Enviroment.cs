using System;
using System.Linq;
using static ValueTypes.VT;
using ValueTypes;
using System.Threading;
using System.Drawing;
using Pastel;
using Atomic_AST;
using System.Collections.Generic;
namespace Atomic_lang;


/* the program contains a main env 
   and each class and each function contains their own env
   with the parent env (env contains functions and vars)
*/
#nullable enable annotations



public class Enviroment
{
	
	public static Enviroment createEnv()
	{
		Enviroment env = new Enviroment(null);



		//functions
		
		//write

		var writeCall = new functionCall();
		writeCall.execute = NativeFunc.write; env.declareVar("write", MK_NATIVE_FN(writeCall), true, null);
		
		//prompt
		var promptCall = new functionCall();
		promptCall.execute = NativeFunc.prompt; env.declareVar("prompt", MK_NATIVE_FN(promptCall), true, null);
		
		
		//read
		var readCall = new functionCall();
		readCall.execute = NativeFunc.read; env.declareVar("read", MK_NATIVE_FN(readCall), true, null);
		
		//toLower
		var toLowerCall = new functionCall();
		toLowerCall.execute = NativeFunc.toLower; env.declareVar("toLower", MK_NATIVE_FN(toLowerCall), true, null);
		return env;
	}
	private void error(string message, Statement? stmt)
	{
		
		Console.WriteLine(("runtime error:\n" + message + $"\nat => line:{stmt.line} column:{stmt.column}").Pastel(Color.Yellow).PastelBg(Color.Red));
		
		Console.WriteLine("press anything to exit".Pastel(Color.Gold));
		Console.ReadKey();
		Thread.CurrentThread.Interrupt();
	}
	private Enviroment? parent;
	private Dictionary<string, RuntimeVal> variables;
	private List<string> locked_variables;
	private List<Enviroment> available_envs;

	public Enviroment(Enviroment? parentENV)
	{
		this.parent = parentENV;
		this.variables = new Dictionary<string, RuntimeVal>();
		this.locked_variables = new List<string>();
		this.available_envs = new List<Enviroment>();
	}

	public RuntimeVal declareVar(string name, RuntimeVal value, bool isLocked, Statement? stmt)
	{
		if (this.variables.ContainsKey(name))
		{
			this.error("var " + name + " already exit", stmt);
			return new NullVal();
		}

		this.variables.Add(name, value);
		if (isLocked)
		{
			this.locked_variables.Add(name);
		}
		return value;
	}


	public RuntimeVal setVar(string name, RuntimeVal value, Statement stmt)
	{
		var env = this.resolve(name, stmt);
		if (env.locked_variables.Any(t => t == name))
		{
			error("cannot assign a value to a locked var!", stmt);
		}
		env.variables[name] = value;
		return value;
	}

	public RuntimeVal findVar(string name, Statement stmt)
	{
		var env = this.resolve(name, stmt);
		return env.variables[name] as RuntimeVal;
	}


	public Enviroment resolve(string name, Statement stmt)
	{
		if (this.variables.ContainsKey(name))
		{
			return this;
		}
		foreach(Enviroment env in available_envs) {
			if(env.variables.ContainsKey(name)) {
				return env;
			}
		}
		if (this.parent == null)
		{
			 error("cannot resolve " + name, stmt);
		     return this;
		}
		
		return this.parent.resolve(name, stmt);
	}
}

