namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class IdentifierNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Identifier;

    public string Name { get; set; }

    public IdentifierNode(string name, SourceLocation sourceLocation)
    {
        Name = name;
        SourceLocation = sourceLocation;
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Enumerable.Empty<IASTNode>(); // Identifiers have no children, so it returns an empty list.
    }
    
    
    public override string ToString()
    {
        return $"IdentifierNode: {Name}";
    }
}
