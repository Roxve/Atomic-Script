import { Ion, Type } from "./Ion.ts";
import { createError } from "../etc.ts";

export class Ionizer {
  private atoms;
  private ions: Ion[];
  line: number = 1;
  colmun: number = 1;

  constructor(atoms: string) {
    try {
      this.atoms = atoms.split("");
    } catch {
      this.atoms = [""];
    }
    this.ions = new Array<Ion>();
  }

  error(message: string, code: string) {
    createError(
      `${message}\nat => line:${this.line}, colmun:${this.colmun}\ngot => char:${
        this.atoms[0]
      }, error code:${code}`,
    );

    return {
      message,
      type: "error",
    };
  }

  private KEYWORDS: Record<string, Type> = {
    set: Type.set_kw,
    locked: Type.locked_kw,
    return: Type.return_kw,
    true: Type.bool_type,
    false: Type.bool_type,
    null: Type.null_type,
    if: Type.if_kw,
    else: Type.else_kw,
    use: Type.use_kw,
  };

  ion(value: any, type: Type): Ion {
    return {
      value,
      type,
      line: this.line,
      colmun: this.colmun,
    };
  }

  isAllowedId(x: string): boolean {
    return x.toUpperCase() != x.toLowerCase() || x != "." && this.isNum(x);
  }

  isNum(x: string): boolean {
    return "0123456789.٠١٢٣٤٥٦٧٨٩".includes(x);
  }

  isOOp(x: string): boolean {
    return "+-*/%=<&".includes(x);
  }
  getLine(x: string | undefined): boolean {
    if (x == "\n") {
      this.line++;
      this.colmun = 1;
      return true;
    } else {
      return false;
    }
  }
  isSkippableChar(x: string) {
    return "; ".includes(x) || x === "\t";
  }
  private add(value: any, type: Type) {
    if (value === undefined) {
      this.ions.push(this.ion(this.atoms.shift(), type));
    } else {
      this.ions.push(this.ion(value, type));
    }
    this.colmun++;
  }

  private take() {
    this.colmun++;
    return this.atoms.shift();
  }
  private at() {
    return this.atoms[0];
  }
  ionize(): Ion[] {
    while (this.atoms.length > 0) {
      switch (this.atoms[0]) {
        //ooperators
        case "+":
        case "-":
        case "*":
        case "/":
        case "%":
        case "&":
        case ">":
        case "<":
          this.add(this.take(), Type.ooperator);
          continue;
        case "=":
          this.take();
          if (this.atoms[0] == "=") {
            this.take();
            this.add("==", Type.ooperator);
          } else {
            this.add("=", Type.equals);
          }
          continue;
        case "|":
          this.take();
          if (this.atoms[0] == "|") {
            this.take();
            this.add("||", Type.ooperator);
          } else {
            this.add("|", Type.ooperator);
          }
          continue;
          //symbols
        case "(":
          this.add(undefined, Type.OpenParen);
          continue;
        case ")":
          this.add(undefined, Type.CloseParen);
          continue;
        case "{":
          this.add(undefined, Type.OpenBrace);
          continue;
        case "}":
          this.add(undefined, Type.CloseBrace);
          continue;
        case "[":
          this.add(undefined, Type.OpenBracket);
          continue;
        case "]":
          this.add(undefined, Type.CloseBracket);
          continue;
        case ",":
          this.add(undefined, Type.Comma);
          continue;
        case ".":
          this.add(undefined, Type.Dot);
          continue;
        case ":":
          this.add(undefined, Type.Colon);
          continue;
        //strings
        case '"':
        case "'":
          let char = this.take();
          let res = "";
          
          while (this.atoms[0] != char && this.atoms.length > 0) {
            if(this.at() === "\\") {
              this.take();
              switch(this.at()) {
                case "n": 
                  this.take();
                  res += "\n";
                  continue;
                case "t": 
                  this.take();
                  res += "\t";
                  continue;
                default: 
                  res += this.take();
                  continue;
              }
            }
            res += this.take();
          }
          if (this.atoms[0] != char) {
            this.error("reached end of file and didnt finish string", "AT0001");
          } else {
            this.add(res, Type.str_type);
            this.take();
          }
          continue;
        //comments
        case "#":
          this.take();
          while (!this.getLine(this.atoms[0]) && this.atoms.length > 0) {
            this.take();
          }
          continue;
        case "\n":
          this.getLine(this.take());
          continue;
        default:
          if (this.isSkippableChar(this.atoms[0])) {
            this.take();
          } //numbers
          else if (this.isNum(this.atoms[0])) {
            let res: string = "";
            while (this.atoms.length > 0 && this.isNum(this.atoms[0])) {
              res += this.take();
            }
            this.add(res, Type.num_type);
          } //identify
          else if (this.isAllowedId(this.atoms[0])) {
            let res: string = "";
            while (this.atoms.length > 0 && this.isAllowedId(this.atoms[0])) {
              res += this.take();
            }
            const keyword = this.KEYWORDS[res];
            //
            if (keyword != undefined) {
              this.add(res, keyword);
            } else {
              this.add(res, Type.id);
            }
          } else {
            this.error("unknown char", "AT0002");
            this.take();
          }
          continue;
      }
    }

    this.add("END", Type.EOF);

    return this.ions;
  }
}
