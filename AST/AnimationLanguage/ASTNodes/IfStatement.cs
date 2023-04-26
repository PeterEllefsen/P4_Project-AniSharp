namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class IfStatementNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.IfStatement;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public ExpressionNode Condition { get; set; } // Represents the condition that must be met for the if block to be executed.
    public BlockNode IfBlock { get; set; } // Represents the block of code that is executed if the condition is met. This block contains a collection of statements.
    public IList<ElseIfNode> ElseIfBranches { get; set; } // Represents the else if branches of the if statement.
    public ElseNode? ElseBranch { get; set; } // Represents the else branch of the if statement.

    public IfStatementNode(
        ExpressionNode condition,
        BlockNode ifBlock,
        IEnumerable<ElseIfNode> elseIfBranches,
        ElseNode? elseBranch,
        SourceLocation sourceLocation)
    {
        Condition = condition;
        IfBlock = ifBlock;
        ElseIfBranches = new List<ElseIfNode>(elseIfBranches);
        ElseBranch = elseBranch;
        SourceLocation = sourceLocation;

        Children.Add(condition);
        Children.Add(ifBlock);
        
        foreach (ElseIfNode elseIfBranch in elseIfBranches)
        {
            Children.Add(elseIfBranch); // Add all else if branches as children.
        }

        if (elseBranch != null)
        {
            Children.Add(elseBranch); // Add the else branch as a child if it exists.
        }
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
}