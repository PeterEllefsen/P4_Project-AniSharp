using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Channels;
using System.Xml;
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
        
        return new ProgramNode(decoratedPrototypes, decoratedSetup, decoratedFunctionDeclarations, decoratedSequences, decoratedTimeline, node.SourceLocation);
    }



    
    public override SetupNode Visit(SetupNode node)
    {
        // Visit the grouping elements node and get the decorated GroupingElementsNode.
        GroupingElementsNode decoratedGroupingElements = (GroupingElementsNode)Visit(node.GroupingElements) ?? throw new InvalidOperationException("Failed to create a decorated grouping elements node.");

        // Create a decorated SetupNode with the decorated GroupingElementsNode and set its SourceLocation.
        SetupNode decoratedSetupNode = new SetupNode(decoratedGroupingElements, node.SourceLocation);
        
        return decoratedSetupNode;
    }


    public override GroupingElementsNode Visit(GroupingElementsNode node)
    {
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
        
        return decoratedGroupingElementsNode;
    }


    
    public override IASTNode? Visit(KeyValuePairNode node)
    {
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
        
        return decoratedKeyValuePairNode;
    }

    
    
    
    public override IASTNode? Visit(IdentifierNode node)
    {
        if (PredefinedKeywords.Contains(node.Name))
        {
            // If the identifier is a predefined keyword, return the node without further checking
            return node;
        }

        // Check if the identifier is a built-in function
        TypeNode.TypeKind? builtInFunctionReturnType = GetBuiltInFunctionReturnType(node.Name);

        if (builtInFunctionReturnType.HasValue)
        {
            node.Type = new TypeNode(builtInFunctionReturnType.Value, node.SourceLocation);
            return node;
        }

        if (!_symbolTable.IsDefined(node.Name))
        {
            throw new InvalidOperationException($"Identifier '{node.Name}' is not defined at {node.SourceLocation}");
        }

        // Look up the corresponding symbol in the symbol table
        Symbol? symbol = _symbolTable.Lookup(node.Name);

        // Set the Type property of the IdentifierNode to the type of the symbol
        if (symbol != null)
        {
            TypeNode.TypeKind typeKind = ConvertStringTypeToTypeKind(symbol.Type);
            TypeNode typeNode = new TypeNode(typeKind, node.SourceLocation);
            node.Type = typeNode;

            // Set the VariableType property of the IdentifierNode to the VariableType of the symbol
            node.VariableType = ConvertStringTypeToVariableType(symbol.Type);
        }
        
        return node;
    }






    public override IASTNode Visit(AssignmentNode node)
    {
        Symbol? symbol = _symbolTable.Lookup("Prototype: " + node.Expression.Identifier?.Name);
        string functionIdentifier = "";
        if (symbol != null)
        {
            functionIdentifier = symbol.Name.Substring(11);
        }

        // Type check ExpressionNode
        ExpressionNode decoratedExpressionNode = (ExpressionNode?)Visit(node.Expression) ?? throw new InvalidOperationException("Failed to create a decorated expression node.");
        // Check if the identifier already exists in the symbol table
        if (_symbolTable.IsDefinedInCurrentScope(node.Identifier.Name))
        {
            if (symbol != null)
            {
                string? nodeIdentifier = "";
                if (node.Identifier.Name != null)
                {
                    nodeIdentifier = node.Identifier.Name;
                }

                Symbol? leftOperand = _symbolTable.Lookup(nodeIdentifier);

                if (leftOperand?.Type.ToLower() != symbol.Type.ToLower())
                {
                    throw new ArgumentException("Cannot assign a value of type " + symbol.Type + " to a variable of type " + leftOperand?.Type + " at " + node.SourceLocation);
                }
                
                VariableType symbolVariableType = ConvertStringTypeToVariableType(symbol.Type);
                decoratedExpressionNode.VariableType = symbolVariableType;

                if (node.IsDeclaration == false && symbolVariableType != decoratedExpressionNode.VariableType)
                {
                    throw new InvalidOperationException(
                        $"Type mismatch in assignment. Cannot assign a value of type '{decoratedExpressionNode.VariableType}' to variable '{node.Identifier.Name}' whose type is '{symbolVariableType}' at {node.SourceLocation}.");
                }
            }
        }
        else
        {
            if (decoratedExpressionNode.VariableType != node.VariableType && decoratedExpressionNode.VariableType != VariableType.Function)
            {
                throw new InvalidOperationException($"Type mismatch in assignment. Cannot assign a value of type '{decoratedExpressionNode.VariableType}' to variable of type '{node.VariableType}' at {node.SourceLocation}.");
            }
            if (decoratedExpressionNode.VariableType.ToString() == VariableType.Function.ToString())
            {
                if (symbol?.Type != null)
                {
                    VariableType symbolVariableType = ConvertStringTypeToVariableType(symbol.Type);
                    decoratedExpressionNode.VariableType = symbolVariableType;
                }
               
            }

            _symbolTable.AddVariable(node.Identifier.Name, ConvertVariableTypeToString(decoratedExpressionNode.VariableType), decoratedExpressionNode);
        }

        // Type check IdentifierNode
        IdentifierNode decoratedIdentifierNode = (IdentifierNode?)Visit(node.Identifier) ?? throw new InvalidOperationException("Failed to create a decorated identifier node.");

        // Create decorated AssignmentNode
        AssignmentNode decoratedAssignmentNode = new AssignmentNode(
            decoratedIdentifierNode,
            node.AssignmentOperator,
            decoratedExpressionNode,
            decoratedExpressionNode.VariableType,
            node.SourceLocation
        );

        if (node.IsDeclaration == true)
        {
            decoratedAssignmentNode.IsDeclaration = true;
        }
        else
        {
            decoratedAssignmentNode.IsDeclaration = false;
        }
        
        return decoratedAssignmentNode;
    }


    

    public override IASTNode? Visit(OperatorNode node)
    {
        return node;
    }

    
    public override IASTNode? Visit(IdentifierGroupingNode node)
    {
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

        return decoratedNode;
    }


    
    
    
    public override IASTNode Visit(ArgumentNode node)
    {
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
        else if (node.Value is FunctionCallNode functionCallNode)
        {
            decoratedValue = Visit(functionCallNode) ?? throw new InvalidOperationException("Failed to create a decorated function call node.");
        }
        else
        {
            throw new InvalidOperationException($"Unrecognized value type for argument '{node.Name}' at {node.SourceLocation}");
        }

        ArgumentNode decoratedArgumentNode = new ArgumentNode(node.Name, decoratedValue, node.SourceLocation);
        
        return decoratedArgumentNode;
    }

    
    
    public override IASTNode Visit(FunctionDeclarationNode node)
    {
        _symbolTable.EnterScope();
        foreach (var child in node.Parameters)
        {
            _symbolTable.AddVariable(child.Name, child.DataType.ToString());
        }
        
        if(!(_symbolTable.IsDefined("Prototype: " + node.Identifier.Name))) //If the prototype does not exist, throw an error
        {
            throw new InvalidOperationException($"Prototype for function '{node.Identifier.Name}' do not exist.");
        }

        string parametersString = "";
        foreach (var parameter in node.Parameters)
        {
            parametersString += parameter.DataType + ", "; //Add the parameters to a string
        }
        
        
        Symbol? symbol = _symbolTable.Lookup("Prototype: " + node.Identifier.Name); //fetch the prototype data
        StringLiteralNode? prototypeParameters; // used to compare the parameters of the prototype with the function declaration
        
        if (symbol == null)
        {
            throw new ArgumentNullException($"Prototype for function {node.Identifier.Name} not found.");
        }
        if (symbol.Value == null)
        {
            throw new ArgumentNullException($"symbol.Value is null");
        }
        
        prototypeParameters = (StringLiteralNode)symbol.Value;

        if (symbol.Type != node.ReturnType.ToString()) //If the return type matches the prototype proceed.
        {
            throw new ArgumentException("Return type of function " + node.Identifier.Name + " does not match return type of prototype.");
        }
        if(parametersString != "" && (prototypeParameters.Value != parametersString)) // If the parameters do not match the prototype
        {
            throw new ArgumentException("Parameters of function " + node.Identifier.Name + " do not match parameters of prototype.");
        }
        if (prototypeParameters.Value == "" && parametersString != "")
        {
            throw new ArgumentException($"Prototype of function {node.Identifier.Name} do not have any parameters.");
        }
        if (prototypeParameters.Value != "" && parametersString == "")
        {
            throw new ArgumentException("Function '" + node.Identifier.Name + "' do not have any parameters, while the prototype do.");
        }

        // Visit ReturnType and Identifier nodes.
        TypeNode decoratedReturnType = (TypeNode?)Visit(node.ReturnType) ?? throw new InvalidOperationException("Fail in node: " + node.Identifier +". Failed to create a decorated return type node.");
        

        // Add function to the symbol table
        _symbolTable.AddFunction(node.Identifier.Name, decoratedReturnType.ToString());
        IdentifierNode decoratedIdentifier = (IdentifierNode?)Visit(node.Identifier) ?? throw new InvalidOperationException("Failed to create a decorated identifier node.");
        
        // Visit Parameter nodes and create a list of decorated parameters.
        List<ParameterNode> decoratedParameters = new List<ParameterNode>();
        if (node.Parameters != null)
        {
            foreach (ParameterNode parameterNode in node.Parameters)
            {
                decoratedParameters.Add((ParameterNode)Visit(parameterNode) ?? throw new InvalidOperationException("Failed to create a decorated parameter node."));
            }
        }

        // Enter the scope for the function declaration.
        

        // Visit the Block node.
        BlockNode decoratedBlock = (BlockNode?)Visit(node.Block) ?? throw new InvalidOperationException("Failed to create a decorated block node.");

        // Exit the scope for the function declaration.
        _symbolTable.ExitScope();

        // Create a new decorated FunctionDeclarationNode with the decorated nodes and set its SourceLocation.
        FunctionDeclarationNode decoratedFunctionDeclarationNode = new FunctionDeclarationNode(decoratedReturnType, decoratedIdentifier, decoratedParameters, decoratedBlock, node.SourceLocation);

        return decoratedFunctionDeclarationNode;
    }



    
