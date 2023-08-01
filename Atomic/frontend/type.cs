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
		write,
		
		
		
		locked, //makes stuff not modifable
		static_word,
		
		//types
		Null,
		line,
		num,
		str,

		boolean,


		//

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