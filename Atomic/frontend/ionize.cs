using System;
using System.Linq;
using System.Collections.Generic;
using Atomic_lang;
using Atomic_debugger;
using Pastel;
using System.Drawing;
namespace Atomic_lang;

public class Ionizer
{
	private string atoms;
	private List<Ion> ions;
	private int line = 1;
	private int column = 0;
	
	public Ionizer(string atoms)
	{
		this.atoms = atoms;
		this.ions = new List<Ion>();
	}

	private void error(string msg)
	{	
		Console.WriteLine(msg + $"\ngot => {this.atom()}\nat => line:{this.line}, column:{this.column}".Pastel(ConsoleColor.Yellow).PastelBg(ConsoleColor.Red));
		Vars.error = true;
		
		take();
	}

	public bool isNum(char x)
	{
		return "0123456789".Contains(x.ToString());
	}

	public bool isAllowedId(char i)
	{
		string x = i.ToString();
		return (x.ToUpper() != x.ToLower()) || "_".Contains(x) || isNum(i);
	}

	private void add(string value, IonType type)
	{
		Ion ion;
		ion.value = value;
		ion.line = this.line;
		ion.column = this.column;
		ion.length = value.Length;
		ion.type = type;
		ions.Add(ion);
	}


	private char take()
	{
		char prev;
		if (atoms.Length > 0)
		{
			prev = atoms[0];
			atoms = atoms.Substring(1);
			column++;
		}
		else
		{
			prev = ';';
		}
		return prev;
	}

	public bool isLine(char i)
	{
		return i.ToString() == "\n";
	}
	public char atom()
	{
		if (atoms.Length > 0)
		{
			return this.atoms[0];
		}
		else
		{
			return ';';
		}
	}

	//keywords and their tokens
	private List<(string value, IonType type)> keywords = new List<(string value, IonType type)> {
			("set", IonType.set_kw),
			("locked", IonType.locked_kw),
			("if", IonType.if_kw),
			("else", IonType.else_kw),
			("func", IonType.func_kw),
			("return", IonType.return_kw),
			("null", IonType.null_type),
			("true", IonType.bool_type),
			("false", IonType.bool_type)
	};
	
	
	
	public bool isKeyword(string x)
	{
		return keywords.Any(t => t.value == x);
	}
	
	public IonType GetKeyword(string x)
	{
		return keywords[keywords.FindIndex(t => t.value == x)].type;
	}
	
	public bool isOp(char x) {
		//no > because it can be a setter '>>'
		return "+-/*=<&".Contains(x);
	}
	
	public bool isSkippableChar(char i)
	{
		string x = i.ToString();
		return x[0] == ' ' || x == "\t" || x[0] == ';';
	}
	public List<Ion> ionize()
	{
		while (atoms.Length > 0)
		{

			if (isLine(atom()))
			{
				line++;
				column = 0;
				take();
				continue;
			}
			else if (isSkippableChar(atom()))
			{
				take();

			}
			//symbols
			else if (atom() == '(')
			{
				add(atom().ToString(), IonType.OpenParen);
				take();
			}
			else if (atom() == ')')
			{
				add(atom().ToString(), IonType.CloseParen);
				take();
			}
			else if (atom() == '{')
			{
				add(atom().ToString(), IonType.OpenBrace);
				take();
			}
			else if (atom() == '}')
			{
				add(atom().ToString(), IonType.CloseBrace);
				take();
			}
			else if (atom() == '[')
			{
				add(atom().ToString(), IonType.OpenBracket);
				take();
			}
			else if (atom() == ']')
			{
				add(atom().ToString(), IonType.CloseBracket);
				take();
			}
			else if (atom() == '.')
			{
				add(atom().ToString(), IonType.Dot);
				take();
			}
			else if (atom() == ':')
			{
				add(atom().ToString(), IonType.Colon);
				take();
			}
			else if (atom() == ',')
			{
				add(atom().ToString(), IonType.Comma);
				take();
			}
			else if (atom() == '|') {
				take();
				if(atom() == '|') {
					take();
					add("||", IonType.ooperator);
				}
				else {
					add("|", IonType.ooperator);
				}
			}
			
			
			else if(atom() == '>') {
				take();
				if(atom() == '>') {
					add(">>", IonType.setter);
					take();
				}
				else {
					add(">", IonType.ooperator);
				}
			}
			else if(isOp(atom())) {
				add(atom().ToString(), IonType.ooperator);
				take();
			}
			//detections systems
			else
			{
				if (isNum(atom()))
				{
					string res = "";

					while (isNum(atom()))
					{
						res += take();
					}
					add(res, IonType.num_type);
				}
				else if (isAllowedId(atom()))
				{
					string res = "";

					while (isAllowedId(atom()))
					{
						res += take();
					}
					
					if (isKeyword(res))
					{
						add(res, GetKeyword(res));
					}
					else
					{
						add(res, IonType.id);
					}
				}


				//strings
				else if (atom() == '"')
				{
					string res = "";
					take();
					while (atom() != '"' && atoms.Length > 0)
					{
						res += take();
					}
					if (atom() != '"')
					{
						error("unfinished string");
					}
					else
					{
						add(res, IonType.str_type);
						take();
					}
				}

				else if (atom().ToString() == "'")
				{
					string res = "";
					take();
					while (atom().ToString() != "'" && atoms.Length > 0)
					{
						res += take();
					}
					if (atom().ToString() != "'")
					{
                		error("unfinished string");
					}
					else
					{
						add(res, IonType.str_type);
						take();
					}
				}

				//comments
				else if (atom() == '#')
				{
					while (!isLine(atom()))
					{
						take();
					}
				}
				
				else {
					error("unknown char");
				}
			}

		}
		add("END", IonType.EOF);
		return this.ions;
	}
}
