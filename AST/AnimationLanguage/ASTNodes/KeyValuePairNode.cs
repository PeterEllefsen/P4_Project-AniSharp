namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class KeyValuePairNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.KeyValuePair;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IdentifierNode Key { get; set; } //The key of the key-value pair.
    public IASTNode Value { get; set; } //The value of the key-value pair.

    public KeyValuePairNode(string key, IASTNode value, SourceLocation sourceLocation)
    {
        Key = new IdentifierNode(key, sourceLocation);
        Value = value;
        SourceLocation = sourceLocation;

        Children.Add(Key);
        Children.Add(value);
    }
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
    
    
    public override string ToString()
    {
        return $"KeyValuePairNode: ({Key}, {Value})";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}
