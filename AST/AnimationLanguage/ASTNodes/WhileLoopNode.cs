namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class WhileLoopNode : StatementNode
{
    public IASTNode Condition { get; set; } // Represents the condition that must be met for the while loop to continue running.
    public BlockNode Body { get; set; } // Represents the body of the while loop.

    public WhileLoopNode(
        IASTNode condition,
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
    
    
    public T Accept<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}