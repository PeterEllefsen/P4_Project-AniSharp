using System.Reflection.Metadata;

namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a function declaration node in the AST.
public class FunctionDeclarationNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.FunctionDeclaration;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IdentifierNode Identifier { get; set; } // Represents the function's name.
    public IList<ParameterNode> Parameters { get; } = new List<ParameterNode>(); // Represents the function's parameters.
    public BlockNode Block { get; set; } // Represents the function's body.

    public FunctionDeclarationNode(
        IdentifierNode identifier,
        IEnumerable<ParameterNode> parameters,
        BlockNode block,
        SourceLocation sourceLocation)
    {
        Identifier = identifier;
        Block = block;
        SourceLocation = sourceLocation;

        Children.Add(identifier);

        foreach (ParameterNode parameterNode in parameters)
        {
            Parameters.Add(parameterNode);
            Children.Add(parameterNode);
        }

        Children.Add(block);
    }
}