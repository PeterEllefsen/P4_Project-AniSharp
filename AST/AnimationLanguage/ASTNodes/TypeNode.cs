namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class TypeNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Type;

    public enum TypeKind //This enum is placed inside the TypeNode class because it is only used by the TypeNode class. It includes all the possible types that can be used in the animation language.
    {
        Int,
        Float,
        String,
        Bool,
        Circle,
        Polygon,
        Group,
        None,
    }

    public TypeKind Kind { get; set; } //The kind of type this node represents.

    public TypeNode(TypeKind kind, SourceLocation sourceLocation)
    {
        Kind = kind;
        SourceLocation = sourceLocation;
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return new List<IASTNode>(); //The TypeNode has no children, so an empty list is returned.
    }
    
    
    public override string ToString()
    {
        return $"TypeNode: {Kind}";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}