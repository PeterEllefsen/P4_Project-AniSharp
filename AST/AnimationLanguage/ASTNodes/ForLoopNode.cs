namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class ForLoopNode : StatementNode
{
    public IASTNode Initialization { get; set; } // Represents the initialization of the for loop.
    public ExpressionNode Condition { get; set; } // Represents the condition that must be met for the for loop to continue running.
    public IASTNode Update { get; set; } // Represents the update of the for loop.
    public BlockNode Body { get; set; } // Represents the body of the for loop.

    public ForLoopNode(
        IASTNode initialization,
        ExpressionNode condition,
        IASTNode update,
        BlockNode body,
        SourceLocation sourceLocation)
        : base(sourceLocation)
    {
        Initialization = initialization;
        Condition = condition;
        Update = update;
        Body = body;
        NodeType = NodeType.ForLoop;
    }
    
    public override string ToString()
    {
        return $"ForLoopNode(Initialization: {Initialization}, Condition: {Condition}, Update: {Update}, Body: {Body})";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }

}