using System.Reflection;
using System.Threading.Channels;
using AnimationLanguage.ASTNodes;

namespace AnimationLanguage.Visitor;
using ASTCommon;


public class TypeCheckingVisitor : ASTVisitor<IASTNode>
{
    private readonly SymbolTable _symbolTable;

    public TypeCheckingVisitor(SymbolTable symbolTable)
    {
        _symbolTable = symbolTable;
    }

    public override IASTNode? Visit(ProgramNode node)
    {
        Console.WriteLine("Type checking program node: ");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(SetupNode node)
    {
        Console.WriteLine("Type checking setup node: ");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(GroupingElementsNode node)
    {
        Console.WriteLine("Type checking groupingelements node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(KeyValuePairNode node)
    {
        Console.WriteLine("Type checking key value pair node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(IdentifierNode node)
    {
        Console.WriteLine("Type checking identifier node");
        return VisitChildren(node);
    }

    public override IASTNode? Visit(AssignmentNode node)
    {
        Console.WriteLine("Type checking assignment node:");
        Console.WriteLine("test");
        IdentifierNode identifierNode = node.Identifier;
        ExpressionNode expression = node.Expression;
        string variableName = identifierNode.Name;
    
        // Retrieve type information from the expression node.
        string expressionType = expression.NodeType.ToString(); // Assuming you've added a 'Type' property to the ExpressionNode class.
        Console.WriteLine("Identifier: " + variableName + " Expression type: " + expressionType);
        // If the assignment node has a type, verify that it matches the expression type.
        if (node.NodeType != null)
        {
            if (node.NodeType.ToString() != expressionType)
            {
                throw new InvalidOperationException($"Type mismatch: Cannot assign {expressionType} to {node.NodeType.ToString()} for variable '{variableName}'.");
            }
        }
        else
        {
            throw new ArgumentNullException($"Type of variable '{variableName}' is null.");
        }

        // Check if the variable is already in the symbol table.
        Symbol? existingSymbol = _symbolTable.Lookup(variableName);

        if (existingSymbol != null)
        {
            if (existingSymbol.Type != node.NodeType.ToString())
            {
                throw new InvalidOperationException($"Type mismatch: Cannot assign {node.NodeType.ToString()} to existing variable '{variableName}' of type {existingSymbol.Type}.");
            }
        }
        else
        {
            // If the variable is not in the symbol table, add it.
            _symbolTable.AddVariable(variableName, node.NodeType.ToString());
        }

        Console.WriteLine($"Type checked assignment node: Identifier='{variableName}', Type='{node.NodeType.ToString()}', ExpressionType='{expressionType}'");

        return VisitChildren(node);
    }

    
    public override IASTNode? Visit(OperatorNode node)
    {
        Console.WriteLine("Type checking operator node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(IdentifierGroupingNode node)
    {
        Console.WriteLine("Type checking identifier grouping node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(ArgumentNode node)
    {
        Console.WriteLine("Type checking argument node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(FunctionDeclarationNode node)
    {
        Console.WriteLine("Type checking function declaration node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(FunctionCallNode node)
    {
        Console.WriteLine("Type checking function call node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(ParameterNode node)
    {
        Console.WriteLine("Type checking parameter node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(CallParameterNode node)
    {
        Console.WriteLine("Type checking call parameter node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(PrototypeNode node)
    {
        Console.WriteLine("Type checking prototype node");
        string functionName = node.FunctionName;
        DataType returnType = node.ReturnType;
        IList<ParameterNode> parameters = node.Parameters;

        // Check if the function is already in the symbol table.
        Symbol? existingSymbol = _symbolTable.Lookup(functionName);

        if (existingSymbol != null)
        {
            throw new InvalidOperationException($"Function '{functionName}' already exists.");
        }
        else
        {
            // If the function is not in the symbol table, add it.
            _symbolTable.AddFunction(functionName, returnType.ToString());
        }

        // Visit the parameters in the prototype.
        foreach (ParameterNode parameter in parameters)
        {
            Visit(parameter);
        }

        Console.WriteLine($"Type checked prototype node: FunctionName='{functionName}', ReturnType='{returnType}', ParametersCount='{parameters.Count}'");

        return VisitChildren(node);
    }

    
    public override IASTNode? Visit(StatementNode node)
    {
        Console.WriteLine("Type checking statement node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(IfStatementNode node)
    {
        Console.WriteLine("Type checking if statement node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(ElseIfNode node)
    {
        Console.WriteLine("Type checking else if node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(ElseNode node)
    {
        Console.WriteLine("Type checking else node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(WhileLoopNode node)
    {
        Console.WriteLine("Type checking while statement node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(ForLoopNode node)
    {
        Console.WriteLine("Type checking for loop node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(BlockNode node)
    {
        Console.WriteLine("Type checking block node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(SeqBlockNode node)
    {
        Console.WriteLine("Type checking SeqBlock node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(AnimationNode node)
    {
        Console.WriteLine("Type checking declaration node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(CommandNode node)
    {
        Console.WriteLine("Type checking command node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(ConditionNode node)
    {
        Console.WriteLine("Type checking condition node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(ExpressionNode node)
    {
        Console.WriteLine("Type checking expression node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(FrameDefNode node)
    {
        Console.WriteLine("Type checking framedef node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(IntegerLiteralNode node)
    {
        Console.WriteLine("Type checking integer literal node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(FloatLiteralNode node)
    {
        Console.WriteLine("Type checking float literal node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(StringLiteralNode node)
    {
        Console.WriteLine("Type checking string literal node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(BooleanLiteralNode node)
    {
        Console.WriteLine("Type checking boolean literal node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(ReturnNode node)
    {
        Console.WriteLine("Type checking return node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(SeqBlockPartNode node)
    {
        Console.WriteLine("Type checking seqblockpart node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(SequenceCallNode node)
    {
        Console.WriteLine("Type checking sequence node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(SequenceNode node)
    {
        Console.WriteLine("Type checking sequence node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(PolygonNode node)
    {
        Console.WriteLine("Type checking polygon node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(CircleNode node)
    {
        Console.WriteLine("Type checking circle node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(TimelineBlockNode node)
    {
        Console.WriteLine("Type checking TimelineBlock node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(TransitionNode node)
    {
        Console.WriteLine("Type checking Transition node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(TupleNode node)
    {
        Console.WriteLine("Type checking Tuple node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(TypeNode node)
    {
        Console.WriteLine("Type checking Type node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(UnaryOperationNode node)
    {
        Console.WriteLine("Type checking UnaryOperation node");
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(NodeList<IASTNode> node)
    {
        Console.WriteLine("Type checking VariableDeclaration node");
        return VisitChildren(node);
    }
}

