namespace AnimationLanguage.ASTNodes;
using ASTCommon;

// This class represents a sequence node in the AST. A sequence consists of an identifier, a collection of parameters, and a codeblock.
public class SequenceNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Sequence;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IdentifierNode Name { get; set; }
    public IList<ParameterNode> Parameters { get; } = new List<ParameterNode>();
    public SeqBlockNode Block { get; set; }

    public SequenceNode(
        IdentifierNode name,
        IEnumerable<ParameterNode> parameters,
        SeqBlockNode block,
        SourceLocation sourceLocation)
    {
        Name = name;
        Block = block;
        SourceLocation = sourceLocation;

        Children.Add(name);

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

        if (parametersStr.Length > 0)
        {
            return $"{Name}(int frameoffset, {parametersStr})";
        }
        else
        {
            return $"{Name}(int frameoffset)";
        }

        
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}