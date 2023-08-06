using System;
using System.Linq;
using static ValueTypes.VT;
using System.Threading;
using System.Collections.Generic;
namespace Atomic;


/* the program contains a main env 
   and each class and each function contains their own env
   with the parent env (env contains functions and vars)
*/


public partial class Global
{
	public static Enviroment createEnv()
	{
		Enviroment env = new Enviroment(null);



		//functions
		
		//write

		var writeCall = new functionCall();
		writeCall.execute = NativeFunc.write; env.declareVar("write", MK_NATIVE_FN(writeCall), true);
		
		//prompt
		var promptCall = new functionCall();
		promptCall.execute = NativeFunc.prompt; env.declareVar("prompt", MK_NATIVE_FN(promptCall), true);
		
		
		//read
		var readCall = new functionCall();
		readCall.execute = NativeFunc.read; env.declareVar("read", MK_NATIVE_FN(readCall), true);
		return env;
	}
}


public class Enviroment
{

	private void error(string message)
	{
		Console.BackgroundColor = ConsoleColor.Red;
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine(message);
		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine("press anything to exit");
		Console.ReadKey();
		Thread.CurrentThread.Interrupt();
	}
	private Enviroment? parent;
	private Dictionary<string, RuntimeVal> variables;
	private List<string> locked_variables;

	public Enviroment(Enviroment? parentENV)
	{
		this.parent = parentENV;
		this.variables = new Dictionary<string, RuntimeVal>();
		this.locked_variables = new List<string>();
	}

	public RuntimeVal declareVar(string name, RuntimeVal value, bool isLocked)
	{
		if (this.variables.ContainsKey(name))
		{
			this.error("var " + name + " already exit");
			return new NullVal();
		}

		this.variables.Add(name, value);
		if (isLocked)
		{
			this.locked_variables.Add(name);
		}
		return value;
	}


	public RuntimeVal setVar(string name, RuntimeVal value)
	{
		var env = this.resolve(name);
		if (env.locked_variables.Any(t => t == name))
		{
			error("cannot assign a value to a locked var!");
		}
		env.variables[name] = value;
		return value;
	}

	public RuntimeVal findVar(string name)
	{
		var env = this.resolve(name);
		return env.variables[name] as RuntimeVal;
	}

	public Enviroment resolve(string name)
	{
		if (this.variables.ContainsKey(name))
		{
			return this;
		}
		if (this.parent == null)
		{
			error("cannot resolve " + name);
			return this;
		}
		return this.parent.resolve(name);
	}
}

