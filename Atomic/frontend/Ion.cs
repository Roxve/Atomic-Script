using System;
using System.Linq;
using System.Collections.Generic;

namespace Atomic_lang;

public struct Ion {
	public IonType type;
	public string value;
	public int length;
	public int line;
	public int column;
}
public enum IonType {
	id,
	setter,
	ooperator,
	
	//keywords
	return_kw,
	if_kw,
	else_kw,
	set_kw,
	func_kw,
	locked_kw,
	use_kw,
	using_kw,
	
	//types
	null_type,
	num_type,
	str_type,
	bool_type,
	
	
	//symbols
	OpenParen, //(
	CloseParen, //)
	OpenBrace, //{
	CloseBrace, //}
	OpenBracket, //[
	CloseBracket, //]
	Dot,
	Colon,
	Comma,
	
	
	//END OF FILE
	EOF,
}
