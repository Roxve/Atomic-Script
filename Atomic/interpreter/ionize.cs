using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.IO;
namespace Atomic;

public class ionizing
{
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
	public ionizing(string code)
	{
		atoms = code;
	}
	public static string atoms { get; set; }
	public string[] keywords = { "write", "set" };
	public static List<(string value, TokenType type)> ions = new List<(string value, TokenType type)>();
	public static int column = 1;
	public static int line = 1;
	public enum TokenType
	{

		id,
		setter,
		op,
		//keyword
		keyword,
		//valueKeywords are keywords that give values for ex. random which gives a random number
		returnKeyword,
		//types
		line,
		num,
		str,
		//barackets
		OpenParen,
		CloseParen,
		EOF,
	}


	public bool isAllowedID(char x)
	{
		//only english && langs that has upper and lower chars is allowed
		return "_qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM".Contains(x);
	}
	public bool IsSkippable(char n)
	{
		string x = n.ToString();
		return x == " " || x == "\t" || x == "\r";
	}
	public bool IsLine(string x)
	{
		return x == "\n";
	}
	public bool isNum(char n)
	{
		string x = n.ToString();
		//getting int by checking if the char code of the string is begger or smaller or equal to (0-9) charcode
		return "0123456789".Contains(x);
	}
	public bool isKeyword(string x)
	{

		return keywords.Contains(x);
	}


	public static void move(int by = 1)
	{
		while (by != 0)
		{
			atoms = atoms.Substring(1);
			column++;
			by--;
		}
	}

	public List<(string value, TokenType type)> ionize()
	{

		while (atoms.Length > 0)
		{

			if (IsLine(atoms[0].ToString()))
			{
				line++;
				ions.Add((line.ToString(), TokenType.line));

				column = 1;
				move();
			}


			else if (IsSkippable(atoms[0]))
			{
				move();
			}


			//detecting strings
			else if (atoms[0] == '"')
			{
				string res = "";
				move();
				try
				{
					while (atoms[0] != '"' && atoms.Length > 0)
					{
						res += atoms[0];
						move();
					}
				}
				catch
				{
					error("reached end of file and didnt finish string");
				}
				if (atoms[0] != '"')
				{
					error("unfinshed string");
				}

				ions.Add((res, TokenType.str));
				move();
			}


			else if (atoms[0] == '(')
			{
				ions.Add(("(", TokenType.OpenParen));
				move();
			}


			else if (atoms[0] == ')')
			{
				ions.Add((")", TokenType.CloseParen));
				move();
			}
			else if (isNum(atoms[0]))
			{
				string res = "";
				while (isNum(atoms[0]))
				{
					res += atoms[0];
					move();
				}
				ions.Add((res, TokenType.num));
			}




			else if (isAllowedID(atoms[0]))
			{
				string res = "";
				while (isAllowedID(atoms[0]) && atoms.Length > 0)
				{
					res += atoms[0];
					move();
				}
				if (isKeyword(res))
				{
					ions.Add((res, TokenType.keyword));

				}
				else
				{
					ions.Add((res, TokenType.id));

				}
			}
		}
		ions.Add(("END", TokenType.EOF));
		return ions;
	}
}
