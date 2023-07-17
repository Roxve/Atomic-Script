using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Ttype;
namespace Atomic;
public class Parsing {

    List<(string value, TokenType type)> ions;
    public Parsing(List<(string value, TokenType type)> ions) {
        this.ions = ions;
    }
}