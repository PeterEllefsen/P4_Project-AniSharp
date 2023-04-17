namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a parameter in the AST.
public class ParameterNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Parameter;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public DataType DataType { get; set; } //The data type of the parameter.
    public string Name { get; set; } //The name of the parameter.

    public ParameterNode(DataType dataType, string name, SourceLocation sourceLocation)
    {
        DataType = dataType;
        Name = name;
        SourceLocation = sourceLocation;
    }
}
