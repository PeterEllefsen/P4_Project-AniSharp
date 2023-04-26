namespace AnimationLanguage.ASTNodes;
using System.Collections.Generic;
using ASTCommon;

public class FunctionCallNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.FunctionCall;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IdentifierNode FunctionIdentifier { get; set; } // Represents the identifier(name) of the function that is being called.
    public IList<ArgumentNode> Arguments { get; } = new List<ArgumentNode>(); // Represents the arguments that are being provided when calling the function.

    public FunctionCallNode(
        IdentifierNode functionIdentifier,
        IEnumerable<ArgumentNode> arguments,
        SourceLocation sourceLocation)
    {
        FunctionIdentifier = functionIdentifier;
        SourceLocation = sourceLocation;
        
        foreach (ArgumentNode argument in arguments)
        {
            Arguments.Add(argument);
            Children.Add(argument);
        }
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
}