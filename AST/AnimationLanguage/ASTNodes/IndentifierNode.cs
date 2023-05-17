namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class IdentifierNode : ExpressionNode
{
    public string Name { get; set; }

    public IdentifierNode(string name, SourceLocation sourceLocation)
        : base(ExpressionNodeType.Identifier, null, null, null, VariableType.Null, sourceLocation)
    {
        Name = name;
    }

    public IEnumerable<IASTNode> GetChildren()
    {
        return Enumerable.Empty<IASTNode>(); // Identifiers have no children, so it returns an empty list.
    }

    public override string ToString()
    {
        return $"{Name}";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }

}