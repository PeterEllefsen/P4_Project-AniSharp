namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class WhileLoopNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.WhileLoop;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IASTNode Condition { get; set; } // Represents the condition that must be met for the while loop to continue running.
    public BlockNode Body { get; set; } // Represents the body of the while loop.

    public WhileLoopNode(
        IASTNode condition,
        BlockNode body,
        SourceLocation sourceLocation)
    {
        Condition = condition;
        Body = body;
        SourceLocation = sourceLocation;

        Children.Add(condition);
        Children.Add(body);
    }
}