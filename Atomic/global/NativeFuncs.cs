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
		public static void print(string type, RuntimeVal arg)
		{
			switch (type)
			{
				case "num":
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write((arg as NumValue).value);
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case "str":
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write((arg as StringVal).value);
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case "null":
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write("null");
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case "bool":
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.Write((arg as BooleanVal).value);
					Console.ForegroundColor = ConsoleColor.White;
					break;
				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write(arg.ToString());
					Console.ForegroundColor = ConsoleColor.White;
					break;
			}
		}
		
		
		public static RuntimeVal write(RuntimeVal[] args, Enviroment? env)
		{
			foreach (RuntimeVal arg in args)
			{
				print(arg.type,arg);
			}
			//auto add a line at the end of executing
			Console.WriteLine();
			return MK_NULL();
		}

		public static RuntimeVal prompt(RuntimeVal[] args, Enviroment? env)
		{
			print(args[0].type, args[0]);

			var results = Console.ReadLine();

			return MK_TYPE(results);
		}

		public static RuntimeVal read(RuntimeVal[] args, Enviroment? env)
		{
			var results = Console.ReadLine();
			return MK_TYPE(results);
		}
	}
}
