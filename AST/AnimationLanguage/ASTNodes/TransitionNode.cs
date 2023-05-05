namespace AnimationLanguage.ASTNodes;
using ASTCommon;

// This class represents a transition node in the AST. A transition node defines a change in the animation properties.
public class TransitionNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Transition;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IList<IASTNode> Parameters { get; } = new List<IASTNode>(); // The parameters of the transition node.

    public TransitionNode(IEnumerable<IASTNode> parameters, SourceLocation sourceLocation)
    {
        SourceLocation = sourceLocation;

        foreach (IASTNode parameter in parameters)
        {
            Parameters.Add(parameter);
            Children.Add(parameter);
        }
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
    
    
    public override string ToString()
    {
        return $"TransitionNode: {string.Join(", ", Parameters)}";
    }
    
    
    public T Accept<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}