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
		    foreach(RuntimeVal arg in args) {
				switch(arg.type) {
					case "num":
					Console.Write((arg as NumValue).value);
					continue;
				default:
					Console.Write(arg.ToString());
					continue;
				}
			}
			//auto add a line at the end of executing
			Console.WriteLine();
			return MK_NULL();
		}
	}
}
