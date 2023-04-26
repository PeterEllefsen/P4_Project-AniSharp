namespace AnimationLanguage.ASTNodes;
using ASTCommon;

// This class represents a tuple node in the AST
public class TupleNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Tuple;
    public IDictionary<string, IASTNode> Arguments { get; }

    public TupleNode(IDictionary<string, IASTNode> arguments, SourceLocation sourceLocation)
    {
        Arguments = arguments;
        SourceLocation = sourceLocation;
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Arguments.Values; //The arguments are the children of the Tuple node, so they are returned.
    }
}