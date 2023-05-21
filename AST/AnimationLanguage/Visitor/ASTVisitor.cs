using AnimationLanguage.ASTCommon;
using AnimationLanguage.ASTNodes;

public abstract class ASTVisitor<T> where T : IASTNode
{
    public abstract T? Visit(AnimationNode node);
    public abstract T? Visit(ArgumentNode node);
    public abstract T? Visit(AssignmentNode node);
    public abstract T? Visit(BlockNode node);
    public abstract T? Visit(CommandNode node);
    public abstract T? Visit(ConditionNode node);
    public abstract T? Visit(ElseIfNode node);
    public abstract T? Visit(ElseNode node);
    public abstract T? Visit(ExpressionNode node);
    public abstract T? Visit(ForLoopNode node);
    public abstract T? Visit(FrameDefNode node);
    public abstract T? Visit(FunctionCallNode node);
    public abstract T? Visit(FunctionDeclarationNode node);
    public abstract T? Visit(GroupingElementsNode node);
    public abstract T? Visit(IdentifierGroupingNode node);
    public abstract T? Visit(IfStatementNode node);
    public abstract T? Visit(IdentifierNode node);
    public abstract T? Visit(KeyValuePairNode node);
    public abstract T? Visit(IntegerLiteralNode node);
    public abstract T? Visit(FloatLiteralNode node);
    public abstract T? Visit(StringLiteralNode node);
    public abstract T? Visit(BooleanLiteralNode node);
    public abstract T? Visit(OperatorNode node);
    public abstract T? Visit(ParameterNode node);
    public abstract T? Visit(CallParameterNode node);
    public abstract T? Visit(ProgramNode node);
    public abstract T? Visit(PrototypeNode node);
    public abstract T? Visit(ReturnNode node);
    public abstract T? Visit(SeqBlockNode node);
    public abstract T? Visit(SeqBlockPartNode node);
    public abstract T? Visit(SequenceCallNode node);
    public abstract T? Visit(SequenceNode node);
    public abstract T? Visit(SetupNode node);
    public abstract T? Visit(ShapeInitNode node);
    public abstract T? Visit(PolygonNode node);
    public abstract T? Visit(CircleNode node);
    public abstract T? Visit(StatementNode node);
    public abstract T? Visit(TimelineBlockNode node);
    public abstract T? Visit(TransitionNode node);
    public abstract T? Visit(TupleNode node);
    public abstract T? Visit(TypeNode node);
    public abstract T? Visit(UnaryOperationNode node);
    public abstract T? Visit(WhileLoopNode node);
    public abstract T? Visit(NodeList<IASTNode> node);


    protected T? VisitChildren(IASTNode node)
    {
        T? result = default(T);

        foreach (var child in node.GetChildren())
        {
            result = child.Accept(this);
        }

        return result;
    }
}
