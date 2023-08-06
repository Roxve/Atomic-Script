# Atomic
Atomic is a simple small dynamic programming lang made for fun! built-on c#
#### warning yes i know my code is shit this isn't a serious job or something please don't hate 🥳

# Features(right now):
- enviroments and vars! declared with ```set varname >> value``` and to change values use ```varname >> value```😁
atomic also support immutable variables with ```set locked varname >> value``` ✅✅
- objects! Declared and assign the same way as vars but they contains properties ```set locked user >> {num: 1,pass: 2235}```;
- built-in functions
- user defined functions, example
```atom
write("Hello, world!")
func add(a,b) {
  a + b
}
write("8 + 8 is equal to",add(8,8))
```
 for now functions returns the last line value (16) i am working on return keyword ✅
# Features(i want):
- full basic working programming lang c# typescript golang f#(less to do more) inspired syntax
# TODO(next update):
- user defined functions return the value after return keyword not the last line
- better errors? (skip errors until finished running, repl mode = no errors, better error locationing?)
# trying atomic:
simply use the command in your terminal ```dotnet run``` to enter repl mode
(make sure you have dotnet-sdk installed and to exit press ```ctrl+c``` in your terminal)
or if you to read a file use ```dotnet run run {file}``` or pass this args using your debugger
and if you want to test it for dev reasons ```dotnet run run? {file}``` (test run is buggy right now)
### if you are using a debugger and getting an error give invalid args (in vscode you can go to .vscode/launch.json and change ```args=[]``` to args=["random"]) (this is a bug i am too lazy to fix)

# license:

i don't know about this atuff but you can use the code as long as you inform me and give me credits i guess
