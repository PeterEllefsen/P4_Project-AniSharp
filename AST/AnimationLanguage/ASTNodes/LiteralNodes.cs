namespace AnimationLanguage.ASTNodes;

using ASTCommon;

public class IntegerLiteralNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.IntegerLiteral;
    public int Value { get; set; } //This is the value of the integer literal.

    public IntegerLiteralNode(int value, SourceLocation sourceLocation)
    {
        Value = value;
        SourceLocation = sourceLocation;
    }
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Enumerable.Empty<IASTNode>(); // Integer literals have no children, so it returns an empty list.
    }
}

public class FloatLiteralNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.FloatLiteral;
    public float Value { get; set; } //This is the value of the float literal.

    public FloatLiteralNode(float value, SourceLocation sourceLocation)
    {
        Value = value;
        SourceLocation = sourceLocation;
    }
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Enumerable.Empty<IASTNode>();
    }
}

public class StringLiteralNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.StringLiteral;
    public string Value { get; set; } //This is the value of the string literal.

    public StringLiteralNode(string value, SourceLocation sourceLocation)
    {
        Value = value;
        SourceLocation = sourceLocation;
    }
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Enumerable.Empty<IASTNode>();
    }

}

public class BooleanLiteralNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.BooleanLiteral;
    public bool Value { get; set; } //This is the value of the boolean literal.

    public BooleanLiteralNode(bool value, SourceLocation sourceLocation)
    {
        Value = value;
        SourceLocation = sourceLocation;
    }
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Enumerable.Empty<IASTNode>();
    }

}
