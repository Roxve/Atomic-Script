using System;
using System.Linq;
using System.IO;
using Atomic;
using Atomic_AST;
using System.ComponentModel;
using Atomic_debugger;
using System.Threading;


namespace Atomic;

public static class Run
{

	public static void Main(String[] args)
	{
		string code;
		if (args.Count() <= 0 || args == null)
		{
			repl();
		}
		else
		{
			switch (args[0])
			{
				case "run":
					if (args.Length < 2)
					{
						Console.WriteLine("please give file to run");
						return;
					}
					code = File.ReadAllText(@"" + args[1]);
					Runp(code);
					break;
				case "run?":
					if (args.Length < 2)
					{
						Console.WriteLine("please give file to run");
						return;
					}
					code = File.ReadAllText(@"" + args[1]);

					TestRun(code);
					return;
				case "help":
					Console.WriteLine("usage 'atomic [command]'\nhelp or ?: disaplys this\nrun [file]: runs the file");
					return;
				case "?":
					Console.WriteLine("usage 'atomic [command]'\nhelp or ?: disaplys this\nrun [file]: runs the file");
					return;
				default:
					repl();
					return;
			}
		}
	}
	public static void Runp(string code)
	{
		var ionize = new Ionizing(code);
		var ionized_code = ionize.ionize();
		var parse = new Parser(ionized_code);
		AST.Program Program = parse.productAST();
		var result = interpreter.evaluate(Program);
	}
	public static void TestRun(string code)
	{
		vars.test = true;
		Console.WriteLine("running?: {0}", true);

		var ionize = new Ionizing(code);
		var ionized_code = ionize.ionize();
		Console.WriteLine("ionized?: {0}", true);
		var parse = new Parser(ionized_code);
		AST.Program Program = parse.productAST();
		Console.WriteLine("parsed?: {0}", true);

		var dump = ObjectDumper.Dump(Program);

		Console.WriteLine("Program?");
		Console.WriteLine(dump);

		var result = interpreter.evaluate(Program);

		Console.WriteLine("results?");
		var dump_results = ObjectDumper.Dump(result);
		Console.WriteLine(dump_results);
	}
	public static void repl()
	{
		string code;
		Console.WriteLine("Type Commands to run in atomic! (.exit to exit)");
		while (true)
		{
			//for now you can write one expr and it returns correctly but after that one it returns null?
			Console.WriteLine("Atomic");
			Console.Write("=>");
			code = Console.ReadLine();
			code = code.Trim();
			if (code == "exit")
			{
				Thread.CurrentThread.Interrupt();
			}
			var ionize = new Ionizing(code);
			Ionizing.SetAtomsForRepl(code);
			var ionized_code = ionize.ionize();
			Console.WriteLine(string.Join(" ", ionized_code));
			var parse = new Parser(ionized_code);
			AST.Program Program = parse.productAST();
			var result = interpreter.evaluate(Program);
			var dump_results = ObjectDumper.Dump(result);
		    Console.WriteLine(dump_results);
		}
	}
}
