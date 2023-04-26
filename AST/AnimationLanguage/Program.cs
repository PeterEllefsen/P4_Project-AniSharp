using AnimationLanguage.ASTCommon;
using Antlr4.Runtime;
namespace AnimationLanguage;


class Program
{
    static void Main(string[] args)
    {
        //The location of the input source code file is defined here.
        string inputSourceCode = File.ReadAllText("ExampleCode.ss");
        
        /* CharStream is an interface in the ANTLR4 library that represents a stream of characters.
        It is used to convert the inputcode of a language into a stream that can be read by the lexer.*/
        ICharStream charStream = CharStreams.fromString(inputSourceCode); //

        
        AnimationLanguageRulesLexer lexer = new AnimationLanguageRulesLexer(charStream); //We use the charStream as input for the lexer.
        CommonTokenStream tokens = new CommonTokenStream(lexer); //The lexer generates tokens, which are then stored in a token stream.
        AnimationLanguageRulesParser parser = new AnimationLanguageRulesParser(tokens); //The parser uses the token stream as input.
        
        ParserRuleContext parseTreeRoot = parser.s(); //The start of the parse tree is defined here. It is the root node.
        
        AnimationLanguageVisitor visitor = new AnimationLanguageVisitor(); //The visitor is initialized as an object
        
        IASTNode astRoot = (IASTNode)visitor.Visit(parseTreeRoot); //The visitor visits the parse tree and returns the root of the AST.
        
        PrintAST(astRoot);

        //The returned object (astRoot) is now the root of your AST
    }
    
    
    
    
    public static void PrintAST(IASTNode node, int indentation = 0)
    {
        if (node == null)
        {
            return;
        }

        string indent = new string(' ', indentation * 2);
        Console.WriteLine($"{indent}{node.GetType().Name}");

        foreach (IASTNode child in node.GetChildren())
        {
            PrintAST(child, indentation + 1);
        }
    }

}