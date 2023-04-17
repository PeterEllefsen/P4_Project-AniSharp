namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class OperatorNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Operator; //This defines that the NodeType of this node is Operator.
    public IList<IASTNode> Children { get; } = new List<IASTNode>(); //Operators have no children.

    public string OperatorSymbol { get; set; } //This is the symbol of the operator.

    public OperatorNode(string operatorSymbol, SourceLocation sourceLocation)
    {
        OperatorSymbol = operatorSymbol;
        SourceLocation = sourceLocation;
    }
}