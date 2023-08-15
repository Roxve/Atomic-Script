---
permalink: /docs/basic_syntax/
layout: single
title: Basic Atomic-Script Syntax
toc: true
toc_label: Syntax
---

# basic stuff
- printing hello world
    ```
    write("hello world!")
    ```



# if-else expressions
in Atomic-Script if-else is an expression which returns last line!
- basic if-else:
    ```
    if [expr]: {
        doStuff()
    }
    else if [expr] {
        doOtherStuff()
    }
    else {
        doOtherOtherStuff()
    }
    ```
- using if else in expressions
    ```
    set name >> prompt("enter your name:")
    set isAdmin >> if name = "Thomas": { true  } else { false }
    ```
## if-else example code
- ...
