export type Node =
  | "Program"
  | "VarCreation"
  | "FuncCreation"
  | "ReturnStmt"
  | "UseStmt"
  | "Num"
  | "Str"
  | "Bool"
  | "Null"
  | "Id"
  | "Property"
  | "Obj"
  | "List"
  | "ListedOR"
  | "BinaryExpr"
  | "AssignExpr"
  | "CallExpr"
  | "MemberExpr"
  | "DashExpr"
  | "IfExpr"
  | "ElseExpr";

export interface Stmt {
  type: Node;
  line: number;
  colmun: number;
}

export interface Program extends Stmt {
  type: "Program";
  body: Stmt[];
}
export interface VarCreation extends Stmt {
  type: "VarCreation";
  name: string;
  isLocked: boolean;
  value: Expr;
}

export interface FuncCreation extends Stmt {
  type: "FuncCreation";
  name: string;
  body: Stmt[];
  parameters: string[];
}

export interface ReturnStmt extends Stmt {
  type: "ReturnStmt";
  value: Expr;
}

export interface UseStmt extends Stmt {
  type: "UseStmt";
  path: string;
  isProton: boolean;
}
// because exprs are stmts i guess
export interface Expr extends Stmt {}
