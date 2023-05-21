using System.Text;

namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a sequence block node in the AST. A sequence block consists of a collection of sequence block parts.
public class SeqBlockNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.SeqBlock;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    public IList<StatementNode> Statements { get; } = new List<StatementNode>();
    public IList<AnimationNode> Animations { get; } = new List<AnimationNode>();

    public SeqBlockNode(
        IEnumerable<StatementNode> statements,
        IEnumerable<AnimationNode> animations,
        SourceLocation sourceLocation)
    {
        SourceLocation = sourceLocation;

        foreach (StatementNode statementNode in statements)
        {
            Statements.Add(statementNode);
            Children.Add(statementNode);
        }

        foreach (AnimationNode animationNode in animations)
        {
            Animations.Add(animationNode);
            Children.Add(animationNode);
        }
    }

    public IEnumerable<IASTNode> GetChildren()
    {
        return Children;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SeqBlockNode {");

        if (Children.Count > 0)
        {
            sb.Append(" Children: [");
            for (int i = 0; i < Children.Count; i++)
            {
                sb.Append(Children[i].ToString());
                if (i < Children.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append("]");
        }

        sb.Append(" }");
        return sb.ToString();
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        return visitor.Visit(this);
    }

}