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
        public static List<Statement> body;
    }




    public class Expression : Statement {}





    public class BinaryExpression : Expression {
        public static string type = NodeType[3];
        public static Expression left;
        public static Expression right;
        public static string Operator;
    }




    public class Identifier : Expression {
        public static string type = NodeType[2];
        public static string symbol;
    }



    public class NumericLiteral : Expression {
        public static string type = NodeType[1];
        public static int value;
    }
}
}
