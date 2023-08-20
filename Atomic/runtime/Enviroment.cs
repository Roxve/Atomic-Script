using System;
using System.Linq;
using static ValueTypes.VT;
using ValueTypes;
using System.Threading;
using System.Drawing;
using Pastel;
using Atomic_AST;
using Atomic_debugger;
using System.Collections.Generic;
namespace Atomic_lang;


/* the program contains a main env 
   and each class and each function contains their own env
   with the parent env (env contains functions and vars)
*/
#nullable enable annotations



public class Enviroment
{

	private static void createFunc(string name, Func<RuntimeVal[],Enviroment,RuntimeVal> execute,Enviroment env) {
		var Call = new functionCall();
		Call.execute = execute;
		env.declareVar(name, MK_NATIVE_FN(Call), true, null);
	}

	public static Enviroment createEnv()
	{
		Enviroment env = new Enviroment(null);


		//functions
		
		//write
		createFunc("write", NativeFunc.write,env);
		
		//prompt
		createFunc("prompt", NativeFunc.prompt, env);
		
		//read
		createFunc("read", NativeFunc.read, env);
		
		//toLower
		createFunc("toLower",NativeFunc.toLower,env);

		//toUpper
		createFunc("toUpper", NativeFunc.toUpper, env);


		return env;
	}
	private void error(string message, Statement? stmt)
	{
		
		Console.WriteLine(("runtime error:\n" + message + $"\nat => line:{stmt.line} column:{stmt.column}").Pastel(Color.Yellow).PastelBg(Color.Red));
		if(!(Vars.mode == "repl")) {
			Console.WriteLine("press anything to exit".Pastel(Color.Gold));
			Console.ReadKey();
			Thread.CurrentThread.Interrupt();
		}
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
	

	public void addEnv(Enviroment env) {
	  //TODO: add to available_envs instead	
		foreach(var variable in env.variables) {
			if(this.variables.ContainsKey(variable.Key)) {
				continue;
			}
			else if(env.locked_variables.Contains(variable.Key)) {
				this.locked_variables.Add(variable.Key);
			}
			this.variables.Add(variable.Key,variable.Value);
		}
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
		if (env is null) {
			return MK_NULL();
		}
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
		if(env is null) {
			return MK_NULL();
		}
		return env.variables[name] as RuntimeVal;
	}


	public Enviroment? resolve(string name, Statement stmt)
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
		   return null;
		}
		
		return this.parent.resolve(name, stmt);
	}
}

