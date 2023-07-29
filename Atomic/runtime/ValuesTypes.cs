
namespace ValueTypes;

public class VT {
    public static string[] ValueType = new string[3] {"null", "number","string"};

    public class NullVal {
        static string type = ValueType[0];
        static string value = "null";
    }


    public class NumValue : NullVal {
        public static string type = ValueType[1];
        public int value {get; set;}
    }

    public class StringVal : NullVal {
        public static string type = ValueType[2];

        public string value {get; set;}
    }

}