namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class IfStatementNode : StatementNode
{
    public ExpressionNode Condition { get; set; } // Represents the condition that must be met for the if statement to run.
    public BlockNode IfBlock { get; set; } // Represents the body of the if statement.
    public IList<ElseIfNode> ElseIfBranches { get; set; } // Represents the else if branches of the if statement.
    public ElseNode? ElseBranch { get; set; } // Represents the else branch of the if statement.

    public IfStatementNode(
        ExpressionNode condition,
        BlockNode ifBlock,
        IEnumerable<ElseIfNode> elseIfBranches,
        ElseNode? elseBranch,
        SourceLocation sourceLocation)
        : base(sourceLocation)
    {
        Condition = condition;
        IfBlock = ifBlock;
        ElseIfBranches = new List<ElseIfNode>(elseIfBranches);
        ElseBranch = elseBranch;
        NodeType = NodeType.IfStatement;
    }
    
    
    public override string ToString()
    {
        return $"IfStatementNode: {Condition}";
    }
}