using System;
using System.Collections.Generic;


namespace Atomic_AST {

public class AST {

    public static string[] NodeType = new string[4] {"Program","NumericLiteral","Identifier","BinaryExpr"};


    public class Statement {
         static string[] type = NodeType;    
    }



    // a program contains many statements
    public class Program : Statement {
        public string type = NodeType[0];
        public List<Statement> body {get; set;}
    }




    public class Expression : Statement {}





    public class BinaryExpression : Expression {
        public  static string type = NodeType[3];
        public  Expression left {get; set;}
        public  Expression right {get; set;}
        public  string Operator {get; set;}
    }




    public class Identifier : Expression {
        public static string type = NodeType[2];
        public string symbol {get; set;}
    }



    public class NumericLiteral : Expression {
        public static string type = NodeType[1];
        public int value {get; set;}
    }
}
}
