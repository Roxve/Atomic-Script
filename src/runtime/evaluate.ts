import {
  Expr,
  FuncCreation,
  Program,
  ReturnStmt,
  Stmt,
  UseStmt,
  VarCreation,
} from "../frontend/AST/stmts.ts";
import {
  AssignExpr,
  BinaryExpr,
  CallExpr,
  IfExpr,
  MemberExpr,
} from "../frontend/AST/exprs.ts";
import {
  Bool,
  Id,
  List,
  Null,
  Num,
  Object,
  Str,
} from "../frontend/AST/values.ts";
import { Enviroment } from "./enviroment.ts";
import * as VT from "./values.ts";
import * as expr from "./eval/expr.ts";
import * as stmt from "./eval/stmt.ts";
import { createError, isTest } from "../etc.ts";

export function error(msg: string, code: string, stmt: Stmt): void {
  createError(
    `Runtime Error:${msg}\nat => line:${stmt.line},colmun:${stmt.colmun},error code:${code}`,
  );
}

export function eval_program(prog: Program, env: Enviroment): VT.RuntimeVal {
  let results: VT.RuntimeVal = VT.MK_NULL();
  prog.body.forEach(function (stmt: Stmt) {
    results = evaluate(stmt, env);
    if (isTest) {
      console.log("last eval:");
      console.log(results);
    }
  });
  return results;
}
export function evaluate(node: Stmt, env: Enviroment): VT.RuntimeVal {
  switch (node.type) {
    case "Id":
      return env.findVar((node as Id).symbol, node as Id);
    case "Program":
      return eval_program(node as Program, env);
    case "Null":
      return VT.MK_NULL();
    case "Bool":
      return VT.MK_BOOL((node as Bool).value);
    case "Str":
      return VT.MK_STR((node as Str).value);
    case "Num":
      return VT.MK_NUM((node as Num).value);
    case "Obj":
      return expr.eval_object(node as Object, env);
    case "List":
      return expr.eval_list(node as List, env);
    case "VarCreation":
      return stmt.eval_var_creation(node as VarCreation, env);
    case "FuncCreation":
      return stmt.eval_func_creation(node as FuncCreation, env);
    case "ReturnStmt":
      return stmt.eval_return_stmt(node as ReturnStmt, env);
    case "UseStmt":
      return stmt.eval_use_stmt(node as UseStmt, env);
    case "BinaryExpr":
      return expr.eval_binary_expr(node as BinaryExpr, env);
    case "AssignExpr":
      return expr.eval_assign_expr(node as AssignExpr, env);
    case "MemberExpr":
      return expr.eval_member_expr(node as MemberExpr, env);
    case "CallExpr":
      return expr.eval_call_expr(node as CallExpr, env);
    case "IfExpr":
      return expr.eval_if_expr(node as IfExpr, env);
    case "ListedOR":
      error(
        "listed or can only be used with '>' & '<' & '==' ooperators only",
        "AT3013",
        node,
      );
      return VT.MK_NULL();
    default:
      error(
        "unknown error please report this ",
        `AT_UNKNOWN_30:${node.type}`,
        node,
      );
      console.log(node);
      return VT.MK_NULL();
  }
}
