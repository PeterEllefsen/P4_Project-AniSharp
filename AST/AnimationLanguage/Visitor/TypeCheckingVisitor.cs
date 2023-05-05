using System.Reflection;
using System.Threading.Channels;
using AnimationLanguage.ASTNodes;

namespace AnimationLanguage.Visitor;
using ASTCommon;


public class TypeCheckingVisitor : ASTVisitor<IASTNode>
{
    private readonly SymbolTable _symbolTable;

    public TypeCheckingVisitor(SymbolTable symbolTable)
    {
        _symbolTable = symbolTable;
    }

    public override IASTNode? Visit(ProgramNode node)
    {
        Console.WriteLine("Type checking program node");
        return VisitChildren(node);
    }

    public override IASTNode? Visit(AssignmentNode node)
    {
        Console.WriteLine("Type checking assignment node");
        return VisitChildren(node);
    }

    public override IASTNode? Visit(IdentifierNode node)
    {
        Console.WriteLine("Type checking identifier node");
        return VisitChildren(node);
    }

    // Add other overrides for all relevant node types
}

