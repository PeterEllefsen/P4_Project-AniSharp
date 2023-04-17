namespace AnimationLanguage.ASTNodes;
using System.Collections.Generic;
using ASTCommon;

public class TimelineNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Timeline;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();