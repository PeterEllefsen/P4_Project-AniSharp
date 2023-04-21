using AnimationLanguage;
using Antlr4.Runtime;

//Setup for reading the ExampleCode.ss

var fileName = "Content\\ExampleCode.ss";

var fileContent = File.ReadAllText(fileName);

var inputStream = new AntlrInputStream(fileContent);

var animationLanguageRulesLexer = new AnimationLanguageRulesLexer(inputStream);
var commonTokenStream = new CommonTokenStream(animationLanguageRulesLexer);
var animationLanguageParser = new AnimationLanguageRulesParser(commonTokenStream); 
var animationLanguageContext = animationLanguageParser.program();
var visitor = new AnimationLanguageVisitor();        
visitor.Visit(animationLanguageContext);