namespace AnimationLanguage.ASTNodes;
using ASTCommon;


//This class represents the root node of the AST.
public class ProgramNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Program;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IList<PrototypeNode> Prototypes { get; } = new List<PrototypeNode>(); //Represents the prototypes of the program. They appear before the setup block, but are not required if there is no functions.
    public SetupNode? Setup { get; set; } //Represents the setup block of the program.
    public IList<FunctionDeclarationNode> FunctionDeclarations { get; } = new List<FunctionDeclarationNode>(); //Represents the function declarations of the program.
    public IList<SequenceNode> Sequences { get; } = new List<SequenceNode>(); //Represents the sequences of the program.
    public TimelineBlockNode? Timeline { get; set; } //Represents the timeline of the program.

    public ProgramNode(
        IEnumerable<PrototypeNode> prototypes,
        SetupNode? setup,
        IEnumerable<FunctionDeclarationNode> functionDeclarations,
        IEnumerable<SequenceNode> sequences,
        TimelineBlockNode? timeline,
        SourceLocation sourceLocation)
    {
        Prototypes = new List<PrototypeNode>(prototypes); //Add all prototypes to the list.
        Setup = setup; //Add the setup block to the list.
        FunctionDeclarations = new List<FunctionDeclarationNode>(functionDeclarations); //Add all function declarations to the list.
        Sequences = new List<SequenceNode>(sequences); //Add all sequences to the list.
        Timeline = timeline; //Add the timeline to the list.
        SourceLocation = sourceLocation; //Set the source location of the node.

        foreach (PrototypeNode prototypeNode in prototypes) 
        {
            Children.Add(prototypeNode); //Add all prototypes as children. If there is none, this will not add anything.
        }
        
        if (setup != null)
        {
            Children.Add(setup); //Add the setup block as a child if it exists.
        }

        foreach (FunctionDeclarationNode functionDeclarationNode in functionDeclarations)
        {
            Children.Add(functionDeclarationNode); //Add all function declarations as children.
        }

        foreach (SequenceNode sequenceNode in sequences)
        {
            Children.Add(sequenceNode); //Add all sequences as children.
        }
        
        if (timeline != null)
        {
            Children.Add(timeline); //Add the timeline as a child if it exists.
        }
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
    
    
    public override string ToString()
    {
        return $"ProgramNode: ({Prototypes.Count} prototypes, {FunctionDeclarations.Count} function declarations, {Sequences.Count} sequences)";
    }
}
