using System;
using System.Linq;
using System.Collections.Generic;
using static ValueTypes.VT;


namespace Atomic;
#nullable enable
#nullable disable warnings
public partial class Global
{
	public static class NativeFunc
	{
		private static NullVal error(string message) {
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("warning function error: ", message,"\nreturning null...");
			Console.ForegroundColor = ConsoleColor.White;
			return MK_NULL();
		}
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
			foreach(RuntimeVal arg in args) {
				print(arg.type, arg);
			}
			

			var results = Console.ReadLine();

			return MK_TYPE(results);
		}

		public static RuntimeVal read(RuntimeVal[] args, Enviroment? env)
		{
			var results = Console.ReadLine();
			return MK_TYPE(results);
		}
		public static RuntimeVal toLower(RuntimeVal[] args, Enviroment? env) {
			if(args.Length < 1) {
				return error("toLower Takes one aurgment!");
			}
			if(args[0].type != "str") {
				return error("excepted string in toLower function");
			}
			return MK_STR((args[0] as StringVal).value.ToLower());
		}
		public static RuntimeVal toUpper(RuntimeVal[] args, Enviroment? env) {
			if(args.Length < 1) {
				return error("toUpper Takes one aurgment!");
			}
			if(args[0].type != "str") {
				return error("excepted string in toUpper function");
			}
			return MK_STR((args[0] as StringVal).value.ToUpper());
		}
	}
}
