namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents an assignment statement in the animation language.
public class AssignmentNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; } //This is the location of the node in the source code.
    public NodeType NodeType => NodeType.Assignment; //This defines that the NodeType of this node is Assignment.
    
    //The children of this node are the identifier and expression nodes. The lower indexes are the leftmost children, while the higher indexes are the rightmost children.
    public IList<IASTNode> Children { get; } = new List<IASTNode>();
    
    public IdentifierNode Identifier { get; set; } //This is the identifier (variable) that is being assigned to.
    public AssignmentOperator AssignmentOperator { get; set; } // Add the AssignmentOperator property
    public ExpressionNode Expression { get; set; } //This is the expression that is being assigned to the identifier.

    //Constructor taking in the identifier, expression, and source location:
    public AssignmentNode(IdentifierNode identifier, AssignmentOperator assignmentOperator, ExpressionNode expression, SourceLocation sourceLocation)
    {
        Identifier = identifier;
        AssignmentOperator = assignmentOperator; // Assign the value of the assignmentOperator parameter
        Expression = expression;
        SourceLocation = sourceLocation; //Assign the value of the sourceLocation parameter to the SourceLocation property.
        Children.Add(identifier);
        Children.Add(expression);
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
}


public enum AssignmentOperator //The different types of assignment operators.
{
    Assign,
    PlusEqual,
    MinusEqual,
}