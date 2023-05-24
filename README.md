# P4-Project - An animation focused language and compiler, with interpretation done in JavaScript Canvas


## How to run the compiler

1. Firstly create some code and place it in the Content directory located inside P4-Project/AST/AnimationLanguage/Content. If there is already a file present called "ExampleCode.ss", rename it or delete it and rename your code file to be "ExampleCode.ss".

2. Open a terminal inside the "AnimationLanguage" directory and use the `dotnet run` command.

3. Open another terminal inside the "codegen" directory located inside the "P4-Project" folder. 

4. If there is only a file called "Program.cs" rename it and create a c# project using the `dotnet new Console` or `dotnet new console` command. 

5. After the project is created, delete the newly created and empty "Program.cs" file and rename the old to "Program.cs".

6. Now run the codegen-project with `dotnet run`.

7. If the last compilation was successful, that means you have a file in the same directory called "output.html". 

8. Open that file in your browser and press "play" to see your animation.
