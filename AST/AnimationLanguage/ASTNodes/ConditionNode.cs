using AnimationLanguage.ASTCommon;

namespace AnimationLanguage.ASTNodes;

//This class represents a condition node in the AST. A condition consists of a left expression, a right expression, a comparison operator and a logical operator.
public class ConditionNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Condition;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IASTNode LeftExpression { get; set; } // Represents the left expression.
    public IASTNode RightExpression { get; set; } // Represents the right expression.
    public OperatorNode? ComparisonOperator { get; set; } // Represents the comparison operator. (eg. ==, !=, <, >, <=, >=)
    public OperatorNode? LogicalOperator { get; set; } // Represents the logical operator. ('and' & 'or')


    public ConditionNode(IASTNode leftExpression, IASTNode rightExpression, OperatorNode? comparisonOperator, OperatorNode? logicalOperator, SourceLocation sourceLocation)
    {
        LeftExpression = leftExpression;
        RightExpression = rightExpression;
        ComparisonOperator = comparisonOperator;
        LogicalOperator = logicalOperator;
        SourceLocation = sourceLocation;

        Children.Add(leftExpression);
        Children.Add(rightExpression);
    }
}
