import { ParserStmt } from "./stmt.ts";
import {
  Bool,
  Id,
  List,
  listedORExpr,
  Null,
  Num,
  Object,
  Property,
  Str,
} from "../AST/values.ts";
import { Expr, Stmt } from "../AST/stmts.ts";
import {
  AssignExpr,
  BinaryExpr,
  CallExpr,
  ElseExpr,
  IfExpr,
  MemberExpr,
} from "../AST/exprs.ts";
import { Ion, Type } from "../Ion.ts";

export class ParserExpr extends ParserStmt {
  protected parse_assign_expr(): Expr {
    const left = this.parse_obj_expr();

    if (this.at().type === Type.equals) {
      this.take();

      const value = this.parse_expr();

      return {
        type: "AssignExpr",
        assigne: left,
        value: value,
        line: this.line,
        colmun: this.colmun,
      } as AssignExpr;
    }

    return left;
  }

  protected parse_obj_expr(): Expr {
    if (this.at().type != Type.OpenBrace) {
      return this.parse_list();
    }

    this.take();

    let properties: Property[] = [];

    while (this.notEOF() && this.at().type != Type.CloseBrace) {
      let key = this.except(Type.id).value;

      let property: Property;

      if (this.at().type === Type.Comma) {
        this.take();
        property = {
          type: "Property",
          key: key,
          value: null,
          line: this.line,
          colmun: this.colmun,
        };
        properties.push(property);
        continue;
      } else if (this.at().type === Type.CloseBrace) {
        property = {
          type: "Property",
          key: key,
          value: null,
          line: this.line,
          colmun: this.colmun,
        };
        properties.push(property);
        continue;
      }
      this.except(Type.Colon);

      let value = this.parse_expr();

      property = {
        type: "Property",
        key: key,
        value: value,
        line: this.line,
        colmun: this.colmun,
      };

      properties.push(property);

      if (this.at().type != Type.CloseBrace) {
        this.except(Type.Comma);
      }
    }
    this.except(Type.CloseBrace);

    let obj: Object = {
      type: "Obj",
      properties: properties,
      line: this.line,
      colmun: this.colmun,
    };
    return obj;
  }
  protected parse_list(): Expr {
    if (this.at().type != Type.OpenBracket) {
      return this.parse_if_expr();
    }
    this.take();
    let values: Expr[] = [];
    while (this.notEOF() && this.at().type != Type.CloseBracket) {
      values.push(this.parse_expr());
      if (this.at().type === Type.Comma) {
        this.take();
        continue;
      }
    }
    this.except(Type.CloseBracket);

    return {
      type: "List",
      values,
    } as List;
  }
  protected parse_if_expr(): Expr {
    if (this.at().type === Type.if_kw) {
      this.take();

      let test = this.parse_compare_expr();
      this.except(Type.Colon);
      this.except(Type.OpenBrace);

      let body: Stmt[] = [];

      while (this.notEOF() && this.at().type != Type.CloseBrace) {
        body.push(this.parse_stmt());
      }
      this.except(Type.CloseBrace);

      let alt: Expr | undefined;
      while (this.notEOF() && this.at().type === Type.else_kw) {
        this.take();
        if (this.at().type === Type.if_kw) {
          alt = this.parse_if_expr();
        } else {
          this.except(Type.OpenBrace);
          let body: Stmt[] = [];

          while (this.notEOF() && this.at().type != Type.CloseBrace) {
            body.push(this.parse_stmt());
          }

          this.except(Type.CloseBrace);
          alt = {
            type: "ElseExpr",
            body,
            line: this.line,
            colmun: this.colmun,
          } as ElseExpr;
        }
      }
      return {
        type: "IfExpr",
        test,
        body,
        alt,
        line: this.line,
        colmun: this.colmun,
      } as IfExpr;
    } else {
      return this.parse_compare_expr();
    }
  }

  protected parse_compare_expr(): Expr {
    const main = this;

    function parse_compare_type2_expr(): Expr {
      let left = parse_compare_type1_expr();

      while (main.at().value === "||" || main.at().value === "&") {
        const ooperator = main.take().value;
        const right = parse_compare_type1_expr();

        left = {
          type: "BinaryExpr",
          left,
          ooperator,
          right,
          line: main.line,
          colmun: main.colmun,
        } as BinaryExpr;
      }
      return left;
    }

    function parse_compare_type1_expr(): Expr {
      let left = main.parse_listed_or_expr();

      while (
        main.at().value === "==" || main.at().value === "<" ||
        main.at().value === ">"
      ) {
        let ooperator = main.take().value;
        const right = main.parse_listed_or_expr();

        left = {
          type: "BinaryExpr",
          left,
          ooperator,
          right,
          line: main.line,
          colmun: main.colmun,
        } as BinaryExpr;
      }
      return left;
    }

    return parse_compare_type2_expr();
  }

