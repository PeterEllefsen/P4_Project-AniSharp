namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class TimelineBlockNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.TimelineBlock;
    public int StartTime { get; set; }
    public int EndTime { get; set; }
    public IList<FrameDefNode> FrameDefinitions { get; set; }

    public TimelineBlockNode(int startTime, int endTime, IList<FrameDefNode> frameDefinitions, SourceLocation sourceLocation)
    {
        StartTime = startTime;
        EndTime = endTime;
        FrameDefinitions = frameDefinitions;
        SourceLocation = sourceLocation;
    }

    public IEnumerable<IASTNode> GetChildren()
    {
        return FrameDefinitions;
    }
    
    
    public override string ToString()
    {
        return $"TimelineBlockNode: StartTime: {StartTime}, EndTime: {EndTime}, FrameDefinitions: [{string.Join(", ", FrameDefinitions)}]";
    }
    
    
    public T Accept<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}