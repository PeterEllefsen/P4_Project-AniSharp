// IdentifierGroupingNode.cs
namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents an identifier grouping node in the AST. An identifier grouping consists of an identifier followed by grouping elements node.
public class IdentifierGroupingNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.IdentifierGrouping;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IdentifierNode Identifier { get; set; } // Represents the identifier.
    public GroupingElementsNode GroupingElements { get; set; } // Represents the grouping elements.

    public IdentifierGroupingNode(IdentifierNode identifier, GroupingElementsNode groupingElements, SourceLocation sourceLocation)
    {
        Identifier = identifier;
        GroupingElements = groupingElements;
        SourceLocation = sourceLocation;

        Children.Add(identifier);
        Children.Add(groupingElements);
    }
}