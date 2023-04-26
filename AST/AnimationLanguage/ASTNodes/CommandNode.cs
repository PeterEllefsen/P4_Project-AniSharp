﻿namespace AnimationLanguage.ASTNodes;
using ASTCommon;

// This class represents a command node in the AST. A command node is a function call with a specific syntax in the language.
public class CommandNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Command;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IdentifierNode Identifier { get; set; } // The identifier of the command node.
    public IList<IASTNode> Parameters { get; } = new List<IASTNode>(); // The parameters of the command node.

    public CommandNode(IdentifierNode identifier, IEnumerable<IASTNode> parameters, SourceLocation sourceLocation)
    {
        Identifier = identifier;
        SourceLocation = sourceLocation;
        Children.Add(identifier);

        foreach (IASTNode parameter in parameters)
        {
            Parameters.Add(parameter);
            Children.Add(parameter);
        }
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
}