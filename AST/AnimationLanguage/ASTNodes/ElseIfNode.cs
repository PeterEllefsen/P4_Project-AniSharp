namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class ElseIfNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.ElseIf;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public ConditionNode Condition { get; set; } // Represents the condition that must be met for the else if block to be executed.
    public BlockNode ElseIfBlock { get; set; } // Represents the block of code that is executed if the condition is met. This block contains a collection of statements.

    public ElseIfNode(ConditionNode condition, BlockNode elseIfBlock, SourceLocation sourceLocation)
    {
        Condition = condition;
        ElseIfBlock = elseIfBlock;
        SourceLocation = sourceLocation;

        Children.Add(condition);
        Children.Add(elseIfBlock);
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
    
    
    public override string ToString()
    {
        return $"ElseIfNode: {Condition}";
    }
    
    
    public T Accept<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}