using AnimationLanguage.ASTCommon;
using AnimationLanguage.ASTNodes;
using AnimationLanguage.Visitor;
using Antlr4.Runtime;

namespace Tests;

public class AST
{
    public IASTNode ASTBuild(string input)
    {
        ICharStream charStream = CharStreams.fromString(input);
        AnimationLanguageRulesLexer lexer = new AnimationLanguageRulesLexer(charStream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        AnimationLanguageRulesParser parser = new AnimationLanguageRulesParser(tokens);
        ParserRuleContext parseTreeRoot = parser.expression();

        AnimationLanguageVisitor visitor = new AnimationLanguageVisitor();
        IASTNode? astRoot = visitor.Visit(parseTreeRoot);
        //[InlineData("1 + 2", "AdditionNode: 1 + 2")]
        return astRoot;
    }
}