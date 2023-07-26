using System;
using Ttype;
using System.Collections.Generic;
using Atomic_AST;
namespace Atomic;

// producting a vaild AST from atoms

public class Parser {
    private List<(string value, TokenType
    type)> ions;
    public Parser(List<(string value, TokenType type)> ions) {
        this.ions = ions;
    }


    //Checking if we reached didnt reach end of file yet?
    private bool NotEOF() {
        return this.ions[0].type != TokenType.EOF;
    }


    private TokenType current_token_type() {
        return ions[0].type;
    }
    private string current_token_value() {
        return ions[0].value;
    }

    //removes first ion and return it
    private (string value, TokenType type) move() {
        var prev = ions[0];
        
        ions.RemoveAt(0);


        return prev;
    }


}
