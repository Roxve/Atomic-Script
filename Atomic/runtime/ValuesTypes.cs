using Atomic_AST;

namespace ValueTypes;

public class VT {
    public static string ValueType = "null,number,string";


    public class RuntimeVal {
        public static string type = ValueType;
    }
    public class NullVal : RuntimeVal {
        public NullVal() {
			type = "null";
		}
        public static string value = "null";
    }


    public class NumValue : RuntimeVal {
        public NumValue() {
			type = "number";
		}
        public long value {get; set;}
		
    }

    public class StringVal : RuntimeVal {
        public StringVal() {
			type = "string";
		}

        public string value {get; set;}
    }

}