public override IASTNode? Visit(FunctionCallNode node)
{
    string functionCallName = node.FunctionIdentifier.Name;
    List<IASTNode> decoratedArguments = new List<IASTNode>();

    string parametersString = "";

    foreach (IASTNode argument in node.Arguments)
    {
        IASTNode? decoratedArgument;
        switch (argument)
        {
            case ArgumentNode argumentNode:
                decoratedArgument = Visit(argumentNode);
                if (decoratedArgument != null)
                {
                    parametersString += ((ArgumentNode)decoratedArgument) + ", ";
                }
                break;
            case IntegerLiteralNode integerNode:
                decoratedArgument = Visit(integerNode);
                if (decoratedArgument != null)
                {
                    parametersString += ((IntegerLiteralNode)decoratedArgument).Type + ", ";
                }
                break;
            case FloatLiteralNode floatNode:
                decoratedArgument = Visit(floatNode);
                if (decoratedArgument != null)
                {
                    parametersString += ((FloatLiteralNode)decoratedArgument).Type + ", ";
                }
                break;
            case IdentifierNode identifierNode:
                decoratedArgument = Visit(identifierNode);
                if (decoratedArgument != null)
                {
                    parametersString += ((IdentifierNode)decoratedArgument).Type + ", ";
                }
                break;
            case FunctionCallNode functionCallNode:
                decoratedArgument = Visit(functionCallNode);
                if (decoratedArgument != null)
                {
                    parametersString += ((FunctionCallNode)decoratedArgument).Type + ", ";
                }
                break;
            default:
                throw new InvalidOperationException($"Invalid argument type: {argument.GetType().Name}. Expected a valid argument type.");
        }

        if (decoratedArgument == null)
        {
            throw new InvalidOperationException($"Failed to create a decorated argument node for argument: {argument}");
        }
        decoratedArguments.Add(decoratedArgument);
    }

    TypeNode.TypeKind returnType;

    // Check if the function is a built-in function
    TypeNode.TypeKind? builtInReturnType = GetBuiltInFunctionReturnType(functionCallName);
    if (builtInReturnType.HasValue)
    {
        returnType = builtInReturnType.Value;
    }
    else
    {
        // Check if the actual arguments types match the expected parameters types
        Symbol? prototypeSymbol = _symbolTable.Lookup("Prototype: " + functionCallName);
        if (prototypeSymbol == null || prototypeSymbol.Value == null)
        {
            throw new InvalidOperationException($"Prototype for function {functionCallName} not found.");
        }

        StringLiteralNode? prototypeParametersNode = prototypeSymbol?.Value as StringLiteralNode;
        
        if (prototypeParametersNode == null || prototypeParametersNode.Value != parametersString)
        {
            throw new InvalidOperationException(
                $"Function call {functionCallName} argument types do not match function parameters. Cannot use parameters of type {parametersString}for function {functionCallName} with parameters of type {prototypeParametersNode?.Value}");
        }

        // Use the GetFunctionReturnType method to get the return type of the function call.
        returnType = GetFunctionReturnType(node.FunctionIdentifier);
    }

    TypeNode typeNode = new TypeNode(returnType, node.SourceLocation);
    FunctionCallNode decoratedFunctionCallNode = new FunctionCallNode(node.FunctionIdentifier, decoratedArguments, node.SourceLocation);
    decoratedFunctionCallNode.Type = typeNode;

    return decoratedFunctionCallNode;
}












    
    public override IASTNode Visit(ParameterNode node)
    {
        ParameterNode decoratedNode = new ParameterNode(node.DataType, node.Name, node.SourceLocation);
        
        return decoratedNode;
    }

    
    public override IASTNode? Visit(CallParameterNode node)
    {
        return VisitChildren(node);
    }
    
    public override IASTNode Visit(PrototypeNode node)
    {
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
            string parameterString = "";
            foreach (var parameter in parameters)
            {
                parameterString += parameter.DataType + ", ";
            }

            StringLiteralNode parameterStringNode = new StringLiteralNode(parameterString, node.SourceLocation);
            _symbolTable.AddPrototype(("Prototype: " + functionName), returnType.ToString(), parameterStringNode);
        }

        // Visit all of the parameters in the prototype and create a list of decorated parameters.
        List<ParameterNode> decoratedParameters = new List<ParameterNode>();
        foreach (ParameterNode parameter in parameters)
        {
            IASTNode? visitedParameter = Visit(parameter);
            if (visitedParameter != null)
            {
                decoratedParameters.Add((ParameterNode)visitedParameter);
            }
        }

        // Create a decorated PrototypeNode
        PrototypeNode decoratedNode = new PrototypeNode(returnType, functionName, decoratedParameters, node.SourceLocation);
        
        return decoratedNode;
    }
    


    public override IASTNode? Visit(StatementNode node)
    {
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

        return decoratedNode;
    }


    
    
    
    
    public override IASTNode Visit(IfStatementNode node)
    {
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

        return decoratedIfStatementNode;
    }


    
    public override IASTNode? Visit(ElseIfNode node)
    {
        // Type check the condition
        ExpressionNode decoratedCondition = (ExpressionNode?)Visit(node.Condition) ?? throw new InvalidOperationException("Failed to create a decorated condition.");

        // Type check the else if block
        BlockNode decoratedElseIfBlock = (BlockNode?)Visit(node.ElseIfBlock) ?? throw new InvalidOperationException("Failed to create a decorated else if block.");

        // Create a new decorated ElseIfNode with the type checked condition and else if block
        ElseIfNode decoratedElseIfNode = new ElseIfNode(decoratedCondition, decoratedElseIfBlock, node.SourceLocation);

        return decoratedElseIfNode;
    }

    
    public override IASTNode? Visit(ElseNode node)
    {
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(WhileLoopNode node)
    {
        return VisitChildren(node);
    }
    
    public override IASTNode Visit(ForLoopNode node)
    {
        _symbolTable.EnterScope(); // Enter new scope
        // Perform type checking and other operations on the ForLoopNode's properties
        // For example:
        Visit((AssignmentNode)node.Initialization);
        Visit((ExpressionNode)node.Condition);
        if (node.Update is AssignmentNode assignmentNode)
        {
            Visit((AssignmentNode)node.Update);
        }
        else if (node.Update is UnaryOperationNode unaryOperationNode)
        {
            Visit((UnaryOperationNode)node.Update);
        }

        Visit(node.Body);
        _symbolTable.ExitScope();

        return node;
    }

    
    
    
    public override IASTNode? Visit(BlockNode node)
    {
        // Type check the statements in the block
        IList<StatementNode> decoratedStatementNodes = new List<StatementNode>();
        foreach (StatementNode statementNode in node.Statements)
        {
            // Check if the statement is a variable declaration
            if (statementNode is AssignmentNode assignmentNode)
            {
                if(assignmentNode.IsDeclaration)
                {
                    Symbol? symbol = _symbolTable.LookupInCurrentScope(assignmentNode.Identifier.Name);
                    // Check if a variable with the same name already exists in the current scope
                    if (symbol != null)
                    {
                        throw new InvalidOperationException($"Variable '{assignmentNode.Identifier.Name}' is already declared in this scope.");
                    }

                    // Add the variable to the symbol table
                    _symbolTable.AddVariable(assignmentNode.Identifier.Name, ConvertVariableTypeToString(assignmentNode.VariableType));
                }
                
            }

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

        return decoratedBlockNode;
    }





    public override IASTNode? Visit(SeqBlockNode node)
    {
        List<StatementNode> decoratedStatements = new List<StatementNode>();
        List<AnimationNode> decoratedAnimations = new List<AnimationNode>();

        foreach (var statementNode in node.Statements)
        {
            if (statementNode is AssignmentNode assignmentNode)
            {
                if(assignmentNode.IsDeclaration)
                {
                    Symbol? symbol = _symbolTable.LookupInCurrentScope(assignmentNode.Identifier.Name);
                    // Check if a variable with the same name already exists in the current scope
                    if (symbol != null)
                    {
                        throw new InvalidOperationException($"Variable '{assignmentNode.Identifier.Name}' is already declared in this scope.");
                    }
                    
                    // Add the variable to the symbol table
                    _symbolTable.AddVariable(assignmentNode.Identifier.Name, ConvertVariableTypeToString(assignmentNode.VariableType));
                }
                
            }
            
            StatementNode decoratedStatementNode = (StatementNode?)Visit(statementNode) ?? throw new InvalidOperationException("Failed to create a decorated statement node.");
            decoratedStatements.Add(decoratedStatementNode);
        }

        foreach (var animationNode in node.Animations)
        {
            AnimationNode decoratedAnimationNode = (AnimationNode?)Visit(animationNode) ?? throw new InvalidOperationException("Failed to create a decorated animation node.");
            decoratedAnimations.Add(decoratedAnimationNode);
        }

        SeqBlockNode decoratedSeqBlockNode = new SeqBlockNode(decoratedStatements, decoratedAnimations, node.SourceLocation);

        return decoratedSeqBlockNode;
    }

    
    public override IASTNode Visit(AnimationNode node)
    {
        // Type check IdentifierNode
        IdentifierNode decoratedIdentifierNode = (IdentifierNode?)Visit(node.Identifier) ?? throw new InvalidOperationException("Failed to create a decorated identifier node.");

        // Type check CommandNode if it exists
        CommandNode? decoratedCommandNode = null;
        if (node.Command != null)
        {
            decoratedCommandNode = (CommandNode?)Visit(node.Command) ?? throw new InvalidOperationException("Failed to create a decorated command node.");
        }

        // Type check each TransitionNode in the list
        List<TransitionNode> decoratedTransitions = new List<TransitionNode>();
        foreach (var transitionNode in node.Transitions)
        {
            TransitionNode decoratedTransitionNode = (TransitionNode?)Visit(transitionNode) ?? throw new InvalidOperationException("Failed to create a decorated transition node.");
            decoratedTransitions.Add(decoratedTransitionNode);
        }

        AnimationNode decoratedAnimationNode = new AnimationNode(decoratedIdentifierNode, decoratedCommandNode, decoratedTransitions, node.SourceLocation);

        return decoratedAnimationNode;
    }

    
    public override IASTNode Visit(CommandNode node)
    {
        // Type check IdentifierNode
        IdentifierNode decoratedIdentifierNode = (IdentifierNode?)Visit(node.Identifier) ?? throw new InvalidOperationException("Failed to create a decorated identifier node.");

        // Type check each parameter in the list
        List<IASTNode> decoratedParameters = new List<IASTNode>();
        if (node.Parameters != null)
        {
            foreach (var parameter in node.Parameters)
            {
                IASTNode? decoratedParameter;

                if (parameter is ParameterNode parameterNode)
                {
                    decoratedParameter = Visit(parameterNode);
                }
                else if (parameter is CallParameterNode callParameterNode)
                {
                    decoratedParameter = Visit(callParameterNode);
                }
                else if (parameter is ExpressionNode expressionNode)
                {
                    decoratedParameter = Visit(expressionNode);
                }
                else if (parameter is TupleNode tupleNode)
                {
                    decoratedParameter = Visit(tupleNode);
                }
                else
                {
                    throw new InvalidOperationException($"Unexpected parameter type: {parameter.GetType().Name}");
                }

                if (decoratedParameter != null)
                {
                    decoratedParameters.Add(decoratedParameter);
                }
                else
                {
                    throw new InvalidOperationException("Failed to create a decorated parameter node.");
                }
            }
        }

        CommandNode decoratedCommandNode = new CommandNode(decoratedIdentifierNode, decoratedParameters, node.SourceLocation);

        return decoratedCommandNode;
    }




    
    public override IASTNode? Visit(ConditionNode node)
    {
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(ExpressionNode node)
    {
        if (node is IdentifierNode identifierNode)
        {
            IdentifierNode decoratedIdentifierNode = (IdentifierNode?)Visit(identifierNode) ?? throw new InvalidOperationException("Failed to create a decorated identifier node.");
            if (_symbolTable.IsDefined(decoratedIdentifierNode.Name))
            {
                Symbol? symbol = _symbolTable.Lookup(decoratedIdentifierNode.Name);
            }
            return node;
        }
        else if (node.ExpressionType is ExpressionNodeType.Binary)
        {
            return VisitBinaryExpression(node);
        }

        return node;
    }

    
    //This method is a custom method for type checking binary nodes.
    public IASTNode? VisitBinaryExpression(ExpressionNode node)
    {
        IASTNode? leftOperand = node.LeftOperand;
        IASTNode? rightOperand = node.RightOperand;
        OperatorNode? operatorNode = node.OperatorNode;
        
        // Visit left operand and right operand and perform type check.
        if ((operatorNode?.OperatorSymbol == "and") || (operatorNode?.OperatorSymbol == "or")) //If the binary expression is a logical expression
        {
            IdentifierNode? decoratedLeftIdentifier = null;
            IdentifierNode? decoratedRightIdentifier = null;
            IdentifierNode? leftIdentifier = null;
            IdentifierNode? rightIdentifier = null;

            if (leftOperand is IdentifierNode leftIdent) 
            {
                leftIdentifier = leftIdent;
                decoratedLeftIdentifier = (IdentifierNode?)Visit(leftIdentifier) ?? throw new InvalidOperationException("Failed to create a decorated identifier node.");
            }

            if (rightOperand is IdentifierNode rightIdent)
            {
                rightIdentifier = rightIdent;
                decoratedRightIdentifier = (IdentifierNode?)Visit(rightIdentifier) ?? throw new InvalidOperationException("Failed to create a decorated identifier node.");
            }

            if (leftOperand is BooleanLiteralNode leftBool && rightOperand is BooleanLiteralNode rightBool) //If both operands are boolean literals
            {
                Visit(leftBool);
                Visit(rightBool);
            }
            else if (leftIdentifier != null && rightIdentifier != null && leftIdentifier.VariableType == VariableType.Bool && rightIdentifier.VariableType == VariableType.Bool) //If both operands are identifiers and both are of type boolean
            {
                Visit(leftIdentifier);
                Visit(rightIdentifier);
            }
            else if (leftIdentifier != null && leftIdentifier.VariableType == VariableType.Bool && rightOperand is BooleanLiteralNode rightBoolLiteral) //If left operand is an identifier and is of type boolean and right operand is a boolean literal
            {
                Visit(leftIdentifier);
                Visit(rightBoolLiteral);
            }
            else if (rightIdentifier != null && rightIdentifier.VariableType == VariableType.Bool && leftOperand is BooleanLiteralNode leftBoolLiteral) //If right operand is an identifier and is of type boolean and left operand is a boolean literal
            {
                Visit(leftBoolLiteral);
                Visit(rightIdentifier);
            }
            else //If none ofthe operands are booleans 
            {
                throw new InvalidOperationException($"{leftOperand} and {rightOperand} must be of type boolean.");
            }
        }
        if (leftOperand is IdentifierNode leftIdentifierNode)
        {
            Visit(leftIdentifierNode);
        }
        else if (leftOperand is ExpressionNode leftExpressionNode)
        {
            Visit(leftExpressionNode);
        }
        else if (leftOperand is IntegerLiteralNode leftIntegerLiteralNode)
        {
            Visit(leftIntegerLiteralNode);
        }
        else if (leftOperand is FloatLiteralNode leftFloatLiteralNode)
        {
            Visit(leftFloatLiteralNode);
        }
        if (rightOperand is IdentifierNode rightIdentifierNode)
        {
            Visit(rightIdentifierNode);
        }
        else if (rightOperand is ExpressionNode rightExpressionNode)
        {
            Visit(rightExpressionNode);
        }
        
        return node;
    }






    
    public override IASTNode? Visit(FrameDefNode node)
    {
        // Visit and type check the sequence call in the frame definition node
        SequenceCallNode decoratedSequenceCallNode = (SequenceCallNode)Visit(node.SequenceCall) ?? throw new InvalidOperationException("Failed to create a decorated sequence call node.");

        // Create a new decorated FrameDefNode with the type checked sequence call
        FrameDefNode decoratedFrameDefNode = new FrameDefNode(node.FrameTime, decoratedSequenceCallNode, node.SourceLocation);

        return decoratedFrameDefNode;
    }

    public override IASTNode? Visit(IntegerLiteralNode node)
    {
        TypeNode typeNode = new TypeNode(TypeNode.TypeKind.Int, node.SourceLocation);
        IntegerLiteralNode decoratedNode = new IntegerLiteralNode(node.Value, node.SourceLocation);
        decoratedNode.Type = typeNode;
        return decoratedNode;
    }

    
    public override IASTNode? Visit(FloatLiteralNode node)
    {
        TypeNode typeNode = new TypeNode(TypeNode.TypeKind.Float, node.SourceLocation);
        FloatLiteralNode decoratedNode = new FloatLiteralNode(node.Value, node.SourceLocation);
        decoratedNode.Type = typeNode;
        return decoratedNode;
    }
    
    public override IASTNode? Visit(StringLiteralNode node)
    {
        TypeNode typeNode = new TypeNode(TypeNode.TypeKind.String, node.SourceLocation);
        StringLiteralNode decoratedNode = new StringLiteralNode(node.Value, node.SourceLocation);
        decoratedNode.Type = typeNode;
        return decoratedNode;
    }
    
    public override IASTNode? Visit(BooleanLiteralNode node)
    {
        TypeNode typeNode = new TypeNode(TypeNode.TypeKind.Bool, node.SourceLocation);
        BooleanLiteralNode decoratedNode = new BooleanLiteralNode(node.Value, node.SourceLocation);
        decoratedNode.Type = typeNode;
        return decoratedNode;
    }
    
    
    
    public override IASTNode? Visit(ReturnNode node)
    {
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

        return decoratedReturnNode;
    }


    
    
    
    public override IASTNode? Visit(SeqBlockPartNode node)
    {
        return VisitChildren(node);
    }
    
    public override IASTNode Visit(SequenceCallNode node)
    {
        IdentifierNode decoratedName = node.Name;
        string argumentsString = "";
        
        // Check if the sequence exists in the symbol table
        Symbol? sequenceSymbol = _symbolTable.Lookup(decoratedName.Name);
        if (sequenceSymbol == null)
        {
            throw new InvalidOperationException($"Sequence '{node.Name.Name}' is not defined.");
        }

        string sequenceParameters = sequenceSymbol.Type;

        // Type check the arguments of the sequence call
        List<ExpressionNode> decoratedArguments = new List<ExpressionNode>();
        if (node.Arguments.Count > 0)
        {
            foreach (ExpressionNode argument in node.Arguments)
            {
                ExpressionNode decoratedArgument = (ExpressionNode?)Visit(argument) ?? throw new InvalidOperationException("Failed to create a decorated argument node.");
                argumentsString += decoratedArgument.VariableType + ", ";
                decoratedArguments.Add(decoratedArgument);
            }
            
        }
        
        if(sequenceParameters != "" && argumentsString != "" && argumentsString != sequenceParameters) //if the parameters and arguments have values, but do not match
        {
            throw new InvalidOperationException($"Sequence '{node.Name.Name}' expects parameters of type '{sequenceParameters}' but got '{argumentsString}'.");
        }
        if (sequenceParameters == "" && argumentsString != "") // if the parameters are empty but the arguments are not
        {
            throw new InvalidOperationException($"Sequence '{node.Name.Name}' expects no parameters but got '{argumentsString}'.");
        }
        if (sequenceParameters != "" && argumentsString == "") // If the parameters are not empty but the arguments are
        {
            throw new InvalidOperationException($"Sequence '{node.Name.Name}' expects parameters of type '{sequenceParameters}' but got no parameters.");
        }

        SequenceCallNode decoratedSequenceCallNode = new SequenceCallNode(node.Name, decoratedArguments, node.SourceLocation);

        return decoratedSequenceCallNode;
    }

    
    public override IASTNode Visit(SequenceNode node)
    {
        // Visit Identifier node.
        string parametersString = "";
        
        // Visit Parameter nodes and create a list of decorated parameters.
        List<ParameterNode> decoratedParameters = new List<ParameterNode>();
        foreach (ParameterNode parameterNode in node.Parameters)
        {
            parametersString += parameterNode.DataType + ", ";
            decoratedParameters.Add((ParameterNode)Visit(parameterNode) ?? throw new InvalidOperationException("Failed to create a decorated parameter node."));
        }
        
        // Add the sequence identifier to the symbol table before entering the sequence scope
        _symbolTable.AddSequence(node.Name.Name, parametersString);

        IdentifierNode decoratedName = node.Name;

        // Enter the scope for the sequence.
        _symbolTable.EnterScope();

        // Visit the SeqBlockNode.
        SeqBlockNode decoratedBlock = (SeqBlockNode?)Visit(node.Block) ?? throw new InvalidOperationException("Failed to create a decorated sequence block node.");

        // Exit the scope for the sequence.
        _symbolTable.ExitScope();

        // Create a new decorated SequenceNode with the decorated nodes and set its SourceLocation.
        SequenceNode decoratedSequenceNode = new SequenceNode(decoratedName, decoratedParameters, decoratedBlock, node.SourceLocation);

        return decoratedSequenceNode;
    }

    
    
    public override IASTNode? Visit(ShapeInitNode node)
    {
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

        return decoratedNode;
    }



    
    public override IASTNode? Visit(PolygonNode node)
    {
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(CircleNode node)
    {
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(TimelineBlockNode node)
    {
        // Visit and type check each frame definition in the timeline block node
        List<FrameDefNode> decoratedFrameDefinitions = new List<FrameDefNode>();

        foreach (var frameDefNode in node.FrameDefinitions)
        {
            FrameDefNode? decoratedFrameDefNode = (FrameDefNode?)Visit(frameDefNode) ?? throw new InvalidOperationException("Failed to create a decorated frame definition node.");
            decoratedFrameDefinitions.Add(decoratedFrameDefNode);
        }

        // Create a new decorated TimelineBlockNode with the type checked frame definitions
        TimelineBlockNode decoratedTimelineBlockNode = new TimelineBlockNode(node.StartTime, node.EndTime, decoratedFrameDefinitions, node.SourceLocation);

        return decoratedTimelineBlockNode;
    }

    public override IASTNode Visit(TransitionNode node)
    {
        // Type check each parameter in the list
        List<IASTNode> decoratedParameters = new List<IASTNode>();
        foreach (var parameter in node.Parameters)
        {
            IASTNode? decoratedParameter;

            if (parameter is ParameterNode parameterNode)
            {
                decoratedParameter = Visit(parameterNode);
            }
            else if (parameter is CallParameterNode callParameterNode)
            {
                decoratedParameter = Visit(callParameterNode);
            }
            else if (parameter is ExpressionNode expressionNode)
            {
                decoratedParameter = Visit(expressionNode);
            }
            else if (parameter is TupleNode tupleNode)
            {
                decoratedParameter = Visit(tupleNode);
            }
            else if (parameter is ArgumentNode argNode)
            {
                decoratedParameter = Visit(argNode);
            }
            else
            {
                throw new InvalidOperationException($"Unexpected parameter type: {parameter.GetType().Name}");
            }

            if (decoratedParameter != null)
            {
                decoratedParameters.Add(decoratedParameter);
            }
            else
            {
                throw new InvalidOperationException("Failed to create a decorated parameter node.");
            }
        }

        TransitionNode decoratedTransitionNode = new TransitionNode(decoratedParameters, node.SourceLocation);

        return decoratedTransitionNode;
    }


    
    public override IASTNode? Visit(TupleNode node)
    {
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(TypeNode node)
    {
        //TypeNode does not have any children, so it can just be returned as is
        return node;
    }

    
    public override IASTNode? Visit(UnaryOperationNode node)
    {
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(NodeList<IASTNode> node)
    {
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
                typeKind = TypeNode.TypeKind.Unknown;
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
            case "Rgb":
                return TypeNode.TypeKind.String;
            case "MoveTo":
                return TypeNode.TypeKind.String;
            case "repeat":
                return TypeNode.TypeKind.Int;
            default:
                return null; // Not a built-in function
        }
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


    public VariableType ConvertStringTypeToVariableType(string type)
    {
        return type.ToLower() switch
        {
            "int" => VariableType.Int,
            "float" => VariableType.Float,
            "string" => VariableType.String,
            "bool" => VariableType.Bool,
            _ => VariableType.Null,
        };
    }

    public string ConvertVariableTypeToString(VariableType variableType)
    {
        return variableType switch
        {
            VariableType.Int => "int",
            VariableType.Float => "float",
            VariableType.String => "string",
            VariableType.Bool => "bool",
            _ => "null",
        };
    }

    
}
