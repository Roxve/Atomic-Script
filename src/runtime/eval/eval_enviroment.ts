import { createEnv, Enviroment } from "../enviroment.ts";
import {
  FuncCreation,
  Program,
  Stmt,
  UseStmt,
  VarCreation,
} from "../../frontend/AST/stmts.ts";
import {
  eval_func_creation,
  eval_use_stmt,
  eval_var_creation,
} from "./stmt.ts";

export function eval_env(prog: Program): Enviroment {
  let env = createEnv();
  for (let stmt of prog.body) {
    evaluate_env(stmt, env);
  }
  return env;
}

export function evaluate_env(node: Stmt, env: Enviroment) {
  switch (node.type) {
    case "VarCreation":
      return eval_var_creation(node as VarCreation, env);
    case "FuncCreation":
      return eval_func_creation(node as FuncCreation, env);
    case "UseStmt":
      return eval_use_stmt(node as UseStmt, env);
    default:
      return;
  }
}
