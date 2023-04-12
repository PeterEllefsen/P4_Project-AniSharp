using AnimationLanguage;
using Antlr4.Runtime;

//Setup for læsning af ExampleCode.ss

var fileName = "Content\\ExampleCode.ss";

var fileContent = File.ReadAllText(fileName);

var inputStream = new AntlrInputStream(fileContent);

var animationLanguageRulesLexer = new AnimationLanguageRulesLexer(inputStream);
var commonTokenStream = new CommonTokenStream(animationLanguageRulesLexer);
var animationLanguageParser = new AnimationLanguageRulesParser(commonTokenStream); //Der kan ved denne skrives .AddErrorListener(new DiagnosticErrorListener()) for at få fejl i consolen
var animationLanguageContext = animationLanguageParser.program();
var visitor = new AnimationVisitor();        
visitor.Visit(animationLanguageContext);

var i = 1;