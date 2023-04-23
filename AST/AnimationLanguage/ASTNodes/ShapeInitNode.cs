namespace AnimationLanguage.ASTNodes
{
    using ASTCommon;

    //This class represents a shape initialization node in the AST.
    public class ShapeInitNode : IASTNode
    {
        public SourceLocation SourceLocation { get; set; }
        public NodeType NodeType => NodeType.ShapeInit;

        public TypeNode ShapeType { get; set; } //The type of shape that is being initialized.
        public Dictionary<string, IASTNode> Arguments { get; set; } //The arguments that are being provided when initializing the shape.

        public ShapeInitNode(TypeNode shapeType, Dictionary<string, IASTNode> arguments, SourceLocation sourceLocation)
        {
            ShapeType = shapeType;
            Arguments = arguments;
            SourceLocation = sourceLocation;
        }
    }
}