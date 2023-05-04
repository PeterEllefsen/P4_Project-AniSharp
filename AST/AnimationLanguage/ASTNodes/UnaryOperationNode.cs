namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class UnaryOperationNode : IASTNode
{
    public IdentifierNode Identifier { get; }
    public UnaryOperator Operator { get; }
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType { get; }

    public UnaryOperationNode(IdentifierNode identifier, UnaryOperator op, SourceLocation sourceLocation)
    {
        Identifier = identifier;
        Operator = op;
        SourceLocation = sourceLocation;
    }

    public IEnumerable<IASTNode> GetChildren()
    {
        return new[] { Identifier };
    }
    

    public override string ToString()
    {
        return $"UnaryOperationNode: {Identifier} {Operator}";
    }
}

public enum UnaryOperator
{
    Increment, // i++
    Decrement  // i--
}