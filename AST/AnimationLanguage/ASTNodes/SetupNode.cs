namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a setup node in the AST. Setup consists of a grouping of statements.
public class SetupNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Setup;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public GroupingNode Grouping { get; set; } //The grouping node of the setup node.

    public SetupNode(GroupingNode grouping, SourceLocation sourceLocation)
    {
        Grouping = grouping; 
        SourceLocation = sourceLocation;

        Children.Add(grouping);
    }
}