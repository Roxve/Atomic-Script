using System;
using System.Linq;
using System.Collections.Generic;
using static ValueTypes.VT;
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
	}
}
