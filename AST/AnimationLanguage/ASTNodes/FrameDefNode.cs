namespace AnimationLanguage.ASTNodes;
using ASTCommon;

// This class represents a frame definition node in the AST. A frame definition node defines a frame in the animation.
public class FrameDefNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.FrameDef;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public int FrameTime { get; set; } //FrameTime represents the time of the frame
    public SequenceCallNode SequenceCall { get; set; } //FunctionCall represents the function call that is executed in the frame

    public FrameDefNode(int frameTime, SequenceCallNode sequenceCall, SourceLocation sourceLocation)
    {
        FrameTime = frameTime;
        SequenceCall = sequenceCall;
        SourceLocation = sourceLocation;

        Children.Add(sequenceCall);
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
    
    
    public override string ToString()
    {
        return $"FrameDefNode: {FrameTime}";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}