using System.Reflection;
using System.Threading.Channels;
using AnimationLanguage.ASTNodes;

namespace AnimationLanguage.Visitor;
using ASTCommon;


public class TypeCheckingVisitor : ASTVisitor<IASTNode>
{
    private readonly ScopedSymbolTable _symbolTable;
    
    //This is a set of keywords that are predefined in the language. These are not allowed to be used as variable names.
    private static readonly HashSet<string> PredefinedKeywords = new HashSet<string> { "sceneWidth", "sceneHeight", "framerate", "backgroundColor"}; // Add more predefined keywords as needed

    public TypeCheckingVisitor(ScopedSymbolTable symbolTable)
    {
        _symbolTable = symbolTable; //The symbol table defined in the Program.cs file will be stoed in this property
    }

    public override IASTNode Visit(ProgramNode node)
{
    Console.WriteLine("Type checking program node");

    IList<PrototypeNode> decoratedPrototypes = new List<PrototypeNode>();
    SetupNode? decoratedSetup = null;
    IList<FunctionDeclarationNode> decoratedFunctionDeclarations = new List<FunctionDeclarationNode>();
    IList<SequenceNode> decoratedSequences = new List<SequenceNode>();
    TimelineBlockNode? decoratedTimeline = null;

    // No need to enter a new scope for prototypes as they are in the global scope
    foreach (IASTNode child in node.Children)
    {
        if (child is PrototypeNode prototypeNode)
        {
            decoratedPrototypes.Add((PrototypeNode)Visit(prototypeNode) ?? throw new InvalidOperationException("Failed to create a decorated prototype node."));
        }
        else if (child is SetupNode setupNode)
        {
            decoratedSetup = (SetupNode)Visit(setupNode) ?? throw new InvalidOperationException("Failed to create a decorated setup node.");
        }
        else if (child is FunctionDeclarationNode functionDefinitionNode)
        {
            decoratedFunctionDeclarations.Add((FunctionDeclarationNode)Visit(functionDefinitionNode) ?? throw new InvalidOperationException("Failed to create a decorated function declaration node."));
        }
        else if (child is SequenceNode sequenceNode)
        {
            decoratedSequences.Add((SequenceNode)Visit(sequenceNode) ?? throw new InvalidOperationException("Failed to create a decorated sequence node."));
        }
        else if (child is TimelineBlockNode timelineBlockNode)
        {
            decoratedTimeline = (TimelineBlockNode)(Visit(timelineBlockNode) ?? throw new InvalidOperationException("Failed to create a decorated timeline block node."));
        }
    }

    Console.WriteLine("Type checked program node");
    return new ProgramNode(decoratedPrototypes, decoratedSetup, decoratedFunctionDeclarations, decoratedSequences, decoratedTimeline, node.SourceLocation);
}



    
    public override SetupNode Visit(SetupNode node)
    {
        Console.WriteLine("Type checking setup node");

        // Visit the grouping elements node and get the decorated GroupingElementsNode.
        GroupingElementsNode decoratedGroupingElements = (GroupingElementsNode)Visit(node.GroupingElements) ?? throw new InvalidOperationException("Failed to create a decorated grouping elements node.");

        // Create a decorated SetupNode with the decorated GroupingElementsNode and set its SourceLocation.
        SetupNode decoratedSetupNode = new SetupNode(decoratedGroupingElements, node.SourceLocation);

        Console.WriteLine("Type checked setup node");
        return decoratedSetupNode;
    }


