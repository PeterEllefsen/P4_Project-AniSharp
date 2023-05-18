namespace AnimationLanguage.ASTNodes;

using ASTCommon;
using System.Collections.Generic;


public class SequenceCallNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.SequenceCall;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IdentifierNode Name { get; set; }
    public IList<ExpressionNode> Arguments { get; } = new List<ExpressionNode>();

    public SequenceCallNode(
        IdentifierNode name,
        IEnumerable<ExpressionNode> arguments,
        SourceLocation sourceLocation)
    {
        Name = name;
        SourceLocation = sourceLocation;

        Children.Add(name);

        foreach (ExpressionNode argumentNode in arguments)
        {
            Arguments.Add(argumentNode);
            Children.Add(argumentNode);
        }
    }

    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }

    public override string ToString()
    {
        string argumentsStr = string.Join(", ", Arguments.Select(a => a.ToString()));
        return $"{Name}({argumentsStr})";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}