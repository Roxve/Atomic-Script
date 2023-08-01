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

		
		//

		Comma,
  		Colon,
  		Semicolon,
  		OpenParen, // (
  		CloseParen, // )
  		OpenBrace, // {
  		CloseBrace, // }


		//END OF FILE
		EOF,
	}
}