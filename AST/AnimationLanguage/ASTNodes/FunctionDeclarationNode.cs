namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class FunctionDeclarationNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.FunctionDeclaration;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public TypeNode ReturnType { get; set; } // Represents the function's return type.
    public IdentifierNode Identifier { get; set; }
    public IList<ParameterNode> Parameters { get; } = new List<ParameterNode>();
    public BlockNode Block { get; set; }

    public FunctionDeclarationNode(
        TypeNode returnType,
        IdentifierNode identifier,
        IEnumerable<ParameterNode> parameters,
        BlockNode block,
        SourceLocation sourceLocation)
    {
        ReturnType = returnType;
        Identifier = identifier;
        Block = block;
        SourceLocation = sourceLocation;

        Children.Add(returnType);
        Children.Add(identifier);

        foreach (ParameterNode parameterNode in parameters)
        {
            Parameters.Add(parameterNode);
            Children.Add(parameterNode);
        }

        Children.Add(block);
    }

    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }

    public override string ToString()
    {
        string parametersStr = string.Join(", ", Parameters.Select(p => p.ToString()));
        return $"FunctionDeclarationNode: {ReturnType} {Identifier}({parametersStr})";
    }
}