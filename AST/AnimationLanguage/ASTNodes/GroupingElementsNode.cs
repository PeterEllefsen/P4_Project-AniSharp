namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a grouping element in a grouping in the AST.
public class GroupingElementsNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.GroupingElements;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IList<ExpressionNode> Expressions { get; } = new List<ExpressionNode>(); //The expressions of the grouping elements.
    public IList<IdentifierNode> Identifiers { get; } = new List<IdentifierNode>(); //The identifiers of the grouping elements.
    public IList<KeyValuePairNode> KeyValuePairs { get; } = new List<KeyValuePairNode>(); //The key-value pairs of the grouping elements.

    public GroupingElementsNode(IEnumerable<ExpressionNode> expressions, IEnumerable<IdentifierNode> identifiers, IEnumerable<KeyValuePairNode> keyValuePairs, SourceLocation sourceLocation)
    {
        foreach (ExpressionNode expressionNode in expressions)
        {
            Expressions.Add(expressionNode);
            Children.Add(expressionNode);
        }

        foreach (IdentifierNode identifierNode in identifiers)
        {
            Identifiers.Add(identifierNode);
            Children.Add(identifierNode);
        }

        foreach (KeyValuePairNode keyValuePairNode in keyValuePairs)
        {
            KeyValuePairs.Add(keyValuePairNode);
            Children.Add(keyValuePairNode);
        }

        SourceLocation = sourceLocation;
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
}