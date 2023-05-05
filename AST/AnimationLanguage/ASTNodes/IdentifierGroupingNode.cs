// IdentifierGroupingNode.cs
namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents an identifier grouping node in the AST. An identifier grouping consists of an identifier followed by grouping elements node.
public class IdentifierGroupingNode : StatementNode
{
    public IdentifierNode Identifier { get; set; } // Represents the identifier.
    public GroupingElementsNode GroupingElements { get; set; } // Represents the grouping elements.

    public IdentifierGroupingNode(IdentifierNode identifier, GroupingElementsNode groupingElements, SourceLocation sourceLocation)
        : base(sourceLocation)
    {
        Identifier = identifier;
        GroupingElements = groupingElements;
        NodeType = NodeType.IdentifierGrouping;
    }
    
    
    public override string ToString()
    {
        return $"IdentifierGroupingNode: {Identifier} {GroupingElements}";
    }
    
    
    public T Accept<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}