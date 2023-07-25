using System;

namespace Atomic;
public static class AST_Types {
    public static string[] NodeType = new string[4] {"Program","NumericLiteral","Identifier","BinaryExpr"};


    public interface Statement {
        public static string[] type = NodeType;
    }

    // a program contains many statement
    public interface Program : Statement {
        public static string[] type = new string[1] {NodeType[0]};
        public static Statement body;
    }

    public interface Expression : Statement {}

    public interface BinaryExpression : Expression {
        public static string[] type = new string[1] {NodeType[3]};
        public static Expression left;
        public static Expression right;
        public static string Operator;
    }

    public interface Identifier : Expression {
        public static string[] type = new string [1] {NodeType[2]};
        public static string symbol;
    }

    public interface NumericLiteral : Expression {
        public static string[] type = new string[1] {NodeType[1]};
        public static int value;
    }
}