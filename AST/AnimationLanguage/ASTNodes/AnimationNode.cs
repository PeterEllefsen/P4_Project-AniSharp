namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class AnimationNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Animation;

    public IdentifierNode Identifier { get; } // The identifier of the animation.
    public CommandNode? Command { get; } // The command of the animation. This is the command that is executed when the animation is called.
    public List<TransitionNode> Transitions { get; }

    public AnimationNode(
        IdentifierNode identifier,
        CommandNode? command,
        IEnumerable<TransitionNode> transitions,
        SourceLocation sourceLocation)
    {
        Identifier = identifier;
        Command = command;
        Transitions = new List<TransitionNode>(transitions);
        SourceLocation = sourceLocation;
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        yield return Identifier; 

        if (Command != null)
        {
            yield return Command;
        }

        foreach (var transition in Transitions)
        {
            yield return transition;
        }
    }
}