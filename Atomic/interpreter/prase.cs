using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Ttype;
namespace Atomic;
public class Parsing {

    List<(string value, TokenType type)> ions;
    public Parsing(List<(string value, TokenType type)> ions) {
        this.ions = ions;
    }
    

    //vars
    public static line = 1;
    public static column = 1;
    public List<(string name, TokenType type, string value)> vars;
    public void error(string message)
	{
		Console.BackgroundColor = ConsoleColor.Red;
		Console.ForegroundColor = ConsoleColor.Yellow;

		Console.WriteLine(message + "\nat => line:{0}, column:{1}", line, column);
		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.White;
        
		Console.WriteLine("press anything to exit");
		Console.ReadKey();
		Environment.Exit(1);

	}

    
}