namespace AnimationLanguage.ASTNodes;
using ASTCommon;
using System.Text; //used to build the strings.

public class IfStatementNode : StatementNode
{
    public ConditionNode Condition { get; set; } // Represents the condition that must be met for the if statement to run.
    public BlockNode IfBlock { get; set; } // Represents the body of the if statement.
    public IList<ElseIfNode> ElseIfBranches { get; set; } // Represents the else if branches of the if statement.
    public ElseNode? ElseBranch { get; set; } // Represents the else branch of the if statement.

    public IfStatementNode(
        ConditionNode condition,
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
        StringBuilder sb = new StringBuilder();
        sb.Append($"IfStatementNode: {Condition}");

        sb.AppendLine();
        sb.Append($"IfBlock: {IfBlock}");

        foreach (var elseIfBranch in ElseIfBranches)
        {
            sb.AppendLine();
            sb.Append($"ElseIfBranch: {elseIfBranch}");
        }

        if (ElseBranch != null)
        {
            sb.AppendLine();
            sb.Append($"ElseBranch: {ElseBranch}");
        }

        return sb.ToString();
    }
    
    
    public T Accept<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

}