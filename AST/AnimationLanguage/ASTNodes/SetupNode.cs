namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a setup node in the AST. Setup consists of a grouping of statements.
public class SetupNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Setup;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public GroupingElementsNode GroupingElements { get; set; } //The grouping elements of the setup node.

    public SetupNode(GroupingElementsNode groupingElements, SourceLocation sourceLocation)
    {
        GroupingElements = groupingElements; 
        SourceLocation = sourceLocation;

        Children.Add(groupingElements);
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
    
    
    public override string ToString()
    {
        return $"SetupNode: {GroupingElements}";
    }
}