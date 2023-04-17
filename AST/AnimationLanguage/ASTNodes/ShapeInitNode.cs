namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a shape initialization node in the AST. A shape initialization node is only the initialization of a shape, and does not handle the movements of the shape at later times.
public class ShapeInitNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.ShapeInit;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public TypeNode Type { get; set; } //The type of shape that is being initialized. TODO: Add type checking to ensure that the type is a shape type.
    public IList<ArgumentNode> Arguments { get; } = new List<ArgumentNode>(); //The arguments used for initializing the shape.

    public ShapeInitNode(TypeNode type, IEnumerable<ArgumentNode> arguments, SourceLocation sourceLocation)
    {
        Type = type;
        SourceLocation = sourceLocation;

        foreach (ArgumentNode argument in arguments)
        {
            Arguments.Add(argument);
            Children.Add(argument);
        }
    }
}