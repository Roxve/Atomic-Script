import { ObjVal, RuntimeVal } from "./values.ts";
import { Stmt } from "../frontend/AST/stmts.ts";
import { Id } from "../frontend/AST/values.ts";
import { MemberExpr } from "../frontend/AST/exprs.ts";
import { MK_NATIVE_FUNC, MK_NULL } from "./values.ts";
import { createError } from "../etc.ts";
import { native } from "../etc/NativeFuncs.ts";

export function createEnv() {
  let env: Enviroment = new Enviroment(null);
  env.declareVar("write", MK_NATIVE_FUNC(native.write), true, null);

  env.declareVar("prompt", MK_NATIVE_FUNC(native.promptFunc), true, null);
  return env;
}
export class Enviroment {
  locked_vars: Set<string>;
  vars: Map<string, RuntimeVal>;
  parent: Enviroment | null;

  public error(msg: string, code: string, stmt: Stmt | null): void {
    createError(
      `Runtime: ${msg}\nat => line:${stmt?.line}, colmun:${stmt?.colmun},error code:${code}`,
    );
  }
  public constructor(parent: Enviroment | null) {
    this.vars = new Map();
    this.locked_vars = new Set();
    this.parent = parent;
  }
  public resolve(name: string, stmt: Stmt): Enviroment | null {
    if (this.vars.has(name)) {
      return this;
    }

    if (!this.parent && this.parent === null || this.parent === undefined) {
      this.error(`cannot resolve ${name}`, "AT2003", stmt);
      return null;
    }
    return this.parent?.resolve(name, stmt);
  }

  public addEnv(env: Enviroment) {
    for (let vari of env.vars) {
      if (this.vars.has(String(vari[0]))) {
        continue;
      }
      if (this.parent?.vars.has(String(vari[0]))) {
        continue;
      }

      this.vars.set(vari[0], vari[1]);
    }
  }

  public declareVar(
    name: string,
    value: RuntimeVal,
    isLocked: boolean,
    stmt: Stmt | null,
  ): RuntimeVal {
    if (this.vars.has(name)) {
      this.error(`var:${name} is already declared`, "AT2001", stmt);
      return MK_NULL();
    }

    this.vars.set(name, value);
    if (isLocked) {
      this.locked_vars.add(name);
    }
    return value;
  }

  public setObjProperty(
    obj: ObjVal,
    property: string,
    value: RuntimeVal,
    stmt: Stmt,
  ): RuntimeVal {
    if (!obj.value.has(property)) {
      this.error(
        `object ${
          ((stmt as MemberExpr).obj as Id).symbol
        } doesnt contain property ${property}`,
        "At2004",
        stmt,
      );
      return MK_NULL();
    }
    obj.value.set(property, value);

    return value;
  }
  public getObjProperty(
    obj: ObjVal,
    property: string,
    stmt: Stmt,
  ): RuntimeVal {
    if (!obj.value.has(property)) {
      this.error(
        `object ${
          ((stmt as MemberExpr).obj as Id).symbol
        } doesnt contain property ${property}`,
        "AT2004",
        stmt,
      );
    }

    return obj.value.get(property) || MK_NULL();
  }
  public setVar(name: string, value: RuntimeVal, stmt: Stmt): RuntimeVal {
    const env = this.resolve(name, stmt);
    if (env === null) {
      return MK_NULL();
    }
    if (env.locked_vars.has(name)) {
      this.error("cannot assinge value to a locked var", "AT2002", stmt);
      return MK_NULL();
    }
    env.vars.set(name, value);
    return value;
  }

  public findVar(name: string, stmt: Stmt): RuntimeVal {
    const env = this.resolve(name, stmt);
    if (env === null) {
      return MK_NULL();
    }
    return env.vars.get(name) || MK_NULL();
  }
}
