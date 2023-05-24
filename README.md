# P4-Project - An animation focused language and compiler, with interpretation done in JavaScript Canvas


## How to run the compiler

1. First write your code in our language in a file, and place it in the 'Content' directory located at P4-Project/AST/AnimationLanguage/Content. Make sure to name your file "ExampleCode.ss". If there is already a file present called "ExampleCode.ss", rename it to something else or delete it.

2. Open a terminal inside of the "AnimationLanguage" directory and use the `dotnet run` command. If `dotnet run` does not work, try using the `dotnet run program.cs` command instead.

3. Open another terminal inside of the "codegen" directory located inside of the "P4-Project" folder. 

4. If there is only a file called "Program.cs" rename it and create a C# project using the `dotnet new Console` or `dotnet new console` command. 

5. After the project is created, delete the newly created and empty "Program.cs", file and rename the old to "Program.cs".

6. Now run the codegen-project with `dotnet run`.

7. If the last compilation was successful, that means you have a file in the same directory called "output.html". 

8. Open that file in your browser and press "play" to see your animation.


## Extra things

- If you want to look at a visual printed representation of the AST, uncomment the "PrintAST(astRoot)" method call inside of the Program.cs file in the actual compiler. The typechecked AST can also be printed, by replacing the "astRoot" variable with the "decoratedAstRoot" variable. Be aware that this will print the exact same output as using the astRoot variable.
