import { Enviroment } from "./enviroment.ts";
import { Stmt } from "../frontend/AST/stmts.ts";

export type ValueType =
  | "null"
  | "str"
  | "num"
  | "bool"
  | "obj"
  | "list"
  | "functionCall"
  | "return"
  | "native-func"
  | "func";

export type ColorType =
  | "red"
  | "green"
  | "white"
  | "yellow";
export interface RuntimeVal {
  type: ValueType;
  value: any;
  color: ColorType;
}

export interface NumVal extends RuntimeVal {
  type: "num";
  value: number;
}

export interface StrVal extends RuntimeVal {
  type: "str";
  value: string;
}

export interface ObjVal extends RuntimeVal {
  type: "obj";
  value: Map<string, RuntimeVal>;
}
export interface ListVal extends RuntimeVal {
  type: "list";
  value: RuntimeVal[]
}
export interface BoolVal extends RuntimeVal {
  type: "bool";
  value: boolean;
}

export interface NullVal extends RuntimeVal {
  type: "null";
  value: null;
}

export type FunctionCall = (
  args: RuntimeVal[],
  env: Enviroment,
) => RuntimeVal;

export interface ReturnVal extends RuntimeVal {
  type: "return";
  value: RuntimeVal;
}

export interface NativeFnVal extends RuntimeVal {
  type: "native-func";
  call: FunctionCall;
  value: NullVal | string;
}

export interface FnVal extends RuntimeVal {
  type: "func";
  name: string;
  parameters: string[];
  body: Stmt[];
  env: Enviroment;
  value: NullVal | string;
}

export function MK_NULL(): NullVal {
  return { type: "null", value: null, color: "red" } as NullVal;
}
export function MK_NUM(num: number = 0): NumVal {
  return { type: "num", value: num, color: "yellow" } as NumVal;
}
export function MK_STR(str: string = ""): StrVal {
  return { type: "str", value: str, color: "green" } as StrVal;
}
export function MK_BOOL(bool: boolean = false): BoolVal {
  return {
    type: "bool",
    value: bool,
    color: bool ? "green" : "red",
  } as BoolVal;
}

export function MK_NATIVE_FUNC(call: FunctionCall): NativeFnVal {
  return {
    type: "native-func",
    call,
    value: MK_NULL(),
  } as NativeFnVal;
}

export function MK_TYPE(t: any): RuntimeVal {
  if (t === undefined || t === null) {
    return MK_NULL();
  } else if (typeof t === "number" || parseInt(t)) {
    return MK_NUM(t);
  } else if (typeof t === "string") {
    return MK_STR(t);
  } else if (typeof t === "boolean") {
    return MK_BOOL(t);
  } else {
    return MK_NULL();
  }
}
