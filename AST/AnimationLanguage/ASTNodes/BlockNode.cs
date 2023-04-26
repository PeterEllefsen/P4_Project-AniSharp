namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a block node in the AST. A block consists of a collection of statements.
public class BlockNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Block;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IList<StatementNode> Statements { get; } = new List<StatementNode>(); // Represents all of the statements within the block.

    public BlockNode(IEnumerable<StatementNode> statements, SourceLocation sourceLocation)
    {
        SourceLocation = sourceLocation;

        foreach (StatementNode statementNode in statements)
        {
            Statements.Add(statementNode);
            Children.Add(statementNode); // Add all statements as children.
        }
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
}