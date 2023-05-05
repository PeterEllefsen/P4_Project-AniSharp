namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents an expression node in the AST. An expression node is a node that represents an expression in the animation language.
//An expression is a combination of values, variables, operators, and function calls that evaluates to a single value.
public class ExpressionNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Expression;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public ExpressionNodeType ExpressionType { get; set; } //The type of expression this node represents.
    public IASTNode? LeftOperand { get; set; } //The left operand of the expression. This is an IASTNode because it can be any type of node.
    public IASTNode? RightOperand { get; set; } //The right operand of the expression. This is an IASTNode because it can be any type of node.
    public OperatorNode? OperatorNode { get; set; } //The operator node of the expression.
    public IASTNode? Value { get; set; } //The value of the expression. This is an IASTNode because it can be any type of node.
    
    public ExpressionNode(
        ExpressionNodeType expressionType, 
        IASTNode? leftOperand,
        IASTNode? rightOperand,
        OperatorNode? operatorNode,
        SourceLocation sourceLocation)
    {
        ExpressionType = expressionType;
        LeftOperand = leftOperand;
        RightOperand = rightOperand;
        OperatorNode = operatorNode;
        SourceLocation = sourceLocation;

        if (leftOperand != null)
        {
            Children.Add(leftOperand); //If there is a left operand, add it to the children list.
        }

        if (rightOperand != null)
        {
            Children.Add(rightOperand); //If there is a right operand, add it to the children list.
        }
    }


    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
    
    
    public override string ToString()
    {
        return $"ExpressionNode: {LeftOperand} {OperatorNode} {RightOperand}";
    }
    
    
    public T Accept<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
    
}

public enum ExpressionNodeType //This enum defines the different types of expression nodes.
{
    Binary,
    Unary,
    Literal,
    Identifier,
    FunctionCall,
    ShapeInit,
    Term
}