namespace AnimationLanguage.ASTNodes;

using ASTCommon;
using System.Collections.Generic;


public class SequenceCallNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; } 
    public NodeType NodeType => NodeType.SequenceCall;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IdentifierNode Identifier { get; set; } //The identifier of the sequence call.
    public IList<ArgumentNode> Arguments { get; set; } //The arguments of the sequence call.

    public SequenceCallNode(IdentifierNode identifier, IEnumerable<ArgumentNode> arguments, SourceLocation sourceLocation)
    {
        Identifier = identifier;
        Arguments = new List<ArgumentNode>(arguments);
        SourceLocation = sourceLocation;

        Children.Add(identifier);
        foreach (var arg in Arguments)
        {
            Children.Add(arg);
        }
    }
}