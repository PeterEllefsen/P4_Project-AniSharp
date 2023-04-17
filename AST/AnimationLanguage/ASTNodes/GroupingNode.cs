namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a grouping node in the AST. A grouping node is a grouping of grouping elements.
public class GroupingNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Grouping;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IList<GroupingElementNode> GroupingElements { get; } = new List<GroupingElementNode>();

    public GroupingNode(IEnumerable<GroupingElementNode> groupingElements, SourceLocation sourceLocation)
    {
        foreach (GroupingElementNode groupingElementNode in groupingElements) //Add all grouping elements as children.
        {
            GroupingElements.Add(groupingElementNode);
            Children.Add(groupingElementNode);
        }
        SourceLocation = sourceLocation;
    }
}