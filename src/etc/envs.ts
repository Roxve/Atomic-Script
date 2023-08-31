import {
  MK_NATIVE_FUNC,
  MK_NULL,
  MK_NUM,
  RuntimeVal,
} from "../runtime/values.ts";
import * as VT from "../runtime/values.ts";
import { mainPath } from "../etc.ts";
import { createEnv, Enviroment } from "../runtime/enviroment.ts";
import { eval_env } from "../runtime/eval/eval_enviroment.ts";
import { Parser } from "../frontend/parser.ts";
import { Ionizer } from "../frontend/ionizer.ts";

export function extra(): Enviroment {
  let atoms = Deno.readTextFileSync(mainPath + "/Protons/extra.proton");

  let env = eval_env(new Parser(new Ionizer(atoms).ionize()).productAST());

  //typescript functions

  let random = MK_NATIVE_FUNC(function (args, env) {
    for (let arg of args) {
      if (arg.type != "num") {
        env.error(
          "excepted args of num in function 'random' in proton extra",
          "EXTRA-ERROR-RANDOM0",
          null,
        );
        return MK_NULL();
      }
    }
    if (args.length === 0) {
      return MK_NUM(Math.random());
    } else if (args.length === 1) {
      return MK_NUM(
        Math.floor(Math.random() * ((args[0] as VT.NumVal).value - 1 + 1) + 1),
      );
    } else if (args.length === 2) {
      let min = args[0] as VT.NumVal;
      let max = args[1] as VT.NumVal;
      return MK_NUM(
        Math.floor(Math.random() * (max.value - min.value + 1) + min.value),
      );
    } else {
      env.error(
        "function 'random' in proton 'extra' can only take args of ethier\n'()' for Math.random, '(max)' for a num from 1 to max, '(min, max)' you get it",
        "EXTRA-ERROR-RANDOM1",
        null,
      );
    }
    return MK_NULL();
  });

  env.declareVar("random", random, true, null);
  return env;
}
