namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class IdentifierNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Identifier;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public string? Name { get; set; }
}
