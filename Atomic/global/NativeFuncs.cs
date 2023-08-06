using System;
using System.Linq;
using System.Collections.Generic;
using static ValueTypes.VT;
using CSharpShellCore;
namespace Atomic;

public partial class Global
{
	public static class NativeFunc
	{
		public static RuntimeVal write(RuntimeVal[] args, Enviroment? env)
		{
			foreach (RuntimeVal arg in args)
			{
				switch (arg.type)
				{
					case "num":
					    Console.ForegroundColor = ConsoleColor.Yellow;
						Console.Write((arg as NumValue).value);
						Console.ForegroundColor = ConsoleColor.White;
						continue;
					case "str":
						Console.ForegroundColor = ConsoleColor.Green;
						Console.Write((arg as StringVal).value);
						Console.ForegroundColor = ConsoleColor.White;
						continue;
					case "null":
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.Write("null");
						Console.ForegroundColor = ConsoleColor.White;
						continue;
					default:
					    Console.ForegroundColor = ConsoleColor.Red;
						Console.Write(arg.ToString());
						Console.ForegroundColor = ConsoleColor.White;
						continue;
				}
			}
			//auto add a line at the end of executing
			Console.WriteLine();
			return MK_NULL();
		}
		
		public static RuntimeVal prompt(RuntimeVal[] args, Enviroment? env) {
			switch (args[0].type)
				{
					case "num":
					    Console.ForegroundColor = ConsoleColor.Yellow;
						Console.Write((args[0] as NumValue).value);
						Console.ForegroundColor = ConsoleColor.White;
						break;
					case "str":
						Console.ForegroundColor = ConsoleColor.Green;
						Console.Write((args[0] as StringVal).value);
						Console.ForegroundColor = ConsoleColor.White;
						break;
					case "null":
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.Write("null");
						Console.ForegroundColor = ConsoleColor.White;
						break;
					default:
					    Console.ForegroundColor = ConsoleColor.Red;
						Console.Write(args[0].ToString());
						Console.ForegroundColor = ConsoleColor.White;
						break;
				}
				
				var results = Console.ReadLine();
				
				return MK_TYPE(results);
		}
		
		public static RuntimeVal read(RuntimeVal[] args, Enviroment? env) {
			var results = Console.ReadLine();
			return MK_TYPE(results);
		}
	}
}
