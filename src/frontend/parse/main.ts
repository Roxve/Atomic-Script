import { Expr, Stmt } from "../AST/stmts.ts";
import { Ion, Type } from "../Ion.ts";
import { createError } from "../../etc.ts";

export class ParserMain {
  protected ions: Array<Ion>;
  protected line: number = 1;
  protected colmun: number = 1;

  public constructor(ions: Ion[]) {
    this.ions = ions;
  }
  protected ErrorEngine = class {
    private superThis;
    constructor(superThis: ParserMain) {
      this.superThis = superThis
    }


    unexcepted_ION_exception(not_excepted: Ion, excepted?: Type) {
      if(excepted) {
        createError(
          `excepted ION of type:${
            this.superThis.getTypeName(excepted)
          }, but got:${this.superThis.getTypeName(not_excepted.type)}\nat => line:${not_excepted.line},colmun:${not_excepted.colmun}\nError:AT1001`
        );
      }
      else {
        createError( 
          `unexcepted ion => value:${not_excepted.value}, type:${
            this.superThis.getTypeName(not_excepted.type)
          }\nat => line:${not_excepted.line}, colmun:${not_excepted.colmun}\nError:AT1001`
        );
      }
    }

    cannot_declare_exception(msg: string, didyoumean?: string) {
      createError(
        `cannot declare ${msg} ${didyoumean ? `,did you mean ${didyoumean}` : ""}\n,at => line:${this.superThis.line}, colmun:${this.superThis.colmun}\nError: AT1002`
      )
    }

    inside_function_creation_exception(msg: string) {
      createError(
        `inside function creation ${msg}\nat => line:${this.superThis.line}, colmun:${this.superThis.colmun}\nError: AT1003`
      )
    }

    excepted_ethier_exception(one: string, two: string, ina: string) {
      createError(
        `excepted ethier ${one} or ${two}, in ${ina}\nat => line:${this.superThis.line}, colmun:${this.superThis.colmun}\nError: AT1004`
      )
    }
  }
  error = new this.ErrorEngine(this);

  protected getTypeName(T: Type): string {
    let name: string = Type[T];

    if (name && name.includes("_kw")) {
      return name.replace("_kw", " keyword");
    } else if (name && name.includes("_type")) {
      return name.replace("_type", "");
    } else {
      return name;
    }
  }

  protected take(): Ion {
    if (this.ions.length <= 0) {
      return {
        value: "END",
        type: Type.EOF,
        line: this.line,
        colmun: this.colmun,
      } as Ion;
    } else {
      this.Update();
      const ion: Ion = this.ions.shift() || {} as Ion;

      return ion;
    }
  }
  protected except(correct_type: Type): Ion {
    if (this.at().type != correct_type) {
      this.error.unexcepted_ION_exception(this.at(), correct_type);
      return this.take();
    } else {
      return this.take();
    }
  }
  protected at(): Ion {
    if (this.ions.length <= 0) {
      return {
        value: "END",
        type: Type.EOF,
        line: this.line,
        colmun: this.colmun,
      } as Ion;
    } else {
      this.Update();
      return this.ions[0];
    }
  }
  protected Update() {
    this.line = this.ions[0].line;
    this.colmun = this.ions[0].colmun;
  }

  protected notEOF(): boolean {
    return this.at().type != Type.EOF;
  }

  // we just need a code that survives complie time here

  protected parse_stmt(): Stmt {
    return {} as Stmt;
  }

  protected parse_expr(): Expr {
    return {} as Expr;
  }
  protected parse_args(): Expr[] {
    return [];
  }
}
