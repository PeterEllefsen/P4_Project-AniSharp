namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class WhileLoopNode : StatementNode
{
    public ExpressionNode Condition { get; set; } // Represents the condition that must be met for the while loop to continue running.
    public BlockNode Body { get; set; } // Represents the body of the while loop.

    public WhileLoopNode(
        ExpressionNode condition,
        BlockNode body,
        SourceLocation sourceLocation)
        : base(sourceLocation)
    {
        Condition = condition;
        Body = body;
        NodeType = NodeType.WhileLoop;
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return new List<IASTNode> {Condition, Body};
    }
    
    
    public override string ToString()
    {
        return $"WhileLoopNode: {Condition}, {Body}";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}