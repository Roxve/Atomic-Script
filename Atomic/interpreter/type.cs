using System;
using System.IO;
namespace Ttype {
    public enum TokenType
	{

		id,
		setter,
		op,
		//keywords
		keyword,
		ignored_keyword, // keywords thats ignored to add code readabilty
		
		set,
		write,
		//types
		line,
		num,
		str,
		//barackets
		OpenParen,
		CloseParen,
		EOF,
	}
}