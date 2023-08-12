using System;
using System.Linq;
using System.Collections.Generic;
using Atomic_AST;
using System.Drawing;
using Pastel;
using Atomic_debugger;

namespace Atomic_lang;

public partial class Parser
{
	private List<Ion> ions;
	public Parser(List<Ion> ions)
	{
		this.ions = ions;
	}
	private int line = 1;
	private int column = 1;
	private void error(string msg, Ion ion) {
		Console.WriteLine((msg + $"\ngot => value:{ion.value} type:{ion.type}\nat => line:{ion.line}, column:{ion.column + ion.length}").Pastel(Color.Yellow).PastelBg(Color.Red));
		
		Vars.error = true;
	}
	
	private bool NotEOF()
	{
		if (ions.Count <= 0)
		{
			return false;
		}
		return this.ions[0].type != IonType.EOF;
	}
	
	private Ion at() {
		Ion end = new Ion();
		if(ions.Count <= 0) {
		    end.type = IonType.EOF;
			end.line = this.line; end.column = this.column;
			end.value = "END";
			
			return end;
		}
		
		Update();
		return this.ions[0];
	}
	
	private void Update() {
		if(ions.Count > 0) {
			this.line = this.ions[0].line;
			this.column = this.ions[0].line;
		}
	}
	
	// allows me to provid values and maybe set default values later on
	private T Create<T>() where T : new() {
		T stmt = new T();
		
		(stmt as Statement).line = this.at().line; (stmt as Statement).column = this.at().column;
		return stmt;
	}
	Ion previous_ion;
	
	private Ion except(IonType correct_type) {
		
		Ion prev = new Ion();
		if(ions.Count > 0) {
			prev = ions[0];
			ions.RemoveAt(0);
		}
		else {
			prev.type = IonType.EOF;
			prev.value = "END"; 
			prev.line = this.line; prev.column = this.column;
		}
		if (prev.type != correct_type)
		{	
			this.error("Parser error, excepting " + correct_type,prev);
		}
		column++;
		this.previous_ion = prev;
		
		Update();
		return prev;
	}
	
	private Ion take() {
		var prev = ions[0];
		ions.RemoveAt(0);
		this.previous_ion = prev;
		Update();
		return prev;
	}
	
	public Program productAST() {
		Program program = Create<Program>();
		while(NotEOF()) {
			program.body.Add(this.parse_statement());
		}
		return program;
	}
	
	private Statement parse_statement() {
		switch(this.at().type) {
			case IonType.set_kw:
				return this.parse_var_declaration();
			case IonType.return_kw:
			    return this.parse_return_stmt();
			default:
				return this.parse_expr();
		}
	}
	
	private Expression parse_expr() {
		return this.parse_assigment_expr();
	}
}