  protected parse_listed_or_expr(): Expr {
    let left = this.parse_mathmatic_expr();

    let exprs: Expr[] = [];
    exprs.push(left);

    while (this.at().value === "|") {
      this.take();
      exprs.push(this.parse_mathmatic_expr());

      left = {
        type: "ListedOR",
        exprs,
        line: this.line,
        colmun: this.colmun,
      } as listedORExpr;
    }

    return left;
  }
  protected parse_mathmatic_expr(): Expr {
    const main = this;
    function parse_additive_expr(): Expr {
      let left = parse_multiplactive_expr();

      while (main.at().value === "+" || main.at().value === "-") {
        let val: string = main.take().value;
        const right = parse_multiplactive_expr();
        left = {
          type: "BinaryExpr",
          left: left,
          ooperator: val,
          right: right,
          line: main.line,
          colmun: main.colmun,
        } as BinaryExpr;
      }
      return left;
    }

    function parse_multiplactive_expr(): Expr {
      let left = main.parse_call_member_expr();

      while (
        main.at().value === "*" || main.at().value === "/" ||
        main.at().value === "%"
      ) {
        let val: string = main.take().value;
        const right = main.parse_call_member_expr();
        left = {
          type: "BinaryExpr",
          left: left,
          ooperator: val,
          right: right,
          line: main.line,
          colmun: main.colmun,
        } as BinaryExpr;
      }
      return left;
    }

    return parse_additive_expr();
  }

  protected parse_call_member_expr(): Expr {
    let member = this.parse_member_expr();

    if (this.at().type === Type.OpenParen) {
      return this.parse_call_expr(member);
    }

    return member;
  }

  protected parse_call_expr(caller?: Expr): Expr {
    if (caller) {
      let callExpr: Expr = {
        type: "CallExpr",
        caller,
        args: this.parse_args(),
        line: this.line,
        colmun: this.colmun,
      } as CallExpr;
      //dont know why i did this? but it doesnt work without it?
      if (this.at().type === Type.OpenParen) {
        callExpr = this.parse_call_expr(callExpr);
      }
      return callExpr;
    } else {
      let left = this.parse_primary_expr();
      if (this.at().type === Type.OpenParen) {
        return this.parse_call_expr(left);
      }
      return left;
    }
  }

  protected parse_args(): Expr[] {
    this.except(Type.OpenParen);

    let args: Expr[] = [];

    if (this.at().type != Type.CloseParen) {
      args = this.parse_args_list();
    }
    this.except(Type.CloseParen);

    return args;
  }
  protected parse_args_list(): Expr[] {
    let args: Expr[] = [];
    args.push(this.parse_assign_expr());

    while (this.at().type === Type.Comma) {
      this.take();
      args.push(this.parse_assign_expr());
    }

    return args;
  }

  protected parse_member_expr(): Expr {
    let left = this.parse_call_expr();

    while (
      this.notEOF() &&
      (this.at().type === Type.Dot || this.at().type === Type.OpenBracket)
    ) {
      let ooperator = this.take();
      let property: Expr;
      let isIndexed: boolean; //indexed => obj[index], !indexed obj.property

      if (ooperator.type === Type.Dot) {
        isIndexed = false;

        property = this.parse_primary_expr();
        if (property.type != "Id") {
          this.error.unexcepted_ION_exception(this.at(), Type.id);
          return { type: "Null", value: null } as Null;
        }
      } //excepts '['
      else {
        isIndexed = true;
        property = this.parse_expr();

        this.except(Type.CloseBracket);
      }
      left = {
        type: "MemberExpr",
        obj: left,
        property,
        isIndexed,
        line: this.line,
        colmun: this.colmun,
      } as MemberExpr;
    }
    return left;
  }

  protected parse_primary_expr(): Expr {
    switch (this.at().type) {
      case Type.OpenParen:
        this.take();
        let expr = this.parse_expr();
        this.except(Type.CloseParen);
        return expr;
      case Type.id:
        return {
          type: "Id",
          symbol: this.take().value,
          line: this.line,
          colmun: this.colmun,
        } as Id;
      case Type.str_type:
        return {
          type: "Str",
          value: this.take().value,
          line: this.line,
          colmun: this.colmun,
        } as Str;
      case Type.num_type:
        return {
          type: "Num",
          value: +this.take().value,
          line: this.line,
          colmun: this.colmun,
        } as Num;
      case Type.bool_type:
        return {
          type: "Bool",
          value: this.take().value == "true" ? true : false,
          line: this.line,
          colmun: this.colmun,
        } as Bool;
      case Type.null_type:
        this.take();
        return {
          type: "Null",
          value: null,
          line: this.line,
          colmun: this.colmun,
        } as Null;
      default:
        this.error.unexcepted_ION_exception(this.at());
        this.take();
        return {
          type: "Null",
          value: null,
          line: this.line,
          colmun: this.colmun,
        } as Null;
    }
  }
}
