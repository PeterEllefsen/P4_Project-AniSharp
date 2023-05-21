namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents an argument node in the AST
public class ArgumentNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Argument;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public string Name { get; set; } //The name of the argument.
    public IASTNode Value { get; set; } //The value of the argument. This is an IASTNode because it can be any type of node.

    public ArgumentNode(string name, IASTNode value, SourceLocation sourceLocation) 
    {
        Name = name;
        Value = value;
        SourceLocation = sourceLocation;

        Children.Add(value);
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
    
    
    public override string ToString()
    {
        return $"ArgumentNode: {Name}";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}