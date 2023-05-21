using AnimationLanguage.ASTCommon;
using AnimationLanguage.ASTNodes;
using AnimationLanguage.Visitor;
using Antlr4.Runtime;

namespace AnimationLanguage
{
    class Program
    {
        static void Main(string[] args)
        {
            // The location of the input source code file is defined here.
            string inputSourceCode = File.ReadAllText("ExampleCode.ss");

            // CharStream is an interface in the ANTLR4 library that represents a stream of characters.
            // It is used to convert the input code of a language into a stream that can be read by the lexer.
            ICharStream charStream = CharStreams.fromString(inputSourceCode);

            AnimationLanguageRulesLexer lexer = new AnimationLanguageRulesLexer(charStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            AnimationLanguageRulesParser parser = new AnimationLanguageRulesParser(tokens);

            ParserRuleContext parseTreeRoot = parser.program();

            AnimationLanguageVisitor visitor = new AnimationLanguageVisitor();
            IASTNode? astRoot = visitor.Visit(parseTreeRoot);

            //Instantiate the ScopedSymbolTable.
            ScopedSymbolTable scopedSymbolTable = new ScopedSymbolTable();
            //Instantiate the TypeCheckingVisitor to perform type checking on the AST.
            TypeCheckingVisitor typeCheckingVisitor = new TypeCheckingVisitor(scopedSymbolTable);
            IASTNode? decoratedAstRoot = typeCheckingVisitor.Visit((ProgramNode)astRoot);


            CodeGenerationVisitor codeGenerationVisitor = new CodeGenerationVisitor();
            codeGenerationVisitor.Visit((ProgramNode)astRoot);
            //PrintAST(astRoot);
        }

        public static void PrintAST(IASTNode? node, int indentation = 0)
        {
            if (node == null)
            {
                return;
            }

            string indent = new string(' ', indentation * 2);
            Console.WriteLine($"{indent}{node.ToString()}");

            foreach (IASTNode child in node.GetChildren())
            {
                PrintAST(child, indentation + 1);
            }
        }
    }
}
