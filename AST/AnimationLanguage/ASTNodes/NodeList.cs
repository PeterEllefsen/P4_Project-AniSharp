using AnimationLanguage.ASTCommon;

//This class works as a collection of nodes
public class NodeList<T> : List<T>, IASTNode where T : IASTNode //It inherits from List<T> and implements IASTNode which means that it is a collection of nodes.
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.List;

    public NodeList(IEnumerable<T> nodes, SourceLocation sourceLocation) 
    {
        SourceLocation = sourceLocation;
        AddRange(nodes); //AddRange adds a collection of nodes to the list.
    }
    
    
    public IEnumerable<IASTNode> GetChildren()
    {
        return this.OfType<IASTNode>(); //OfType<IASTNode> returns elements of a collection that are of type IASTNode. It returns itself since it is a collection of nodes.
    }
    
    
    public override string ToString()
    {
        return $"NodeList: {Count} elements";
    }
    
    
    public T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode
    {
        T? result = default(T);

        foreach (IASTNode node in this)
        {
            T? tempResult = node.Accept(visitor);
            if (tempResult != null)
            {
                result = tempResult;
            }
        }

        return result;
    }

}