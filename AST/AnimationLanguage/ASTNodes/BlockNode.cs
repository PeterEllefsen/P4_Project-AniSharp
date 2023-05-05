namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a block node in the AST. A block consists of a collection of statements.
public class BlockNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Block;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IList<StatementNode> Statements { get; } = new List<StatementNode>(); // Represents all of the statements within the block.

    public ReturnNode? ReturnNode { get; set; } // Represents the return statement in the block, if present.

    public BlockNode(IEnumerable<StatementNode> statements, ReturnNode? returnNode, SourceLocation sourceLocation)
    {
        SourceLocation = sourceLocation;
        ReturnNode = returnNode;

        foreach (StatementNode statementNode in statements)
        {
            Statements.Add(statementNode);
            Children.Add(statementNode); // Add all statements as children.
        }

        if (returnNode != null)
        {
            Children.Add(returnNode); // Add the return statement as a child, if present.
        }
    }


    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }


    public override string ToString()
    {
        string statementsStr = string.Join(", ", Statements.Select(s => s.ToString()));
        string returnStr = ReturnNode != null ? $", {ReturnNode}" : "";
        return $"BlockNode: {statementsStr}{returnStr}";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}