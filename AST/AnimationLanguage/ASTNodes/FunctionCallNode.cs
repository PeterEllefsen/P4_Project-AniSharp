namespace AnimationLanguage.ASTNodes;
using System.Collections.Generic;
using ASTCommon;

public class FunctionCallNode : ExpressionNode
{
    public IdentifierNode FunctionIdentifier { get; set; } // Represents the identifier(name) of the function that is being called.
    public IList<IASTNode> Arguments { get; } = new List<IASTNode>(); // Represents the arguments that are being provided when calling the function.

    public FunctionCallNode(
        IdentifierNode functionIdentifier,
        IEnumerable<IASTNode> arguments,
        SourceLocation sourceLocation)
        : base(ExpressionNodeType.FunctionCall, null, null, null, VariableType.Function, sourceLocation) // Assuming Function is a valid VariableType
    {
        FunctionIdentifier = functionIdentifier;
        
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
        return $"{FunctionIdentifier}({argumentsStr})";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}