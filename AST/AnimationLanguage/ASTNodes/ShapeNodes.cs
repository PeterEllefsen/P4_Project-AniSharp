namespace AnimationLanguage.ASTNodes;
using ASTCommon;
using System.Collections.Generic;

public class PolygonNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Polygon;
    public IList<IASTNode> Children { get; } = new List<IASTNode>(); //The children of this node are the arguments used for drawing the polygon.
    
    public IDictionary<string, IASTNode> Args { get; set; } //The arguments used for drawing the polygon.

    public PolygonNode(IDictionary<string, IASTNode> args, SourceLocation sourceLocation)
    {
        Args = args;
        SourceLocation = sourceLocation;

        foreach (var arg in Args)
        {
            Children.Add(arg.Value); //Add all arguments as children.
        }
    }
}

public class CircleNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Circle;
    public IList<IASTNode> Children { get; } = new List<IASTNode>(); //The children of this node are the arguments used for drawing the circle.

    public IDictionary<string, IASTNode> Args { get; set; } //The arguments used for drawing the circle.

    public CircleNode(IDictionary<string, IASTNode> args, SourceLocation sourceLocation)
    {
        Args = args;
        SourceLocation = sourceLocation;

        foreach (var arg in Args)
        {
            Children.Add(arg.Value); //Add all arguments as children.
        }
    }
}
