namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a sequence block node in the AST. A sequence block consists of a collection of sequence block parts.
public class SeqBlockNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.SeqBlock;
    public IList<SeqBlockPartNode> SeqBlockParts { get; set; } // Represents all of the sequence block parts within the sequence block.

    public SeqBlockNode(IList<SeqBlockPartNode> seqBlockParts, SourceLocation sourceLocation)
    {
        SeqBlockParts = seqBlockParts;
        SourceLocation = sourceLocation;
    }
}