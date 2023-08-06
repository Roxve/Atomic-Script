using System;
using System.IO;
namespace Ttype {
    public enum TokenType
	{

		id,
		setter,
		op,
		//keywords	
		set,
		func,
		return_kw,
		
		locked, //makes stuff not modifable
		static_kw,
		
		
		//types
		Null,
		line,
		num,
		str,
		Bool,


		//symbols

		Comma,
  	  Colon,
		Dot,
  	  Semicolon,
  	  OpenParen, // (
        CloseParen, // )
  	  OpenBrace, // {
  	  CloseBrace, // }
        OpenBracket, // [
		CloseBracket, // ]

		//END OF FILE
		EOF,
	}
}