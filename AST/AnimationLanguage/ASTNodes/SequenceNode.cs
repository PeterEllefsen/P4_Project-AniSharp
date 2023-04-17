namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class SequenceNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Sequence;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IdentifierNode Identifier { get; set; } // Represents the identifier(name) of the sequence.
    public BlockNode Block { get; set; } // Represents the block of code that is executed in the sequence. This block contains a collection of statements.

    public SequenceNode(IdentifierNode identifier, BlockNode block, SourceLocation sourceLocation)
    {
        Identifier = identifier;
        Block = block;
        SourceLocation = sourceLocation;

        Children.Add(identifier);
        Children.Add(block);
    }
}