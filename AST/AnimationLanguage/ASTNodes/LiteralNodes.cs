namespace AnimationLanguage.ASTNodes;

using ASTCommon;

public class IntegerLiteralNode : ExpressionNode
{
    public int Value { get; }
    private TypeNode _typeNode;

    public IntegerLiteralNode(int value, SourceLocation sourceLocation)
        : base(
            ExpressionNodeType.Literal,
            null, // LeftOperand
            null, // RightOperand
            null, // OperatorNode
            VariableType.Int,
            sourceLocation)
    {
        Value = value;
        _typeNode = new TypeNode(TypeNode.TypeKind.Int, sourceLocation);
    }

    public new TypeNode? Type
    {
        get => _typeNode;
        set => _typeNode = value ?? throw new ArgumentNullException(nameof(value), "Type cannot be null.");
    }

    public override string ToString()
    {
        return $"IntegerLiteralNode: {Value}";
    }

    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}




public class FloatLiteralNode : ExpressionNode
{
    public float Value { get; }

    public FloatLiteralNode(float value, SourceLocation sourceLocation)
        : base(
            ExpressionNodeType.Literal,
            null, // LeftOperand
            null, // RightOperand
            null, // OperatorNode
            VariableType.Float,
            sourceLocation)
    {
        Value = value;
    }

    
    public override string ToString()
    {
        return $"FloatLiteralNode: {Value}";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}

public class StringLiteralNode : ExpressionNode
{
    public string Value { get; }

    public StringLiteralNode(string value, SourceLocation sourceLocation)
        : base(
            ExpressionNodeType.Literal,
            null, // LeftOperand
            null, // RightOperand
            null, // OperatorNode
            VariableType.String,
            sourceLocation)
    {
        Value = value;
    }

    
    public override string ToString()
    {
        return $"StringLiteralNode: {Value}";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}

public class BooleanLiteralNode : ExpressionNode
{
    public bool Value { get; }

    public BooleanLiteralNode(bool value, SourceLocation sourceLocation)
        : base(
            ExpressionNodeType.Literal,
            null, // LeftOperand
            null, // RightOperand
            null, // OperatorNode
            VariableType.Bool,
            sourceLocation)
    {
        Value = value;
    }

    
    public override string ToString()
    {
        return $"BooleanLiteralNode: {Value}";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}

