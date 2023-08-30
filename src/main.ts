import { Ionizer } from "./frontend/ionizer.ts";
import { Parser } from "./frontend/parser.ts";
import { createEnv, Enviroment } from "./runtime/enviroment.ts";
import { evaluate } from "./runtime/evaluate.ts";
import { isError, setTest } from "./etc.ts";
import { rgb24 } from "https://deno.land/std@0.200.0/fmt/colors.ts";
import { createError, setMainPath, setPath } from "./etc.ts";
import * as path from "https://deno.land/std@0.188.0/path/mod.ts";
import * as VT from "./runtime/values.ts";
import { RuntimeToStr } from "./etc.ts";

function main(args: string[]) {
  if (args === undefined || args === null || args.length <= 0) {
    Repl();
  }
  let atoms = "";
  setMainPath(path.dirname(path.fromFileUrl(import.meta.url)));
  switch (args[0]) {
    case "run":
      if (args.length < 2) {
        createError("file to run excepted");
      }

      try {
        atoms = Deno.readTextFileSync(args[1]).toString();
      } catch {
        createError(
          "file doesnt exit or no read premsision given \nFile => " + args[1],
        );
      }
      const file_dir = path.dirname(path.resolve(args[1]));
      setPath(file_dir);
      Deno.chdir(file_dir);

      Run(atoms);
      break;
    case "run?":
      if (args.length < 2) {
        createError("file to run excepted.");
      }
      atoms = Deno.readTextFileSync(args[1]).toString();
      const file_dirr = path.dirname(path.resolve(args[1]));
      setPath(file_dirr);
      Deno.chdir(file_dirr);
      console.log(atoms);
      RunTest(atoms);
      break;
  }
}

function Repl() {
  console.log();
  const env: Enviroment = createEnv();
  while (true) {
    console.log("%cAtomic", "color: #c22147");

    const atoms: any = prompt(rgb24("=>", {
      r: 194,
      g: 33,
      b: 71,
    }));
    if (atoms == ".exit") {
      Deno.exit(0);
    }
    const ionizer = new Ionizer(atoms);
    const ionized = ionizer.ionize();

    const parser: Parser = new Parser(ionized);
    const parsed = parser.productAST();

    const run = evaluate(parsed, env);
    let value: string = RuntimeToStr(run);
    
    console.log(`%c${value}`, `color: ${run.color}`);
  }
}
export function RunTest(atoms: string) {
  setTest();
  const env = createEnv();
  const ionizer = new Ionizer(atoms);
  const ionized = ionizer.ionize();
  console.log("%c*******IONIZED:*******", "font-size: larger; color: red");
  console.log(ionized);
  console.log("\n\n\n\n\n\n");

  const parser: Parser = new Parser(ionized);
  const parsed = parser.productAST();
  console.log("%c******PARSED:******", "font-size: larger; color: red");

  console.log(parsed);

  console.log("\n\n\n\n\n\n");

  if (isError) {
    Deno.exit(1);
  }

  const run = evaluate(parsed, env);
  console.log(run);
}
export function Run(atoms: string) {
  const env = createEnv();

  const ionizer = new Ionizer(atoms);
  const ionized = ionizer.ionize();

  const parser = new Parser(ionized);
  const parsed = parser.productAST();

  if (isError) {
    Deno.exit(1);
  }
  const run = evaluate(parsed, env);
}
// Learn more at https://deno.land/manual/examples/module_metadata#concepts
if (import.meta.main) {
  main(Deno.args);
}
