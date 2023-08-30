//import { assertEquals } from "https://deno.land/std@0.199.0/assert/mod.ts";
import { RunTest } from "./main.ts";
import { setPath } from "./etc.ts";
import * as path from "https://deno.land/std@0.188.0/path/mod.ts";

Deno.test(function AtomsTest() {
  let test_path = "../TestProj/main.atoms";
  setPath(path.dirname(path.resolve(test_path)));

  RunTest(Deno.readTextFileSync("../TestProj/main.atoms"));
});
