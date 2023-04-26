namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a time block in the animation language. A time block is a block of code that executes sequences of animations at specific times.
public class TimelineBlockNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.TimelineBlock;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public int StartTime { get; set; } // Represents the start time of the timeline.
    public int EndTime { get; set; } // Represents the end time of the timeline.
    public IList<AssignmentNode> Assignments { get; } = new List<AssignmentNode>(); // Represents the assignments that are executed in the timeline.

    public TimelineBlockNode(int startTime, int endTime, IEnumerable<AssignmentNode> assignments, SourceLocation sourceLocation)
    {
        StartTime = startTime;
        EndTime = endTime;
        SourceLocation = sourceLocation;

        foreach (AssignmentNode assignmentNode in assignments)
        {
            Assignments.Add(assignmentNode);
            Children.Add(assignmentNode);
        }
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
}