namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents an assignment statement in the animation language.
public class AssignmentNode : StatementNode
{
    public IdentifierNode Identifier { get; set; } //This is the identifier (variable) that is being assigned to.
    public AssignmentOperator AssignmentOperator { get; set; } // Add the AssignmentOperator property
    public ExpressionNode Expression { get; set; } //This is the expression that is being assigned to the identifier.

    //Constructor taking in the identifier, expression, and source location:
    public AssignmentNode(IdentifierNode identifier, AssignmentOperator assignmentOperator, ExpressionNode expression, SourceLocation sourceLocation)
        : base(sourceLocation)
    {
        Identifier = identifier;
        AssignmentOperator = assignmentOperator;
        Expression = expression;
        InitializeStatementNode();
    }

    private void InitializeStatementNode()
    {
        NodeType = NodeType.Assignment;
        Assignment = this;
        Children.Add(Identifier);
        Children.Add(Expression);
    }
    
    
    public override string ToString()
    {
        string assignmentOperatorStr = AssignmentOperator.ToString();
        return $"AssignmentNode: {Identifier} {assignmentOperatorStr} {Expression}";
    }
    
    
    public T Accept<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}


public enum AssignmentOperator //The different types of assignment operators.
{
    Assign,
    PlusEqual,
    MinusEqual,
    Null,
}