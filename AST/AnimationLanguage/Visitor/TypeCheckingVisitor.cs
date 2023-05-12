using System.Linq.Expressions;
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

    Console.WriteLine("Type checked program node: ");
    return new ProgramNode(decoratedPrototypes, decoratedSetup, decoratedFunctionDeclarations, decoratedSequences, decoratedTimeline, node.SourceLocation);
}



    
    public override SetupNode Visit(SetupNode node)
    {
        Console.WriteLine("Type checking setup node");

        // Visit the grouping elements node and get the decorated GroupingElementsNode.
        GroupingElementsNode decoratedGroupingElements = (GroupingElementsNode)Visit(node.GroupingElements) ?? throw new InvalidOperationException("Failed to create a decorated grouping elements node.");

        // Create a decorated SetupNode with the decorated GroupingElementsNode and set its SourceLocation.
        SetupNode decoratedSetupNode = new SetupNode(decoratedGroupingElements, node.SourceLocation);

        Console.WriteLine("Type checked setup node: " + decoratedSetupNode);
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
    IdentifierNode identifierNode = node.Identifier;
    string variableName = identifierNode.Name;
    string variableType;

    // Check if the expression is an Identifier
    if (node.Expression.ExpressionType == ExpressionNodeType.Identifier)
    {
        IdentifierNode rightIdentifierNode = (IdentifierNode)node.Expression;
        string rightVariableName = rightIdentifierNode.Name;

        // Lookup the symbols for the left and right identifiers
        Symbol? leftSymbol = _symbolTable.Lookup(variableName);
        Symbol? rightSymbol = _symbolTable.Lookup(rightVariableName);

        Console.WriteLine("RIGHT VARIABLE NAME: " + rightVariableName);

        if (leftSymbol == null || rightSymbol == null)
        {
            throw new InvalidOperationException($"Undefined variable(s) in assignment: '{variableName}' or '{rightVariableName}'.");
        }

        // Check if the types match
        if (leftSymbol.Type != rightSymbol.Type)
        {
            throw new InvalidOperationException($"Type mismatch: Cannot assign {rightSymbol.Type} to {leftSymbol.Type} for variable '{variableName}'.");
        }

        variableType = leftSymbol.Type;

        Console.WriteLine($"Type checked assignment node: Identifier='{variableName}', Type='{variableType}', ExpressionType='{rightSymbol.Type}'");

        // Return a decorated AssignmentNode
        AssignmentNode decoratedAssignmentNode = new AssignmentNode(identifierNode, node.AssignmentOperator, rightIdentifierNode, node.SourceLocation);
        return decoratedAssignmentNode;
    }
    else
    {
        // Set the Value property of the ExpressionNode before calling Visit
        if (node.Expression.ExpressionType == ExpressionNodeType.Literal)
        {
            node.Expression.Value = node.Expression;
        }
        else
        {
            node.Expression.Value = node.Expression.RightOperand;
        }

        // Visit the ExpressionNode to ensure its decorated correctly
        ExpressionNode expression = (ExpressionNode?)Visit(node.Expression) ?? throw new InvalidOperationException("Failed to create a decorated expression node.");
        string expressionType = expression.Type?.ToString() ?? "Unknown";

        Symbol? existingSymbol = _symbolTable.Lookup(variableName);
        if (existingSymbol != null)
        {
            Console.WriteLine("THIS IS THE SYMBOL: " + existingSymbol.Name);
            variableType = existingSymbol.Type;
        }
        else
        {
            Console.WriteLine("SYMBOL IS NULL");
            _symbolTable.AddVariable(variableName, expressionType); //Add the symbol to the symbol table with the type of the expression if it is not null. Otherwise the type is unknown.
            variableType = expressionType;
            Console.WriteLine("New Symbol added to symbol table: " + node.Identifier.Name + " with type: " + expression.Type);
        }

        // Check if the variable type matches the expression type.
        if (variableType != null && expressionType != null && !variableType.Equals(expressionType))
        {
            Console.WriteLine($"Error in assignment: variableName = {variableName}, variableType = {variableType}, expressionType = {expressionType}");
            throw new InvalidOperationException($"Type mismatch: Cannot assign {expressionType} to {variableType} for variable '{variableName}'.");
        }



        Console.WriteLine($"Type checked assignment node: Identifier='{variableName}', Type='{variableType}', ExpressionType='{expressionType}'");

        // Return a decorated AssignmentNode
        AssignmentNode decoratedAssignmentNode = new AssignmentNode(identifierNode, node.AssignmentOperator, expression, node.SourceLocation);
        return decoratedAssignmentNode;
    }
}












    public override IASTNode? Visit(OperatorNode node)
    {
        Console.WriteLine("Type checking operator node");
        return node;
    }

    
    public override IASTNode? Visit(IdentifierGroupingNode node)
    {
        Console.WriteLine("Type checking identifier grouping node");

        // If the identifier already exists in the symbol table, throw an error
        if (_symbolTable.IsDefined(node.Identifier.Name))
        {
            throw new InvalidOperationException($"Identifier '{node.Identifier.Name}' already exists at {node.SourceLocation}");
        }

        // Visit the grouping elements and create a decorated grouping elements node
        GroupingElementsNode decoratedGroupingElements = (GroupingElementsNode)Visit(node.GroupingElements)
                                                         ?? throw new InvalidOperationException("Failed to create a decorated grouping elements node.");

        // Create a decorated identifier grouping node with type information
        IdentifierGroupingNode decoratedNode = new IdentifierGroupingNode(node.Identifier, decoratedGroupingElements, node.SourceLocation);

        // Add the identifier to the symbol table with the appropriate type and values
        _symbolTable.AddVariable(node.Identifier.Name, "Group", decoratedNode);

        Console.WriteLine("Type checked identifier grouping node");
        return decoratedNode;
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
        TypeNode decoratedReturnType = (TypeNode?)Visit(node.ReturnType) ?? throw new InvalidOperationException("Fail in node: " + node.Identifier +". Failed to create a decorated return type node.");
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
            IASTNode? decoratedArgument;

            switch (argument)
            {
                case ArgumentNode argumentNode:
                    decoratedArgument = Visit(argumentNode);
                    break;
                case IntegerLiteralNode integerNode:
                    decoratedArgument = Visit(integerNode);
                    break;
                case FloatLiteralNode floatNode:
                    decoratedArgument = Visit(floatNode);
                    break;
                case IdentifierNode identifierNode:
                    decoratedArgument = Visit(identifierNode);
                    break;

                // Add more cases here for other types of nodes that can be used as arguments.
                default:
                    throw new InvalidOperationException($"Invalid argument type: {argument.GetType().Name}. Expected a valid argument type.");
            }

            if (decoratedArgument == null)
            {
                throw new InvalidOperationException($"Failed to create a decorated argument node for argument: {argument}");
            }
            decoratedArguments.Add(decoratedArgument);
        }

        // Use the GetFunctionReturnType method to get the return type of the function call.
        TypeNode.TypeKind returnType = GetFunctionReturnType(node.FunctionIdentifier);

        TypeNode typeNode = new TypeNode(returnType, node.SourceLocation);
        FunctionCallNode decoratedFunctionCallNode = new FunctionCallNode(node.FunctionIdentifier, decoratedArguments, node.SourceLocation);
        decoratedFunctionCallNode.Type = typeNode;

        Console.WriteLine("HERE IS THE RETURN TYPE OF FUNCTION CALL: " + decoratedFunctionCallNode.FunctionIdentifier.ToString() + " " + decoratedFunctionCallNode.Type);
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
        else if (node.ForStatement != null || node is ForLoopNode)
        {
            decoratedNode = Visit((ForLoopNode)node);
        }
        else if (node.WhileStatement != null)
        {
            decoratedNode = Visit(node.WhileStatement);
        }
        else if (node.ReturnStatement != null)
        {
            decoratedNode = Visit(node.ReturnStatement);
        }
        else if (node is IfStatementNode ifStatementNode)
        {
            decoratedNode = Visit(ifStatementNode);
        }
        else if (node is IdentifierGroupingNode identifierGroupingNode)
        {
            decoratedNode = Visit(identifierGroupingNode);
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


    
    
    
    
    public override IASTNode Visit(IfStatementNode node)
    {
        Console.WriteLine("Type checking if statement node");

        // Type check the condition
        ExpressionNode decoratedCondition = (ExpressionNode?)Visit(node.Condition) ?? throw new InvalidOperationException("Failed to create a decorated condition.");

        // Type check the if block
        BlockNode decoratedIfBlock = (BlockNode?)Visit(node.IfBlock) ?? throw new InvalidOperationException("Failed to create a decorated if block.");

        // Type check the else if branches
        IList<ElseIfNode> decoratedElseIfBranches = new List<ElseIfNode>();
        foreach (ElseIfNode elseIfBranch in node.ElseIfBranches)
        {
            ElseIfNode decoratedElseIfBranch = (ElseIfNode?)Visit(elseIfBranch) ?? throw new InvalidOperationException("Failed to create a decorated else if branch.");
            decoratedElseIfBranches.Add(decoratedElseIfBranch);
        }

        // Type check the else branch
        ElseNode? decoratedElseBranch = null;
        if (node.ElseBranch != null)
        {
            decoratedElseBranch = (ElseNode?)Visit(node.ElseBranch) ?? throw new InvalidOperationException("Failed to create a decorated else branch.");
        }

        // Create a new decorated IfStatementNode with the type checked condition, if block, else if branches, and else branch
        IfStatementNode decoratedIfStatementNode = new IfStatementNode(decoratedCondition, decoratedIfBlock, decoratedElseIfBranches, decoratedElseBranch, node.SourceLocation);

        Console.WriteLine("Type checked if statement node");
        return decoratedIfStatementNode;
    }


    
    public override IASTNode? Visit(ElseIfNode node)
    {
        Console.WriteLine("Type checking else if node");
        Console.WriteLine("Condition " + node.Condition);

        // Type check the condition
        ExpressionNode decoratedCondition = (ExpressionNode?)Visit(node.Condition) ?? throw new InvalidOperationException("Failed to create a decorated condition.");

        // Type check the else if block
        BlockNode decoratedElseIfBlock = (BlockNode?)Visit(node.ElseIfBlock) ?? throw new InvalidOperationException("Failed to create a decorated else if block.");

        // Create a new decorated ElseIfNode with the type checked condition and else if block
        ElseIfNode decoratedElseIfNode = new ElseIfNode(decoratedCondition, decoratedElseIfBlock, node.SourceLocation);

        Console.WriteLine("Type checked else if node");
        return decoratedElseIfNode;
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
    
    public override IASTNode Visit(ForLoopNode node)
    {
        Console.WriteLine("Type checking for loop node");

        // Perform type checking and other operations on the ForLoopNode's properties
        // For example:
        Visit((AssignmentNode)node.Initialization);
        Visit((ExpressionNode)node.Condition);
        Visit((AssignmentNode)node.Update);



        _symbolTable.EnterScope(); // Enter new scope
        Visit(node.Body);
        _symbolTable.ExitScope();

        Console.WriteLine("Type checked for loop node");
        return node;
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
        Console.WriteLine("NOW VISITING EXPRESSION: " + node);
        Console.WriteLine("Node type is: " + node.Value);
        if (node.ExpressionType == ExpressionNodeType.Literal && node.Value != null)
        {
            Console.WriteLine("THIS IS THE EXPRESSION TYPE " + node.ExpressionType);
            // Visit the literal value node and set the Type of the decorated expression node
            IASTNode? decoratedValueNode = VisitNodeBasedOnExpressionType(node.ExpressionType, node.Value);
            ExpressionNode decoratedLiteralExpressionNode = new ExpressionNode(
                node.ExpressionType,
                null, // LeftOperand
                null, // RightOperand
                null, // OperatorNode
                node.SourceLocation
            )
            {
                Value = decoratedValueNode
            };

            // Set the Type property based on the type of decoratedValueNode
            if (decoratedValueNode is IntegerLiteralNode intNode)
            {
                decoratedLiteralExpressionNode.Type = intNode.Type;
            }
            else if (decoratedValueNode is FloatLiteralNode floatNode)
            {
                decoratedLiteralExpressionNode.Type = floatNode.Type;
            }
            else if (decoratedValueNode is StringLiteralNode stringLiteralNode)
            {
                decoratedLiteralExpressionNode.Type = stringLiteralNode.Type;
            }
            else if (decoratedValueNode is BooleanLiteralNode boolNode)
            {
                decoratedLiteralExpressionNode.Type = boolNode.Type;
            }

            Console.WriteLine("Type checked expression node");
            return decoratedLiteralExpressionNode;
        }

        if (node.ExpressionType == ExpressionNodeType.Identifier)
        {
            if (node is IdentifierNode identifierNode)
            {
                string variableName = identifierNode.Name;
                Symbol? symbol = _symbolTable.Lookup(variableName);
                if (symbol == null)
                {
                    throw new InvalidOperationException($"Undefined variable '{variableName}' used in expression.");
                }
                
                ExpressionNode decoratedIdentifierExpressionNode = new ExpressionNode(
                    node.ExpressionType,
                    null, // LeftOperand
                    null, // RightOperand
                    null, // OperatorNode
                    node.SourceLocation
                )
                {
                    Value = symbol.Value,
                };

                decoratedIdentifierExpressionNode.Type = new TypeNode(GetTypeKindFromString(symbol.Type), node.SourceLocation);
                Console.WriteLine("decoratedIdentifierExpressionNode type is: " + decoratedIdentifierExpressionNode.Type);
                Console.WriteLine("Type checked expression node");
                return decoratedIdentifierExpressionNode;
            }
            else
            {
                throw new InvalidOperationException("Invalid node type for identifier expression.");
            }
        }



        IASTNode? decoratedLeftOperand = null;
        IASTNode? decoratedRightOperand = null;
        OperatorNode? decoratedOperatorNode = null;

        if (node.LeftOperand != null)
        {
            Console.WriteLine("Left operand: " + node.LeftOperand);
            decoratedLeftOperand = VisitNodeBasedOnExpressionType(node.ExpressionType, node.LeftOperand) ?? throw new InvalidOperationException("Failed to create a decorated left operand node.");
        }

        if (node.RightOperand != null)
        {
            Console.WriteLine("Right operand: " + node.RightOperand);
            decoratedRightOperand = VisitNodeBasedOnExpressionType(node.ExpressionType, node.RightOperand) ?? throw new InvalidOperationException("Failed to create a decorated right operand node.");
        }

        if (node.OperatorNode != null)
        {
            Console.WriteLine("Operator: " + node.OperatorNode);
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
        Console.WriteLine("Now in integer literal node with value" + node.Value);
        TypeNode typeNode = new TypeNode(TypeNode.TypeKind.Int, node.SourceLocation);
        IntegerLiteralNode decoratedNode = new IntegerLiteralNode(node.Value, node.SourceLocation);
        decoratedNode.Type = typeNode;
        return decoratedNode;
    }

    
    public override IASTNode? Visit(FloatLiteralNode node)
    {
        Console.WriteLine("Type checking float literal node with value " + node.Value);
        TypeNode typeNode = new TypeNode(TypeNode.TypeKind.Float, node.SourceLocation);
        FloatLiteralNode decoratedNode = new FloatLiteralNode(node.Value, node.SourceLocation);
        decoratedNode.Type = typeNode;
        return decoratedNode;
    }
    
    public override IASTNode? Visit(StringLiteralNode node)
    {
        Console.WriteLine("Type checking string literal node");
        TypeNode typeNode = new TypeNode(TypeNode.TypeKind.String, node.SourceLocation);
        StringLiteralNode decoratedNode = new StringLiteralNode(node.Value, node.SourceLocation);
        decoratedNode.Type = typeNode;
        return decoratedNode;
    }
    
    public override IASTNode? Visit(BooleanLiteralNode node)
    {
        Console.WriteLine("Type checking boolean literal node");
        TypeNode typeNode = new TypeNode(TypeNode.TypeKind.Bool, node.SourceLocation);
        BooleanLiteralNode decoratedNode = new BooleanLiteralNode(node.Value, node.SourceLocation);
        decoratedNode.Type = typeNode;
        return decoratedNode;
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
        IdentifierNode decoratedName = node.Name;
        
        // Add the sequence identifier to the symbol table before entering the sequence scope
        _symbolTable.AddSequence(decoratedName.Name);

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

    
    
    public override IASTNode? Visit(ShapeInitNode node)
    {
        Console.WriteLine("Type checking shape initialization node");

        // Visit the arguments and create decorated arguments
        Dictionary<string, IASTNode> decoratedArguments = new Dictionary<string, IASTNode>();
        foreach (KeyValuePair<string, IASTNode> arg in node.Arguments)
        {
            IASTNode decoratedArgValue;

            // Call VisitNodeBasedOnExpressionType method with the expression type of arg.Value
            decoratedArgValue = VisitNodeBasedOnExpressionType(((ExpressionNode)arg.Value).ExpressionType, arg.Value) ?? throw new InvalidOperationException($"Failed to create a decorated argument value for '{arg.Key}'.");

            decoratedArguments.Add(arg.Key, decoratedArgValue);
        }

        // Create a decorated shape initialization node with the same shape type and decorated arguments
        ShapeInitNode decoratedNode = new ShapeInitNode(node.ShapeType, decoratedArguments, node.SourceLocation);

        Console.WriteLine("Type checked shape initialization node");
        return decoratedNode;
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
        Console.WriteLine("TYPE NODE: " + node.GetType());
        //TypeNode does not have any children, so it can just be returned as is
        return node;
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
                    Console.WriteLine("Type CHECKING integer literal node");
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
    
    
    // This method is used to get the return type of a function that is called.
    private TypeNode.TypeKind GetFunctionReturnType(IdentifierNode functionIdentifier)
    {
        // First, check if the function is a built-in function
        TypeNode.TypeKind? builtInFunctionType = GetBuiltInFunctionReturnType(functionIdentifier.Name);
        if (builtInFunctionType.HasValue)
        {
            return builtInFunctionType.Value;
        }

        // If not a built-in function, look it up in the symbol table
        Symbol? symbol = _symbolTable.Lookup(functionIdentifier.Name);
        if (symbol == null)
        {
            throw new InvalidOperationException($"Function '{functionIdentifier.Name}' is not defined.");
        }

        return ConvertStringTypeToTypeKind(symbol.Type);
    }
    
    
    //This method is used for returning the type of the built-in functions
    private TypeNode.TypeKind? GetBuiltInFunctionReturnType(string functionName)
    {
        switch (functionName)
        {
            case "rgb":
                return TypeNode.TypeKind.String;
            case "MoveTo":
                return TypeNode.TypeKind.String;
            case "repeat":
                return TypeNode.TypeKind.Int;
            default:
                return null; // Not a built-in function
        }
    }
    
    private TypeNode.TypeKind GetTypeKindFromString(string type)
    {
        // Extract the type name from the string representation of the TypeNode
        string[] typeParts = type.ToLower().Split();

        // If the string does not contain a space character, use the first element directly
        string typeName = typeParts.Length > 1 ? typeParts[1] : typeParts[0];

        return typeName switch
        {
            "int" => TypeNode.TypeKind.Int,
            "float" => TypeNode.TypeKind.Float,
            "bool" => TypeNode.TypeKind.Bool,
            "string" => TypeNode.TypeKind.String,
            "Circle" => TypeNode.TypeKind.Circle,
            "Polygon" => TypeNode.TypeKind.Polygon,
            _ => throw new InvalidOperationException($"Unknown type '{typeName}'."),
        };
    }


    private Symbol? GetSymbolFromIdentifierNode(IdentifierNode identifierNode)
    {
        string variableName = identifierNode.Name;
        Symbol? symbol = _symbolTable.Lookup(variableName);
        if (symbol == null)
        {
            throw new InvalidOperationException($"Undefined variable '{variableName}' used in expression.");
        }

        return symbol;
    }


}

