
namespace ValueTypes;

public class VT {
    public static string[] ValueType = new string[3] {"null", "number","string"};


    public class RuntimeVal {
        static string[] type = ValueType;
    }
    public class NullVal : RuntimeVal {
        public static string type = ValueType[0];
        public static string value = "null";
    }


    public class NumValue : RuntimeVal {
        public static string type = ValueType[1];
        public int value {get; set;}
    }

    public class StringVal : RuntimeVal {
        public static string type = ValueType[2];

        public string value {get; set;}
    }

}