    public override GroupingElementsNode Visit(GroupingElementsNode node)
    {
        Console.WriteLine("Type checking grouping elements node");

        // Create lists to hold decorated children.
        List<ExpressionNode> decoratedExpressions = new List<ExpressionNode>();
        List<IdentifierNode> decoratedIdentifiers = new List<IdentifierNode>();
        List<KeyValuePairNode> decoratedKeyValuePairs = new List<KeyValuePairNode>();

        // Visit child nodes (expressions, identifiers, and key-value pairs) and add them to the decoratedChildren list.
        foreach (IASTNode child in node.Children)
        {
            if (child is ExpressionNode expressionNode)
            {
                decoratedExpressions.Add((ExpressionNode?)Visit(expressionNode) ?? throw new InvalidOperationException("Failed to create a decorated expression node."));
            }
            else if (child is IdentifierNode identifierNode)
            {
                decoratedIdentifiers.Add((IdentifierNode?)Visit(identifierNode) ?? throw new InvalidOperationException("Failed to create a decorated identifier node."));
            }
            else if (child is KeyValuePairNode keyValuePairNode)
            {
                decoratedKeyValuePairs.Add((KeyValuePairNode?)Visit(keyValuePairNode) ?? throw new InvalidOperationException("Failed to create a decorated key-value pair node."));
            }
        }

        // Create a new decorated GroupingElementsNode with the decorated children and set its SourceLocation.
        GroupingElementsNode decoratedGroupingElementsNode = new GroupingElementsNode(decoratedExpressions, decoratedIdentifiers, decoratedKeyValuePairs, node.SourceLocation);

        Console.WriteLine("Type checked grouping elements node");
        return decoratedGroupingElementsNode;
    }


    
    public override IASTNode? Visit(KeyValuePairNode node)
    {
        Console.WriteLine("Type checking key-value pair node");

        IdentifierNode decoratedKey = (IdentifierNode?)Visit(node.Key) ?? throw new InvalidOperationException("Failed to create a decorated key identifier node.");

        IASTNode valueNode;
        if (node.Value is ExpressionNode expressionNode)
        {
            valueNode = (ExpressionNode?)Visit(expressionNode) ?? throw new InvalidOperationException("Failed to create a decorated value node.");
        }
        else if (node.Value is FunctionCallNode functionCallNode)
        {
            valueNode = (FunctionCallNode?)Visit(functionCallNode) ?? throw new InvalidOperationException("Failed to create a decorated value node.");
        }
        else
        {
            throw new InvalidOperationException($"Expected an ExpressionNode or FunctionCallNode but got {node.Value.GetType().Name}");
        }

        KeyValuePairNode decoratedKeyValuePairNode = new KeyValuePairNode(decoratedKey.Name, valueNode, node.SourceLocation);

        Console.WriteLine("Type checked key-value pair node");
        return decoratedKeyValuePairNode;
    }

    
    
    
    public override IASTNode? Visit(IdentifierNode node)
    {
        Console.WriteLine("Type checking identifier node");

        if (PredefinedKeywords.Contains(node.Name))
        {
            // If the identifier is a predefined keyword, return the node without further checking
            return node;
        }

        if (!_symbolTable.IsDefined(node.Name))
        {
            throw new InvalidOperationException($"Identifier '{node.Name}' is not defined.");
        }

        IdentifierNode decoratedIdentifierNode = new IdentifierNode(node.Name, node.SourceLocation);
        Console.WriteLine("Type checked identifier node");
        return decoratedIdentifierNode;
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
    
    
    
    public override IASTNode Visit(ArgumentNode node)
    {
        Console.WriteLine("Type checking argument node");

        // Type check the value of the argument
        IASTNode decoratedValue;
        if (node.Value is ExpressionNode expressionNode)
        {
            decoratedValue = Visit(expressionNode) ?? throw new InvalidOperationException("Failed to create a decorated expression node.");
        }
        else if (node.Value is IdentifierNode identifierNode)
        {
            decoratedValue = Visit(identifierNode) ?? throw new InvalidOperationException("Failed to create a decorated identifier node.");
        }
        else if (node.Value is TupleNode tupleNode)
        {
            decoratedValue = Visit(tupleNode) ?? throw new InvalidOperationException("Failed to create a decorated tuple node.");
        }
        else
        {
            throw new InvalidOperationException($"Unrecognized value type for argument '{node.Name}' at {node.SourceLocation}");
        }

        ArgumentNode decoratedArgumentNode = new ArgumentNode(node.Name, decoratedValue, node.SourceLocation);

        Console.WriteLine("Type checked argument node");
        return decoratedArgumentNode;
    }

    
    
    public override IASTNode Visit(FunctionDeclarationNode node)
    {
        Console.WriteLine("Type checking function declaration node");

        // Visit ReturnType and Identifier nodes.
        TypeNode decoratedReturnType = (TypeNode?)Visit(node.ReturnType) ?? throw new InvalidOperationException("Failed to create a decorated return type node.");
        IdentifierNode decoratedIdentifier = (IdentifierNode?)Visit(node.Identifier) ?? throw new InvalidOperationException("Failed to create a decorated identifier node.");

        // Visit Parameter nodes and create a list of decorated parameters.
        List<ParameterNode> decoratedParameters = new List<ParameterNode>();
        foreach (ParameterNode parameterNode in node.Parameters)
        {
            decoratedParameters.Add((ParameterNode)Visit(parameterNode) ?? throw new InvalidOperationException("Failed to create a decorated parameter node."));
        }

        // Enter the scope for the function declaration.
        _symbolTable.EnterScope();

        // Visit the Block node.
        BlockNode decoratedBlock = (BlockNode?)Visit(node.Block) ?? throw new InvalidOperationException("Failed to create a decorated block node.");

        // Exit the scope for the function declaration.
        _symbolTable.ExitScope();

        // Create a new decorated FunctionDeclarationNode with the decorated nodes and set its SourceLocation.
        FunctionDeclarationNode decoratedFunctionDeclarationNode = new FunctionDeclarationNode(decoratedReturnType, decoratedIdentifier, decoratedParameters, decoratedBlock, node.SourceLocation);

        Console.WriteLine("Type checked function declaration node");
        return decoratedFunctionDeclarationNode;
    }

    
    public override IASTNode? Visit(FunctionCallNode node)
    {
        List<IASTNode> decoratedArguments = new List<IASTNode>();

        foreach (IASTNode argument in node.Arguments)
        {
            if (argument is ArgumentNode argumentNode) // Change this to integernode and floatnode and so on
            {
                IASTNode? decoratedArgument = Visit(argumentNode);
                if (decoratedArgument == null)
                {
                    throw new InvalidOperationException($"Failed to create a decorated argument node for argument: {argument}");
                }
                decoratedArguments.Add(decoratedArgument);
            }
            else
            {
                throw new InvalidOperationException($"Invalid argument type: {argument.GetType().Name}. Expected: ArgumentNode");
            }
        }

        // Use the GetFunctionReturnType method to get the return type of the function call.
        TypeNode.TypeKind returnType = GetFunctionReturnType(node.FunctionIdentifier);

        TypeNode typeNode = new TypeNode(returnType, node.SourceLocation);
        FunctionCallNode decoratedFunctionCallNode = new FunctionCallNode(node.FunctionIdentifier, decoratedArguments, node.SourceLocation);
        decoratedFunctionCallNode.Type = typeNode;

        return decoratedFunctionCallNode;
    }



    
    public override IASTNode Visit(ParameterNode node)
    {
        Console.WriteLine($"Type checking parameter node: Name='{node.Name}', DataType='{node.DataType}'");
        
        ParameterNode decoratedNode = new ParameterNode(node.DataType, node.Name, node.SourceLocation);

        Console.WriteLine($"Type checked parameter node: Name='{decoratedNode.Name}', DataType='{decoratedNode.DataType}'");

        return decoratedNode;
    }

    
    public override IASTNode? Visit(CallParameterNode node)
    {
        Console.WriteLine("Type checking call parameter node");
        return VisitChildren(node);
    }
    
    public override IASTNode Visit(PrototypeNode node)
    {
        Console.WriteLine("Type checking prototype node");
        string functionName = node.FunctionName;
        DataType returnType = node.ReturnType;
        IList<ParameterNode> parameters = node.Parameters;

        // Check if the function is already in the symbol table.
        Symbol? existingSymbol = _symbolTable.Lookup(functionName); //Check if the prototype has already been defined

        if (existingSymbol != null)
        {
            throw new InvalidOperationException($"Prototype of function '{functionName}' already exists.");
        }
        else //If not, then add it to the symbol table
        {
            _symbolTable.AddFunction(functionName, returnType.ToString());
        }

        // Visit all of the parameters in the prototype and create a list of decorated parameters.
        List<ParameterNode> decoratedParameters = new List<ParameterNode>();
        foreach (ParameterNode parameter in parameters)
        {
            Console.WriteLine($"Visiting parameter '{parameter.Name}'");
            IASTNode? visitedParameter = Visit(parameter);
            if (visitedParameter != null)
            {
                decoratedParameters.Add((ParameterNode)visitedParameter);
            }
        }

        // Create a decorated PrototypeNode
        PrototypeNode decoratedNode = new PrototypeNode(returnType, functionName, decoratedParameters, node.SourceLocation);

        Console.WriteLine($"Type checked prototype node: FunctionName='{functionName}', ReturnType='{returnType}', ParametersCount='{decoratedParameters.Count}'");

        return decoratedNode;
    }
    


    public override IASTNode? Visit(StatementNode node)
    {
        Console.WriteLine("Type checking statement node");

        IASTNode? decoratedNode = null;

        if (node.Assignment != null)
        {
            decoratedNode = Visit(node.Assignment);
        }
        else if (node.FunctionCall != null)
        {
            decoratedNode = Visit(node.FunctionCall);
        }
        else if (node.IfStatement != null)
        {
            decoratedNode = Visit(node.IfStatement);
        }
        else if (node.ForStatement != null)
        {
            decoratedNode = Visit(node.ForStatement);
        }
        else if (node.WhileStatement != null)
        {
            decoratedNode = Visit(node.WhileStatement);
        }
        else if (node.ReturnStatement != null)
        {
            decoratedNode = Visit(node.ReturnStatement);
        }
        else
        {
            throw new InvalidOperationException($"Unrecognized statement at {node.SourceLocation}");
        }

        if (decoratedNode == null)
        {
            throw new InvalidOperationException("Failed to create a decorated statement node.");
        }

        Console.WriteLine("Type checked statement node");
        return decoratedNode;
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

        // Type check the statements in the block
        IList<StatementNode> decoratedStatementNodes = new List<StatementNode>();
        foreach (StatementNode statementNode in node.Statements)
        {
            StatementNode decoratedStatementNode = (StatementNode?)Visit(statementNode) ?? throw new InvalidOperationException("Failed to create a decorated statement node.");
            decoratedStatementNodes.Add(decoratedStatementNode);
        }

        // Type check the return node in the block
        ReturnNode? decoratedReturnNode = null;
        if (node.ReturnNode != null)
        {
            decoratedReturnNode = (ReturnNode?)Visit(node.ReturnNode) ?? throw new InvalidOperationException("Failed to create a decorated return node.");
        }

        // Create a new decorated BlockNode with the type checked statements and return node
        BlockNode decoratedBlockNode = new BlockNode(decoratedStatementNodes, decoratedReturnNode, node.SourceLocation);

        Console.WriteLine("Type checked block node");
        return decoratedBlockNode;
    }




    public override IASTNode? Visit(SeqBlockNode node)
    {
        Console.WriteLine("Type checking sequence block node");

        List<StatementNode> decoratedStatements = new List<StatementNode>();
        List<AnimationNode> decoratedAnimations = new List<AnimationNode>();

        foreach (var statementNode in node.Statements)
        {
            StatementNode decoratedStatementNode = (StatementNode?)Visit(statementNode) ?? throw new InvalidOperationException("Failed to create a decorated statement node.");
            decoratedStatements.Add(decoratedStatementNode);
        }

        foreach (var animationNode in node.Animations)
        {
            AnimationNode decoratedAnimationNode = (AnimationNode?)Visit(animationNode) ?? throw new InvalidOperationException("Failed to create a decorated animation node.");
            decoratedAnimations.Add(decoratedAnimationNode);
        }

        SeqBlockNode decoratedSeqBlockNode = new SeqBlockNode(decoratedStatements, decoratedAnimations, node.SourceLocation);

        Console.WriteLine("Type checked sequence block node");
        return decoratedSeqBlockNode;
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

        IASTNode? decoratedLeftOperand = null;
        IASTNode? decoratedRightOperand = null;
        OperatorNode? decoratedOperatorNode = null;

        if (node.LeftOperand != null)
        {
            decoratedLeftOperand = VisitNodeBasedOnExpressionType(node.ExpressionType, node.LeftOperand) ?? throw new InvalidOperationException("Failed to create a decorated left operand node.");
        }

        if (node.RightOperand != null)
        {
            decoratedRightOperand = VisitNodeBasedOnExpressionType(node.ExpressionType, node.RightOperand) ?? throw new InvalidOperationException("Failed to create a decorated right operand node.");
        }

        if (node.OperatorNode != null)
        {
            decoratedOperatorNode = (OperatorNode?)Visit(node.OperatorNode) ?? throw new InvalidOperationException("Failed to create a decorated operator node.");
        }

        ExpressionNode decoratedExpressionNode = new ExpressionNode(
            node.ExpressionType,
            decoratedLeftOperand,
            decoratedRightOperand,
            decoratedOperatorNode,
            node.SourceLocation
        );

        Console.WriteLine("Type checked expression node");
        return decoratedExpressionNode;
    }

    
    public override IASTNode? Visit(FrameDefNode node)
    {
        Console.WriteLine("Type checking frame definition node");

        // Visit and type check the sequence call in the frame definition node
        SequenceCallNode decoratedSequenceCallNode = (SequenceCallNode)Visit(node.SequenceCall) ?? throw new InvalidOperationException("Failed to create a decorated sequence call node.");

        // Create a new decorated FrameDefNode with the type checked sequence call
        FrameDefNode decoratedFrameDefNode = new FrameDefNode(node.FrameTime, decoratedSequenceCallNode, node.SourceLocation);

        Console.WriteLine("Type checked frame definition node");
        return decoratedFrameDefNode;
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

        IASTNode? decoratedReturnValue = null;

        if (node.ReturnValue != null)
        {
            if (node.ReturnValue is ExpressionNode expressionNode)
            {
                decoratedReturnValue = (ExpressionNode?)Visit(expressionNode) ?? throw new InvalidOperationException("Failed to create a decorated return value node.");
            }
            else if (node.ReturnValue is FunctionCallNode functionCallNode)
            {
                decoratedReturnValue = (FunctionCallNode?)Visit(functionCallNode) ?? throw new InvalidOperationException("Failed to create a decorated return value node.");
            }
            else
            {
                throw new InvalidOperationException($"Expected an ExpressionNode or FunctionCallNode but got {node.ReturnValue.GetType().Name}");
            }
        }

        ReturnNode decoratedReturnNode = new ReturnNode(decoratedReturnValue, node.SourceLocation);

        Console.WriteLine("Type checked return node");
        return decoratedReturnNode;
    }


    
    
    
    public override IASTNode? Visit(SeqBlockPartNode node)
    {
        Console.WriteLine("Type checking seqblockpart node");
        return VisitChildren(node);
    }
    
    public override IASTNode Visit(SequenceCallNode node)
    {
        Console.WriteLine("Type checking sequence call node");

        // Check if the sequence exists in the symbol table
        Symbol? sequenceSymbol = _symbolTable.Lookup(node.Name.ToString());
        if (sequenceSymbol == null || sequenceSymbol.Type != "seq")
        {
            throw new InvalidOperationException($"Sequence '{node.Name.ToString()}' is not defined.");
        }

        // Type check the arguments of the sequence call
        List<ExpressionNode> decoratedArguments = new List<ExpressionNode>();
        foreach (ExpressionNode argument in node.Arguments)
        {
            ExpressionNode decoratedArgument = (ExpressionNode?)Visit(argument) ?? throw new InvalidOperationException("Failed to create a decorated argument node.");
            decoratedArguments.Add(decoratedArgument);
        }

        SequenceCallNode decoratedSequenceCallNode = new SequenceCallNode(node.Name, decoratedArguments, node.SourceLocation);

        Console.WriteLine("Type checked sequence call node");
        return decoratedSequenceCallNode;
    }

    
    public override IASTNode Visit(SequenceNode node)
    {
        Console.WriteLine("Type checking sequence node");

        // Visit Identifier node.
        IdentifierNode decoratedName = (IdentifierNode?)Visit(node.Name) ?? throw new InvalidOperationException("Failed to create a decorated identifier node.");

        // Visit Parameter nodes and create a list of decorated parameters.
        List<ParameterNode> decoratedParameters = new List<ParameterNode>();
        foreach (ParameterNode parameterNode in node.Parameters)
        {
            decoratedParameters.Add((ParameterNode)Visit(parameterNode) ?? throw new InvalidOperationException("Failed to create a decorated parameter node."));
        }

        // Enter the scope for the sequence.
        _symbolTable.EnterScope();

        // Visit the SeqBlockNode.
        SeqBlockNode decoratedBlock = (SeqBlockNode?)Visit(node.Block) ?? throw new InvalidOperationException("Failed to create a decorated sequence block node.");

        // Exit the scope for the sequence.
        _symbolTable.ExitScope();

        // Create a new decorated SequenceNode with the decorated nodes and set its SourceLocation.
        SequenceNode decoratedSequenceNode = new SequenceNode(decoratedName, decoratedParameters, decoratedBlock, node.SourceLocation);

        Console.WriteLine("Type checked sequence node");
        return decoratedSequenceNode;
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
        Console.WriteLine("Type checking timeline block node");

        // Visit and type check each frame definition in the timeline block node
        List<FrameDefNode> decoratedFrameDefinitions = new List<FrameDefNode>();

        foreach (var frameDefNode in node.FrameDefinitions)
        {
            FrameDefNode? decoratedFrameDefNode = (FrameDefNode?)Visit(frameDefNode) ?? throw new InvalidOperationException("Failed to create a decorated frame definition node.");
            decoratedFrameDefinitions.Add(decoratedFrameDefNode);
        }

        // Create a new decorated TimelineBlockNode with the type checked frame definitions
        TimelineBlockNode decoratedTimelineBlockNode = new TimelineBlockNode(node.StartTime, node.EndTime, decoratedFrameDefinitions, node.SourceLocation);

        Console.WriteLine("Type checked timeline block node");
        return decoratedTimelineBlockNode;
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
    
    
    //-----------------Help methods-----------------//
    private IASTNode? VisitNodeBasedOnExpressionType(ExpressionNodeType expressionType, IASTNode node)
    {
        switch (expressionType)
        {
            case ExpressionNodeType.Binary:
            case ExpressionNodeType.Unary:
            case ExpressionNodeType.Term:
                return Visit((ExpressionNode)node);
            case ExpressionNodeType.Literal:
                if (node is IntegerLiteralNode)
                {
                    return Visit((IntegerLiteralNode)node);
                }
                else if (node is FloatLiteralNode)
                {
                    return Visit((FloatLiteralNode)node);
                }
                else if (node is StringLiteralNode)
                {
                    return Visit((StringLiteralNode)node);
                }
                else if (node is BooleanLiteralNode)
                {
                    return Visit((BooleanLiteralNode)node);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported literal type: {node.GetType().Name}");
                }
            case ExpressionNodeType.Identifier:
                return Visit((IdentifierNode)node);
            case ExpressionNodeType.FunctionCall:
                return Visit((FunctionCallNode)node);
            case ExpressionNodeType.ShapeInit:
                return Visit((ShapeInitNode)node);
            default:
                throw new InvalidOperationException($"Unsupported expression type: {expressionType}");
        }
    }
    
    
    private TypeNode.TypeKind GetFunctionReturnType(IdentifierNode functionIdentifier)
    {
        Symbol? symbol = _symbolTable.Lookup(functionIdentifier.Name);
        if (symbol == null)
        {
            throw new InvalidOperationException($"Function '{functionIdentifier.Name}' is not defined.");
        }

        return ConvertStringTypeToTypeKind(symbol.Type);
    }
    
    
    private TypeNode.TypeKind ConvertStringTypeToTypeKind(string type)
    {
        type = type.ToLower();
        TypeNode.TypeKind typeKind;

        switch (type)
        {
            case "int":
                typeKind = TypeNode.TypeKind.Int;
                break;
            case "float":
                typeKind = TypeNode.TypeKind.Float;
                break;
            case "string":
                typeKind = TypeNode.TypeKind.String;
                break;
            case "bool":
                typeKind = TypeNode.TypeKind.Bool;
                break;
            case "circle":
                typeKind = TypeNode.TypeKind.Circle;
                break;
            case "polygon":
                typeKind = TypeNode.TypeKind.Polygon;
                break;
            case "group":
                typeKind = TypeNode.TypeKind.Group;
                break;
            default:
                typeKind = TypeNode.TypeKind.None;
                break;
        }

        return typeKind;
    }


}

