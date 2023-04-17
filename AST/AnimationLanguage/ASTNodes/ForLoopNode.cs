namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class ForLoopNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.ForLoop;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IASTNode Initialization { get; set; } // Represents the initialization of the for loop.
    public IASTNode Condition { get; set; } // Represents the condition that must be met for the for loop to continue.
    public IASTNode Update { get; set; } // Represents the update of the for loop.
    public BlockNode Body { get; set; } // Represents the body of the for loop.

    public ForLoopNode(
        IASTNode initialization,
        IASTNode condition,
        IASTNode update,
        BlockNode body,
        SourceLocation sourceLocation)
    {
        Initialization = initialization;
        Condition = condition;
        Update = update;
        Body = body;
        SourceLocation = sourceLocation;

        Children.Add(initialization);
        Children.Add(condition);
        Children.Add(update);
        Children.Add(body);
    }
}