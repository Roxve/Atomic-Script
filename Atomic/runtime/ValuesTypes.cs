﻿using Atomic_AST;
using System.Collections.Generic;

namespace ValueTypes;

public class VT {
	
	
	//just to remmber type names and we might need that later
    public static string ValueType = "null,num,string,bool,obj";


    public class RuntimeVal {
        public string type = ValueType;
    }
    public class NullVal : RuntimeVal {
        public NullVal() {
			type = "null";
		}
        public static string value = "null";
    }


    public class NumValue : RuntimeVal {
        public NumValue() {
			type = "num";
		}
        public int value {get; set;}
		
    }

    public class StringVal : RuntimeVal {
        public StringVal() {
			type = "string";
		}

        public string value {get; set;}
    }
	public class BooleanVal : RuntimeVal
	{
		public BooleanVal() {
			type = "bool";
		}
		public bool value {get; set;}
	}
	
	public class ObjectVal : RuntimeVal {
		public ObjectVal() {
			type = "obj";
		}
		public Dictionary<string,RuntimeVal> properties {get; set;}
	}
	
	
	
	
	public static BooleanVal MK_BOOL(bool value = false) {
	    var Inew = new BooleanVal(); Inew.value = value;
		return Inew;
	}
    
	
	public static StringVal MK_STR(string value = "unknown") {
		var Inew = new StringVal(); Inew.value = value;
		return Inew;
	}
	
	public static NumValue MK_NUM(int num = 0) {
		var Inew = new NumValue(); Inew.value = num;
		
		return Inew;
	}
	
	public static NullVal MK_NULL() {
		return new NullVal();
	}
}