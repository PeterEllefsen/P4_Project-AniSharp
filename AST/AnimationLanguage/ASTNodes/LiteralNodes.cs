﻿namespace AnimationLanguage.ASTNodes;

using ASTCommon;

public class IntegerLiteralNode : ExpressionNode
{
    public int Value { get; }

    public IntegerLiteralNode(int value, SourceLocation sourceLocation)
        : base(
            ExpressionNodeType.Literal,
            null, // LeftOperand
            null, // RightOperand
            null, // OperatorNode
            sourceLocation)
    {
        Value = value;
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
            sourceLocation)
    {
        Value = value;
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
            sourceLocation)
    {
        Value = value;
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
            sourceLocation)
    {
        Value = value;
    }
}

