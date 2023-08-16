---
# You don't need to edit this file, it's empty on purpose.
# Edit theme's home layout instead if you wanna make some changes
# See: https://jekyllrb.com/docs/themes/#overriding-theme-defaults
layout: splash
permalink: /

header:
  overlay_color: "#a34046"
  actions:
    - label: "<i class='fas fa-download'></i>github"
      url: "https://github.com/Atonix0/Atomic-Script"
excerpt: "A typescript,c#,python syntax inspired Atomic scripting language!."
---

# [<img src="./res/logo.png" height="10%" width = "10%">](./res/logo.png) Atomic Script
# Features:

### this is just an overview go to [basic syntax](/Atomic-Script/docs/basic_syntax) for more info about the syntax


### basic info:
- user definded functions & varibales with enviroments, objects[wip],easily add built-in functions!
- if-else exprs!


### Atomic-Script Only:
- (coming soon) | ooperator! have multiplier values you wanna check if a var is equal to? use the | ooperator
    ```
    set userid >> prompt("enter your id:")
    if userid = 7788 | 7782 | 2783: {
        # do stuff
    }

    ```

- semicolon? no semicolon? choose what you want cuz semicolon is a skippable char!

    ```
    # this works!
    write("Hello world!");
    write("nah huh") # this also works!

    ;;;;;;;;;;;;;;;;;;;;;;;
    ;;write("cutomozied");;
    ;;;;;;;;;;;;;;;;;;;;;;;
    # this also works!
    ```
- human understable! wtf is fn fun func var print put etc... we got write and set!
- *less keywords:*
- one keyword for setting everything!

    ```
    set name >> prompt("enter your name:");
    set writeName(message) {
        write(message " " + name);
        return "succes!";
    }
    writeName("Hello!")
    ```
- more...
