using System;
using System.Linq;
using System.IO;
using Atomic;
using Atomic_AST;
using System.ComponentModel;


namespace Atomic;

public static class Run
{

	public static void Main(String[] args)
	{
		string code;
		switch (args[0])
		{
			case "run":
				if (args.Length < 2)
				{
					Console.WriteLine("please give file to run");
					return;
				}
				code = File.ReadAllText(@"" + args[1]);
				break;
			case "help":
				Console.WriteLine("usage 'atomic [command]'\nhelp or ?: disaplys this\nrun [file]: runs the file");
				return;
			case "?":
				Console.WriteLine("usage 'atomic [command]'\nhelp or ?: disaplys this\nrun [file]: runs the file");
				return;
			default:
				Console.WriteLine("usage 'atomic [command]' use command help for help!");
				return;
		}
		Console.WriteLine("running?: {0}", true);

		var ionize = new Ionizing(code);
		var ionized_code = ionize.ionize();
		Console.WriteLine("ionized?: {0}", true);
		var parse = new Parser(ionized_code);
		AST.Program Program = parse.productAST();
		Console.WriteLine("parsed?: {0}", true);
		foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(Program)) {
			string name = descriptor.Name;
			object value = descriptor.GetValue(Program);
			
			
			Console.WriteLine("{0} = {1}", name,value);
		}

	}
}
