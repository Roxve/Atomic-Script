﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using Ttype;
namespace Atomic;

public class Ionizing
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
	public Ionizing(string code)
	{
		atoms = code;
	}
	public static string atoms { get; set; }
	public string[] keywords = { "write", "set" };
	public static List<(string value, TokenType type)> ions = new List<(string value, TokenType type)>();
	public static int column = 1;
	public static int line = 1;



	public bool isAllowedID(char x)
	{
		//only english && langs that has upper and lower chars is allowed
		return "_qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM".Contains(x);
	}
	public bool isOp(char x)
	{
		return "+-/*".Contains(x);
	}
	public char current_atom() {
		if(atoms.Length > 0) {
			return atoms[0];
		}
		return ';';
	}
	public bool IsSkippable(char n)
	{
		string x = n.ToString();
		return x == " " || x == "\t" || x == "\r" || x == ";";
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
	public static void SetAtomsForRepl(string code) {
		atoms = code;
	}
	public List<(string value, TokenType type)> ionize()
	{
		
		while (atoms.Length > 0)
		{

			if (IsLine(current_atom().ToString()))
			{
				line++;


				column = 1;
				move();
			}


			else if (IsSkippable(current_atom()))
			{
				move();
			}


			//detecting strings
			else if (current_atom() == '"')
			{
				string res = "";
				move();
				try
				{
					while (current_atom() != '"' && atoms.Length > 0)
					{
						res += current_atom();
						move();
					}
				}
				catch
				{
					error("reached end of file and didnt finish string");
				}
				if (current_atom() != '"')
				{
					error("unfinshed string");
				}

				ions.Add((res, TokenType.str));
				move();
			}


			else if (current_atom() == '(')
			{
				ions.Add(("(", TokenType.OpenParen));
				move();
			}


			else if (current_atom() == ')')
			{
				ions.Add((")", TokenType.CloseParen));
				move();
			}
			else if (isNum(current_atom()))
			{
				string res = "";

				while (isNum(current_atom()))
				{
					res += current_atom();
					move();
				}
				ions.Add((res, TokenType.num));
			}




			else if (isAllowedID(current_atom()))
			{
				string res = "";

				while (isAllowedID(current_atom()))
				{
					res += current_atom();
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


			else if (isOp(current_atom()))
			{
				string res = current_atom().ToString();
				ions.Add((res, TokenType.op));
				move();
			}
			else
			{
				error("unknown char");
			}
		}


		ions.Add(("END", TokenType.EOF));
		return ions;
	}
}