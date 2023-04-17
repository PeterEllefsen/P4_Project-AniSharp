namespace AnimationLanguage.ASTNodes;
using ASTCommon;
// This class represents an expression in the animation language.
public class ExpressionNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; } 
    public NodeType NodeType => NodeType.Expression; //This defines that the NodeType of this node is Expression.
    
    //The children of this node are the left and right operands. The lower indexes are the leftmost children, while the higher indexes are the rightmost children.
    public IList<IASTNode> Children { get; } = new List<IASTNode>(); 

    public ExpressionNodeType ExpressionType { get; set; } //This is the type of expression e.g. Binary, Unary, Literal, Identifier, FunctionCall, ShapeInit, Term.
    public IASTNode? LeftOperand { get; set; } //This is the left operand of the expression.
    public IASTNode? RightOperand { get; set; } //This is the right operand of the expression.
    public OperatorNode? OperatorNode { get; set; } //This is the operator node of the expression. It is optional because some expressions do not have an operator node.
    public IntegerLiteralNode? IntegerLiteral { get; set; } //This is the integer literal node of the expression. It is optional because some expressions do not have an integer literal node.
    public FloatLiteralNode? FloatLiteral { get; set; } //This is the float literal node of the expression. It is optional because some expressions do not have a float literal node.
    public StringLiteralNode? StringLiteral { get; set; } //This is the string literal node of the expression. It is optional because some expressions do not have a string literal node.
    public BooleanLiteralNode? BooleanLiteral { get; set; } //This is the boolean literal node of the expression. It is optional because some expressions do not have a boolean literal node.
    public IdentifierNode? Identifier { get; set; } //This is the identifier node of the expression. It is optional because some expressions do not have an identifier node.
    public FunctionCallNode? FunctionCall { get; set; } //This is the function call node of the expression. It is optional because some expressions do not have a function call node.
    public ShapeInitNode? ShapeInit { get; set; } //This is the shape init node of the expression. It is optional because some expressions do not have a shape init node.
    public TermNode? Term { get; set; } //This is the term node of the expression. It is optional because some expressions do not have a term node.
    

    public ExpressionNode(
        ExpressionNodeType expressionType,
        IASTNode? leftOperand,
        IASTNode? rightOperand,
        OperatorNode operatorNode,
        SourceLocation sourceLocation)
    {
        ExpressionType = expressionType;
        LeftOperand = leftOperand;
        RightOperand = rightOperand;
        OperatorNode = operatorNode;
        SourceLocation = sourceLocation;

        if (leftOperand != null)
        {
            Children.Add(leftOperand);
        }

        if (rightOperand != null)
        {
            Children.Add(rightOperand);
        }
    }
}

    public enum ExpressionNodeType //This enum defines the types of expressions.
    {
        Binary,
        Unary,
        Literal,
        Identifier,
        FunctionCall,
        ShapeInit,
        Term
    }