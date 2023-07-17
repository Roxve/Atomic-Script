using System;
using System.IO;
namespace Ttype {
    public enum TokenType
	{

		id,
		setter,
		op,
		//keyword
		keyword,
		//returnKeywords are keywords that give values for ex. random which gives a random number
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
}