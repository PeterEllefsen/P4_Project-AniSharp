namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class ReturnNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Return;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IASTNode? ReturnValue { get; set; } //The value that is goingg to be returned by the function when it is called.

    public ReturnNode(IASTNode? returnValue, SourceLocation sourceLocation)
    {
        ReturnValue = returnValue;
        SourceLocation = sourceLocation;

        if (returnValue != null)
        {
            Children.Add(returnValue);
        }
    }
}