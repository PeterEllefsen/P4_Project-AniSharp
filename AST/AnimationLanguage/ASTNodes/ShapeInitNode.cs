namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a shape initialization node in the AST.
public class ShapeInitNode : ExpressionNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.ShapeInit;

    public TypeNode ShapeType { get; set; } //The type of shape that is being initialized.
    public Dictionary<string, IASTNode> Arguments { get; set; } //The arguments that are being provided when initializing the shape.

    public ShapeInitNode(TypeNode shapeType, Dictionary<string, IASTNode> arguments, SourceLocation sourceLocation)
        : base(ExpressionNodeType.ShapeInit, null, null, null, sourceLocation)
    {
        ShapeType = shapeType;
        Arguments = arguments;
        SourceLocation = sourceLocation;
    }
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Arguments.Values; //The arguments are the children of the ShapeInit node, so they are returned.
    }
    
    
    public override string ToString()
    {
        return $"ShapeInitNode: {ShapeType} {Arguments}";
    }
}