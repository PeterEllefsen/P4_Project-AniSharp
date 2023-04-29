namespace AnimationLanguage.ASTNodes;
using ASTCommon;

// This class represents a sequence node in the AST. A sequence consists of an identifier, a collection of parameters, and a codeblock.
public class SequenceNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; } 
    public NodeType NodeType => NodeType.Sequence;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IdentifierNode Identifier { get; set; } // Represents the identifier(name) of the sequence.
    public IList<ParameterNode>? Parameters { get; set; } // Represents the parameters of the sequence.
    public SeqBlockNode SeqBlock { get; set; } // Represents the block of code that is executed in the sequence. This block contains a collection of statements.

    public SequenceNode(IdentifierNode identifier, IList<ParameterNode>? parameters, SeqBlockNode seqBlock, SourceLocation sourceLocation)
    {
        Identifier = identifier;
        Parameters = parameters;
        SeqBlock = seqBlock;
        SourceLocation = sourceLocation;

        Children.Add(identifier);

        if (parameters != null) //If the sequence has parameters, add them as children.
        {
            foreach (var parameter in parameters) 
            {
                Children.Add(parameter);
            }
        }

        Children.Add(seqBlock);
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
    
    
    public override string ToString()
    {
        string parametersString = Parameters != null ? string.Join(", ", Parameters.Select(p => p.ToString())) : "None"; //If the sequence has parameters, add them to the string. Otherwise, add "None".
        return $"SequenceNode: Identifier: {Identifier}, Parameters: {parametersString}, SeqBlockNode: {SeqBlock}"; 
    }


}