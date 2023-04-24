namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a sequence block part node in the AST. A sequence block part consists of a single statement.
public class SeqBlockPartNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.SeqBlockPart;
    public IASTNode Child { get; set; } // Represents the statement within the sequence block part.

    public SeqBlockPartNode(IASTNode child, SourceLocation sourceLocation)
    {
        Child = child;
        SourceLocation = sourceLocation;
    }
}