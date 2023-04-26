namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class KeyValuePairNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.KeyValuePair;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IdentifierNode Key { get; set; } //The key of the key-value pair.
    public IASTNode Value { get; set; } //The value of the key-value pair.

    public KeyValuePairNode(IdentifierNode key, IASTNode value, SourceLocation sourceLocation)
    {
        Key = key;
        Value = value;
        SourceLocation = sourceLocation;

        Children.Add(key);
        Children.Add(value);
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
}
