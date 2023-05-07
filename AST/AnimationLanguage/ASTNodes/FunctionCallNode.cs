namespace AnimationLanguage.ASTNodes;
using System.Collections.Generic;
using ASTCommon;

public class FunctionCallNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.FunctionCall;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IdentifierNode FunctionIdentifier { get; set; } // Represents the identifier(name) of the function that is being called.
    public IList<IASTNode> Arguments { get; } = new List<IASTNode>(); // Represents the arguments that are being provided when calling the function.
    public TypeNode? Type { get; set;} // The type of the function call node. This is set by the type checker.

    public FunctionCallNode(
        IdentifierNode functionIdentifier,
        IEnumerable<IASTNode> arguments,
        SourceLocation sourceLocation)
    {
        FunctionIdentifier = functionIdentifier;
        SourceLocation = sourceLocation;
        
        foreach (IASTNode argument in arguments)
        {
            Arguments.Add(argument);
            Children.Add(argument);
        }
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
    
    
    public override string ToString()
    {
        string argumentsStr = string.Join(", ", Arguments.Select(a => a.ToString()));
        return $"FunctionCallNode: {FunctionIdentifier}({argumentsStr})";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}