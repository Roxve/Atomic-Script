using System;
using System.Linq;
using Atomic_debugger;
using System.Drawing;
using Pastel;
using ValueTypes;
using System.IO;
using System.Diagnostics;




namespace Atomic_lang;

public static class Run
{
	public static void Main(String[] args)
	{
		if (args is null || args.Length <= 0)
		{
			Repl();
		}
		else
		{
			switch (args[0])
			{
				case "help":
				case "?":
				case "-h":
					Console.WriteLine("commands:\nno args: enter repl mode\nrun: runs a file\nrun?: runs a file and disaplays AST/IONS(tokens) for dev reasons");
					break;
				case "run":
					if (args.Length < 2)
					{
						Console.WriteLine("excepted file path to run at the next arg!");
						break;
					}
					string code = File.ReadAllText(args[1]);
					Run.aFile(code);
					break;
				case "run?":
					if (args.Length < 2)
					{
						Console.WriteLine("excepted file path to run at the next arg!");
						break;
					}
					string codet = File.ReadAllText(args[1]);
					Run.Test(codet);
					break;
				default:
					Repl();
					break;
			}
		}
	}
	public static int Repl()
	{
		ConsoleExtensions.Enable();
		var env = Enviroment.createEnv();
		Console.WriteLine("entered repl mode! use '?' for commands(outside repl)\n.exit to exit".Pastel(Color.DarkOrange));
		string code;
		Vars.mode = "repl";
		while (true)
		{
			
			Console.WriteLine("Atomic".Pastel(Color.Magenta));
			Console.Write("=>".Pastel(Color.Magenta));
			code = Console.ReadLine();
			if (code == ".exit")
			{
				break;
			}
			else
			{
				

				var ionizer = new Ionizer(code);
				var ionized_atoms = ionizer.ionize();


				var parser = new Parser(ionized_atoms);
				var parsed_ions = parser.productAST();

				var run = Interpreter.evaluate(parsed_ions, env);

				switch (run.type)
				{
					case "num":
						Console.WriteLine((run as NumValue).value.ToString().Pastel(Color.Gold));
						continue;
					case "str":
						Console.WriteLine((run as StringVal).value.Pastel(Color.Gold));
						continue;
					case "null":
						Console.WriteLine("null".Pastel(Color.OrangeRed));
						continue;
					default:
						continue;
				}
			}
		}
		return 0;
	}
	public static int Test(string code)
	{
		var env = Enviroment.createEnv();

		var ionizer = new Ionizer(code);
		var ionized_atoms = ionizer.ionize();
		Console.WriteLine(IonDumper.Dump(ionized_atoms));
		
		var parser = new Parser(ionized_atoms);
		var parsed_ions = parser.productAST();
		Console.WriteLine(ObjectDumper.Dump(parsed_ions));

		var run = Interpreter.evaluate(parsed_ions, env);

		return 0;
	}
	public static int aFile(string code)
	{
		var env = Enviroment.createEnv();

		var ionizer = new Ionizer(code);
		var ionized_atoms = ionizer.ionize();
		
		
		var parser = new Parser(ionized_atoms);
		var parsed_ions = parser.productAST();
		if(Vars.error) {
			return 1;
		}
		else {
			var run = Interpreter.evaluate(parsed_ions, env);
			if(Vars.error) {
				return 1;
			}
			return 0;
		}
	}
}
