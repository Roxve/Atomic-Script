using System;
using System.Linq;
using System.Collections.Generic;
using static ValueTypes.VT;
using System.Threading;
namespace Atomic;


/* the program contains a main env 
   and each class and each function contains their own env
   with the parent env (env contains functions and vars)
*/
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

	public Enviroment(Enviroment? parentENV)
	{
		this.parent = parentENV;
		this.variables = new Dictionary<string, RuntimeVal>();
	}

	public RuntimeVal declareVar(string name, RuntimeVal value)
	{
		if (this.variables.ContainsKey(name))
		{
			this.error("var " + name + " already exit");
			return new NullVal();
		}
		
		this.variables.Add(name, value);
		return value;
	}
	
	
	public RuntimeVal setVar(string name,RuntimeVal value) {
		var env = this.resolve(name);
		env.variables[name] = value;
		return value;
	}
	
	public RuntimeVal findVar(string name) {
		var env = this.resolve(name);
		return env.variables[name] as RuntimeVal;
	}
	
	public Enviroment resolve(string name) {
		if(this.variables.ContainsKey(name)) {
			return this;
		}
		if(this.parent == null) {
			error("cannot resolve " + name);
			return this;
		}
		return this.parent.resolve(name);
	}
}
