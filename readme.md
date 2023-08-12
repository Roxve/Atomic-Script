# Atomic
Atomic is a simple small dynamic programming lang made for fun! built-on c#
#### warning yes i know my code is shit this isn't a serious job or something please don't hate 🥳

# Features(basic syntax):
### TODO move the basic syntax to a website
- customize your code with semicolons! Because i don't like the requirements of adding ; at the end of lines but i like how they look so i added ';' as a skippable character if you want to end a line with semicolon do it if you don't don't!
- enviroments and vars! declared with ```set varname >> value``` and to change values use ```varname >> value```😁
atomic also support immutable variables with ```set locked varname >> value``` ✅✅
- objects! Declared and assign the same way as vars but they contains properties ```set locked user >> {num: 1,pass: 2235}```;
- built-in functions
- comments currently there is only one line comments using '#'
- user defined functions, example
```cs
write("Hello, world!")
func add(a,b) {
  return a + b;
}
write("8 + 8 is equal to",add(8,8))
```
🦜
- if-else expressions
```cs
set name >> prompt("enter your name: ")
if name = "Ahmed":  {
     write("you are an admins friend")
}
else if name = "Thomas": {
      write("you are an admin :)")
}
else {
 write("you are not an admin ):")
}
set isRobotQ >> toLower(prompt("are you a robot? Y/n"))

# true if user gave y as an answer false if anything else
set isRobotA >> if isRobotQ = y: { true } else { false }
```
✅
# Features(i want):
- full basic working programming lang c# typescript golang f#(less to do more) inspired syntax
# TODO(next update):
- make a website for the language as it's going tough it's early usable stage faster than i tough
- else if is an expr not a statement (returns a value)
- better errors? (skip errors until finished running, repl mode = no errors, better error locationing?)(already done that but it needs more work)
# trying atomic:
simply use the command in your terminal ```dotnet run``` to enter repl mode
(make sure you have dotnet-sdk installed and to exit press ```ctrl+c``` in your terminal)
or if you to read a file use ```dotnet run run {file}``` or pass this args using your debugger
and if you want to test it for dev reasons ```dotnet run run? {file}``` 
### if you are using a debugger and getting an error give invalid args (in vscode you can go to .vscode/launch.json and change ```args=[]``` to args=["random"]) (this is a bug i am too lazy to fix)

# license:

i don't know about this atuff but you can use the code as long as you inform me and give me credits i guess
