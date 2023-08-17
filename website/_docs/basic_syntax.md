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
## declarations and asinging:
- everything using set keyword
    ```
    # set varname >> value
    set x >> 15
    # set func_name([parameters]) {body}
    set writeSum(y) {
        return x + y;
    }
    write(writeSum(5));
    # outputs 20
    ```
- assinge
    ```
    # varname >> value
    x >> 6
    

    # lambada (i think)
    set awesome() {
        write("awesome!")
    }
    
    set new_awesome >> awesome
    # outputs "awesome"
    new_awesome()
    ```
- immutable var declaration (funtions are locked by default)
    ```
    set locked nuh >> 9
    nuh >> 10
    # returns an error
    ```
## ooperators (not operators)
```
multiply: *, divide: /,addition: +, substarct: -, module: %

and: &, or-or: ||
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
- example code for if else:
    ```
    set name >> prompt("enter your name:")
    if name = "Ahmed": {
        write("You are an admin's frined " + name + "!")
    }
    else if name = "Thomas" || name = "Aton":     {
        write("You are an admin " + name)
    }
    else {
        write("nuh huh?")
    }

    set is_robot >> prompt("are you a robot? Y/n")
    is_robot >> toLower(is_robot)
    set is_robot_answer >> if is_robot = "y":{ true } else { false }
    write("isRobot: ",is_robot_answer,"got =>", is_robot)
    ```
