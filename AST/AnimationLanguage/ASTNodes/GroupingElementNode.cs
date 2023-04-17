namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a grouping element in a grouping in the AST.
public class GroupingElementNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.GroupingElement;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public GroupingElementType ElementType { get; set; } //Grouping elements can be expressions, identifiers or key-value pairs.
    public ExpressionNode? Expression { get; set; } //The expression of the grouping element if there is one.
    public IdentifierNode? Identifier { get; set; } //The identifier of the grouping element if there is one.
    public KeyValuePairNode? KeyValuePair { get; set; } //The key-value pair of the grouping element if there is one.

    public GroupingElementNode(GroupingElementType elementType, ExpressionNode? expression, IdentifierNode? identifier, KeyValuePairNode? keyValuePair, SourceLocation sourceLocation)
    {
        ElementType = elementType;
        Expression = expression;
        Identifier = identifier;
        KeyValuePair = keyValuePair;
        SourceLocation = sourceLocation;

        if (expression != null)
        {
            Children.Add(expression); //If the grouping element is an expression, add it as a child.
        }

        if (identifier != null)
        {
            Children.Add(identifier); //If the grouping element is an identifier, add it as a child.
        }

        if (keyValuePair != null)
        {
            Children.Add(keyValuePair); //If the grouping element is a key-value pair, add it as a child.
        }
    }
}

public enum GroupingElementType //The different types of grouping elements that can exist in a grouping.
{
    Expression,
    Identifier,
    KeyValuePair
}
