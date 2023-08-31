import { ParserMain } from "./main.ts";
import {
  Expr,
  FuncCreation,
  ReturnStmt,
  Stmt,
  UseStmt,
  VarCreation,
} from "../AST/stmts.ts";
import { Id, Null } from "../AST/values.ts";
import { Type } from "../Ion.ts";
import { currentPath, mainPath } from "../../etc.ts";

export class ParserStmt extends ParserMain {
  parse_creation(): Stmt {
    this.except(Type.set_kw);

    let isLocked: boolean = false;
    if (this.at().type == Type.locked_kw) {
      this.take();

      isLocked = true;
    }
    let name = this.except(Type.id).value;
    switch (this.at().type) {
      case Type.OpenParen:
        if (isLocked) {
          this.error.cannot_declare_exception("a locked function", "to declare a private function?");
          return { type: "Null", value: null } as Null;
        }
        return this.parse_func_creation(name);
        break;
      default:
        return this.parse_var_creation(name, isLocked);
    }
  }

  parse_var_creation(name: string, isLocked: boolean): Stmt {
    let value: Expr;
    if (this.at().type != Type.Colon) {
      if (isLocked) {
        this.error.cannot_declare_exception("a locked var without a value!");
        return {
          type: "Null",
          value: null,
          line: this.line,
          colmun: this.colmun,
        } as Null;
      }
      value = {
        type: "Null",
        value: null,
        line: this.line,
        colmun: this.colmun,
      } as Null;
    } else {
      this.take();
      value = this.parse_expr();
    }

    return {
      type: "VarCreation",
      name,
      value,
      isLocked,
      line: this.line,
      colmun: this.colmun,
    } as VarCreation;
  }

  parse_func_creation(name: string): Stmt {
    let args: Expr[] = this.parse_args();
    let parameters: string[] = [];

    for (let arg of args) {
      if (arg.type != "Id") {
        this.error.inside_function_creation_exception("parameters must be of type ids!");
        return { type: "Null", value: null } as Null;
      }
      parameters.push((arg as Id).symbol);
    }
    this.except(Type.Colon);
    this.except(Type.OpenBrace);

    let body: Stmt[] = [];

    while (this.notEOF() && this.at().type != Type.CloseBrace) {
      body.push(this.parse_stmt());
    }

    this.except(Type.CloseBrace);
    return {
      type: "FuncCreation",
      name,
      parameters,
      body,
      line: this.line,
      colmun: this.colmun,
    } as FuncCreation;
  }

  parse_return_stmt(): Stmt {
    this.take();

    let value = this.parse_expr();

    return {
      type: "ReturnStmt",
      value,
      line: this.line,
      colmun: this.colmun,
    } as ReturnStmt;
  }
  parse_use_stmt(): Stmt {
    this.take();
    let pathl = "";
    let isProton: boolean;
    if (this.at().type === Type.str_type) {
      pathl = currentPath + "/" + this.take().value;
      isProton = false;
    } else if (this.at().type === Type.id) {
      isProton = true;
      pathl = this.take().value.toLowerCase();
    } else {
      this.error.excepted_ethier_exception("id for moudle name", "string for file path", "use stmt");
      return { type: "Null", value: null } as Null;
    }
    return {
      type: "UseStmt",
      path: pathl,
      isProton,
    } as UseStmt;
  }
}
