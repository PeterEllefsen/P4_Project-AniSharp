namespace AnimationLanguage.ASTNodes;
using ASTCommon;
{
    public class GroupingNode : IASTNode
    {
        public SourceLocation SourceLocation { get; set; }
        public NodeType NodeType => NodeType.Grouping;
        public IList<IASTNode> Children { get; } = new List<IASTNode>();

        public IList<GroupingElementNode> GroupingElements { get; } = new List<GroupingElementNode>();

        public GroupingNode(IEnumerable<GroupingElementNode> groupingElements, SourceLocation sourceLocation)
        {
            GroupingElements.AddRange(groupingElements);
            SourceLocation = sourceLocation;

            Children.AddRange(groupingElements);
        }
    }
}