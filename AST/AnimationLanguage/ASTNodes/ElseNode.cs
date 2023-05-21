namespace AnimationLanguage.ASTNodes;
using ASTCommon;

public class ElseNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Else;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    //There is no condition for an else block, so there is no Condition property.
    public BlockNode ElseBlock { get; set; } // Represents the block of code that is executed. This block contains a collection of statements.

    public ElseNode(BlockNode elseBlock, SourceLocation sourceLocation)
    {
        ElseBlock = elseBlock;
        SourceLocation = sourceLocation;

        Children.Add(elseBlock);
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }
    
    
    public override string ToString()
    {
        return $"ElseNode: {ElseBlock}";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }
}