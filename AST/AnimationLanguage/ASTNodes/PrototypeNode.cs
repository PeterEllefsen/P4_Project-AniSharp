namespace AnimationLanguage.ASTNodes;
using ASTCommon;
public class PrototypeNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Prototype;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public DataType ReturnType { get; set; } //The return type of the function.
    public string FunctionName { get; set; } //The name of the function.
    public IList<ParameterNode> Parameters { get; } = new List<ParameterNode>(); //The parameters of the function.

    public PrototypeNode(
        DataType returnType, 
        string functionName,
        IEnumerable<ParameterNode> parameters, 
        SourceLocation sourceLocation)
    {
        ReturnType = returnType;
        FunctionName = functionName;
        Parameters = new List<ParameterNode>(parameters);
        SourceLocation = sourceLocation;

        foreach (ParameterNode parameterNode in parameters)
        {
            Children.Add(parameterNode);
        }
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
    
    
    public override string ToString()
    {
        return $"PrototypeNode: {ReturnType} {FunctionName}({string.Join(", ", Parameters)})";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}

public enum DataType //This enum defines the different data types that can be used in the language.
{
    Void,
    Int,
    Float,
    String,
    Bool,
    Group,
    Circle,
    Polygon
}