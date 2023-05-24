namespace AnimationLanguage.Visitor;
using ASTCommon;
using ASTNodes;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

public class AnimationLanguageVisitor : AnimationLanguageRulesBaseVisitor<IASTNode>
{
    public override IASTNode VisitProgram(AnimationLanguageRulesParser.ProgramContext context) // This method is called when the parser visits the program node.
    {
        //All of the following variables are created to later be stored as properties of the ProgramNode.
        List<PrototypeNode> prototypeNodes = new List<PrototypeNode>();
        SetupNode? setupNode = null;
        List<FunctionDeclarationNode> functionDeclarations = new List<FunctionDeclarationNode>();
        List<SequenceNode> sequenceNodes = new List<SequenceNode>();
        TimelineBlockNode? timelineBlockNode = null;

        // Visit setup, prototypes, functions, sequences, and timeline blocks, and store the appropriate nodes.
        foreach (var child in context.children)
        {
            if (child is AnimationLanguageRulesParser.SetupBlockContext setupBlockContext)
            {
                setupNode = (SetupNode)VisitSetupBlock(setupBlockContext); // If the child is a setup block, visit it and store the result as the setup node.
            }
            else if (child is AnimationLanguageRulesParser.PrototypeContext prototypeContext)
            {
                prototypeNodes.Add((PrototypeNode)VisitPrototype(prototypeContext));
            }
            else if (child is AnimationLanguageRulesParser.FuncDeclContext funcDeclContext) 
            {
                functionDeclarations.Add((FunctionDeclarationNode)VisitFuncDecl(funcDeclContext)); // If the child is a function declaration, visit it and store the result as a function declaration node.
            }
            else if (child is AnimationLanguageRulesParser.SequencesContext sequencesContext)
            {
                List<SequenceNode> sequenceNodesFromContext = VisitAndGetSequences(sequencesContext); // If the child is a sequence, visit it and store the result as a list of sequence nodes.
                sequenceNodes.AddRange(sequenceNodesFromContext); // Add the sequence nodes from the context to the list of sequence nodes.
            }
            else if (child is AnimationLanguageRulesParser.TimelineBlockContext timelineBlockContext)
            {
                timelineBlockNode = (TimelineBlockNode)VisitTimelineBlock(timelineBlockContext); // If the child is a timeline block, visit it and store the result as the timeline block node.
            }
        }

        // Create a ProgramNode with the collected nodes and set its SourceLocation by defining the start line and start column.
        ProgramNode programNode = new ProgramNode(prototypeNodes, setupNode, functionDeclarations, sequenceNodes, timelineBlockNode, new SourceLocation(context.Start.Line, context.Start.Column));

        return programNode;
    }
    
    
    public override IASTNode VisitSetupBlock(AnimationLanguageRulesParser.SetupBlockContext context)
    {
        // Visit the grouping context and get the GroupingElementsNode.
        GroupingElementsNode groupingElements = (GroupingElementsNode)VisitGroupingElements(context.grouping().groupingElements());

        // Create a SetupNode with the GroupingElementsNode and set its SourceLocation.
        SetupNode setupNode = new SetupNode(groupingElements, new SourceLocation(context.Start.Line, context.Start.Column));
        return setupNode;
    }



    public override IASTNode VisitGroupingElements(AnimationLanguageRulesParser.GroupingElementsContext context)
    {
        IList<ExpressionNode> expressions = new List<ExpressionNode>(); // Create a list of expression nodes.
        IList<IdentifierNode> identifiers = new List<IdentifierNode>(); // Create a list of identifier nodes.
        IList<KeyValuePairNode> keyValuePairs = new List<KeyValuePairNode>(); // Create a list of key value pair nodes.

        foreach (var item in context.children)
        {
            if (item is AnimationLanguageRulesParser.KeyValuePairContext keyValuePairContext) // If the visited item is a key value pair, visit it and add the result to the list of key value pairs.
            {
                KeyValuePairNode keyValuePairNode = VisitKeyValuePair(keyValuePairContext);
                keyValuePairs.Add(keyValuePairNode);
            }
            else if (item is AnimationLanguageRulesParser.ExpressionContext expressionContext) // If the visited item is an expression, visit it and add the result to the list of expressions.
            {
                ExpressionNode expressionNode = (ExpressionNode)VisitExpression(expressionContext);
                expressions.Add(expressionNode);
            }
            else if (item is ITerminalNode terminalNode && terminalNode.Symbol.Type == AnimationLanguageRulesParser.IDENTIFIER) // If the visited item is a terminal node, and it fits the properties of an IDENTIFIER, create an identifier node and add it to the list of identifiers.
            {
                IdentifierNode identifierNode = new IdentifierNode(terminalNode.GetText(), GetSourceLocation(terminalNode.Symbol));
                identifiers.Add(identifierNode);
            }
        }


        GroupingElementsNode groupingElements = new GroupingElementsNode(expressions, identifiers, keyValuePairs, GetSourceLocation(context.Start)); // Create a GroupingElementsNode with the collected nodes and set its SourceLocation.
        return groupingElements;
    }
    

    public override KeyValuePairNode VisitKeyValuePair(AnimationLanguageRulesParser.KeyValuePairContext context)
    {
        // Retrieve the identifier and expression nodes from the context.

        IdentifierNode identifier = (IdentifierNode)Visit(context.IDENTIFIER()); // Visit the IDENTIFIER context and get the IdentifierNode.
        IASTNode expressionNode = Visit(context.expression()); // Visit the expression context and get the IASTNode.

        // Check if the expressionNode is an instance of ExpressionNode or FunctionCallNode.
        if (!(expressionNode is ExpressionNode) && !(expressionNode is FunctionCallNode))
        {
            throw new InvalidOperationException($"Expected an ExpressionNode or FunctionCallNode but got {expressionNode.GetType().Name}");
        }

        // Create a new KeyValuePairNode with the retrieved identifier and expression data.
        KeyValuePairNode keyValuePairNode = new KeyValuePairNode(identifier.Name, (dynamic)expressionNode, GetSourceLocation(context.Start)); //"dynamic" is used to allow the compiler to choose the correct type at runtime.

        return keyValuePairNode;
    }


    
    //VisitTerminal works to create an IdentifierNode when the parser visits an IDENTIFIER terminal node.
    public override IASTNode VisitTerminal(ITerminalNode node)
    {
        if (node.Symbol.Type == AnimationLanguageRulesParser.IDENTIFIER)
        {
            return new IdentifierNode(node.GetText(), GetSourceLocation(node.Symbol));
        }

        return base.VisitTerminal(node);
    }

    
    //This method visits an assignment context and returns an AssignmentNode.
    public override IASTNode VisitAssignment(AnimationLanguageRulesParser.AssignmentContext context)
    {
        AssignmentOperator assignmentOperator = VisitAssOps(context.assOps());
        int identifierChildIndex = context.type() != null ? 1 : 0;
        IdentifierNode identifierNode = (IdentifierNode)Visit(context.GetChild(identifierChildIndex));
        ExpressionNode expression = (ExpressionNode)VisitExpression(context.expression());
        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        // Determine the VariableType for the assignment
        VariableType variableType = VariableType.Null;
        bool isDeclaration = false;
        if (context.type() != null)
        {
            isDeclaration = true; // The assignment is a declaration
            TypeNode.TypeKind typeKind = VisitType(context.type());
            variableType = TypeKindToVariableType(typeKind);
        }
        else
        {
            variableType = expression.VariableType;
        }

        AssignmentNode assignmentNode = new AssignmentNode(
            identifierNode,
            assignmentOperator,
            expression,
            variableType,
            sourceLocation
        )
        {
            IsDeclaration = isDeclaration // Set the IsDeclaration property
        };
        return assignmentNode;
    }



    public TypeNode.TypeKind VisitType(AnimationLanguageRulesParser.TypeContext context)
    {
        if (context.INT() != null)
        {
            return TypeNode.TypeKind.Int;
        }
        else if (context.FLOAT_TYPE() != null)
        {
            return TypeNode.TypeKind.Float;
        }
        else if (context.STRING_TYPE() != null)
        {
            return TypeNode.TypeKind.String;
        }
        else if (context.BOOL() != null)
        {
            return TypeNode.TypeKind.Bool;
        }
        else
        {
            throw new InvalidOperationException($"Unexpected type in VisitType at {GetSourceLocation(context.Start)}");
        }
    }




    //This method visits the assignment operator context and returns the current AssignmentOperator.
    public AssignmentOperator VisitAssOps(AnimationLanguageRulesParser.AssOpsContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context), "Context cannot be null");
        }
        if (context.EQUAL() != null)
        {
            return AssignmentOperator.Assign;
        }
        else if (context.PLUSEQUAL() != null)
        {
            return AssignmentOperator.PlusEqual;
        }
        else if (context.MINUSEQUAL() != null)
        {
            return AssignmentOperator.MinusEqual;
        }
        else
        {
            // TODO: mby return EmptyNode instead of throwing error, instead of throwing errors
            throw new InvalidOperationException("Unexpected assignment operator");
        }
    }



    //This method visits a type context and returns a TypeNode based on what type the context is.
    // public override IASTNode VisitType(AnimationLanguageRulesParser.TypeContext context)
    // {
    //     TypeNode.TypeKind typeKind; //The TypeKind enum is used to determine what types are allowed.
    //
    //     // Check the type of the context and set the corresponding TypeKind.
    //     if (context.INT() != null)
    //     {
    //         typeKind = TypeNode.TypeKind.Int;
    //     }
    //     else if (context.FLOAT_TYPE() != null)
    //     {
    //         typeKind = TypeNode.TypeKind.Float;
    //     }
    //     else if (context.STRING_TYPE() != null)
    //     {
    //         typeKind = TypeNode.TypeKind.String;
    //     }
    //     else if (context.BOOL() != null)
    //     {
    //         typeKind = TypeNode.TypeKind.Bool;
    //     }
    //     else if (context.CIRCLE() != null)
    //     {
    //         typeKind = TypeNode.TypeKind.Circle;
    //     }
    //     else if (context.POLYGON() != null)
    //     {
    //         typeKind = TypeNode.TypeKind.Polygon;
    //     }
    //     else
    //     {
    //         // TODO: mby return EmptyNode instead.
    //         throw new NotSupportedException($"Unsupported type encountered at line {context.Start.Line} column {context.Start.Column}");
    //     }
    //
    //     // Create a new TypeNode with the determined TypeKind and SourceLocation.
    //     TypeNode typeNode = new TypeNode(typeKind, GetSourceLocation(context.Start));
    //
    //     return typeNode;
    // }


    //This is a custom method that visits an expression context and returns an ExpressionNode. Since it is not present in the parser, it does not need to be overridden.
    public IASTNode VisitExpression(AnimationLanguageRulesParser.ExpressionContext context)
    {
        if (context is AnimationLanguageRulesParser.IntegerExpressionContext integerExpressionContext)
        {
            return VisitIntegerExpression(integerExpressionContext);
        }
        else if (context is AnimationLanguageRulesParser.FloatExpressionContext floatExpressionContext)
        {
            return VisitFloatExpression(floatExpressionContext);
        }
        else if (context is AnimationLanguageRulesParser.StringExpressionContext stringExpressionContext)
        {
            return VisitStringExpression(stringExpressionContext);
        }
        else if (context is AnimationLanguageRulesParser.BooleanExpressionContext booleanExpressionContext)
        {
            return VisitBooleanExpression(booleanExpressionContext);
        }
        else if (context is AnimationLanguageRulesParser.IdentifierExpressionContext identifierExpressionContext)
        {
            return VisitTerminal(identifierExpressionContext.IDENTIFIER());
        }
        else if (context is AnimationLanguageRulesParser.FunctionCallExpressionContext functionCallExpressionContext)
        {
            return VisitFuncCall(functionCallExpressionContext.funcCall());
        }
        else if (context is AnimationLanguageRulesParser.ShapeInitExpressionContext shapeInitExpressionContext)
        {
            return VisitShapeinit(shapeInitExpressionContext.shapeinit());
        }
        else if (context is AnimationLanguageRulesParser.BinaryExpressionContext binaryExpressionContext)
        {
            return VisitBinaryExpression(binaryExpressionContext);
        }
        else if (context.GetChild(0) is AnimationLanguageRulesParser.ParenthesizedExpressionContext parenthesizedExpressionContext)
        {
            return VisitParenthesizedExpression(parenthesizedExpressionContext);
        }
        else
        {
            throw new InvalidOperationException($"Unexpected expression type in VisitExpression at {GetSourceLocation(context.Start)}");
        }
    }



    private IASTNode VisitBinaryExpression(AnimationLanguageRulesParser.BinaryExpressionContext context)
    {
        IASTNode leftOperand = Visit(context.expression(0));

        AnimationLanguageRulesParser.ExpressionContext rightExpressionContext = context.expression(1);

        if (rightExpressionContext.Parent is AnimationLanguageRulesParser.ParenthesizedExpressionContext parenthesizedExpressionContext)
        {
            rightExpressionContext = parenthesizedExpressionContext.expression();
        }

        IASTNode rightOperand = VisitExpression(rightExpressionContext);

        OperatorNode operatorNode = (OperatorNode)VisitOperator(context.@operator());
        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        // Add this line to determine the variable type of the binary expression:
        VariableType variableType = DetermineVariableType(leftOperand, operatorNode, rightOperand);
        
        return new ExpressionNode(
            ExpressionNodeType.Binary,
            leftOperand,
            rightOperand,
            operatorNode,
            variableType,
            sourceLocation
        );
    }

    
    //This method determines the variable type of a binary expression.
    private VariableType DetermineVariableType(IASTNode leftOperand, OperatorNode operatorNode, IASTNode rightOperand)
    {
        if (leftOperand is ExpressionNode leftExpression && rightOperand is ExpressionNode rightExpression)
        {
            if (leftExpression.VariableType == rightExpression.VariableType)
            {
                return leftExpression.VariableType;
            }
        }

        // If the types do not match or cannot be determined, return Null.
        return VariableType.Null;
    }

    

    public override IASTNode VisitOperator(AnimationLanguageRulesParser.OperatorContext operatorContext)
    {
        if (operatorContext.logicOpp() != null)
        {
            return VisitLogicOpp(operatorContext.logicOpp());
        }

        string operatorString = operatorContext.GetText();
        OperatorNode operatorNode = new OperatorNode(operatorString, GetSourceLocation(operatorContext.Start));
        return operatorNode;
    }





    public override IASTNode VisitIntegerExpression(AnimationLanguageRulesParser.IntegerExpressionContext context)
    {
        int value = int.Parse(context.INTEGER().GetText());

        if (context.MINUS() != null)
        {
            value = -value;
        }
        
        return new IntegerLiteralNode(value, GetSourceLocation(context.Start)); //The sourcelocation is retrieved from the context.
    }


    public override IASTNode VisitFloatExpression(AnimationLanguageRulesParser.FloatExpressionContext context)
    {
        float value = float.Parse(context.FLOAT().GetText());
        
        if (context.MINUS() != null)
        {
            value = -value;
        }
        
        return new FloatLiteralNode(value, GetSourceLocation(context.Start));
    }

    public override IASTNode VisitStringExpression(AnimationLanguageRulesParser.StringExpressionContext context)
    {
        string value = context.STRING().GetText();
        value = value.Substring(1, value.Length - 2); // Removes the quotes surrounding the string, suchh that it is only the relevant data we gather
        return new StringLiteralNode(value, GetSourceLocation(context.Start)); 
    }

    public override IASTNode VisitBooleanExpression(AnimationLanguageRulesParser.BooleanExpressionContext context)
    {
        if(context.boolean().TRUE() != null)
        {
            return new BooleanLiteralNode(true, GetSourceLocation(context.Start));
        }
        else if(context.boolean().FALSE() != null)
        {
            return new BooleanLiteralNode(false, GetSourceLocation(context.Start));
        }
        else
        {
            throw new InvalidOperationException($"Unexpected boolean type in VisitBooleanExpression at {GetSourceLocation(context.Start)}");
        }
    }

    //This method is called when a function call is encountered in the code.
    public override IASTNode VisitFuncCall(AnimationLanguageRulesParser.FuncCallContext context)
    {
        IdentifierNode identifier = new IdentifierNode(context.IDENTIFIER().GetText(), GetSourceLocation(context.IDENTIFIER().Symbol)); //Create a new IdentifierNode with the name of the function and the SourceLocation of the IDENTIFIER terminal.

        List<IASTNode> arguments = new List<IASTNode>(); //Create a new list of IASTNodes to store the arguments of the function call.
        if (context.funcArgs() != null)
        {
            AnimationLanguageRulesParser.FuncArgsContext funcArgsContext = context.funcArgs();

            foreach (var child in funcArgsContext.children)
            {
                if (child.GetType() == typeof(AnimationLanguageRulesParser.IntegerExpressionContext))
                {
                    arguments.Add(VisitIntegerExpression((AnimationLanguageRulesParser.IntegerExpressionContext)child));
                }
                else if(child.GetType() == typeof(AnimationLanguageRulesParser.FunctionCallExpressionContext))
                {
                    arguments.Add(VisitFunctionCallExpression((AnimationLanguageRulesParser.FunctionCallExpressionContext)child));
                }

                else if (child.GetType() == typeof(AnimationLanguageRulesParser.IdentifierExpressionContext))
                {
                    arguments.Add(VisitIdentifierExpression((AnimationLanguageRulesParser.IdentifierExpressionContext)child));
                }
            }
        }

        FunctionCallNode functionCall = new FunctionCallNode(identifier, arguments, GetSourceLocation(context.Start));
        ExpressionNode decoratedFunctionCall = (ExpressionNode)functionCall;
        decoratedFunctionCall.Identifier = identifier;
        return (ExpressionNode)functionCall;
    }
    

    //This represents a shape initialization.
    public override IASTNode VisitShapeinit(AnimationLanguageRulesParser.ShapeinitContext context)
    {
        //If the shape is a circle, create a new TypeNode with the TypeKind Circle, otherwise create a new TypeNode with the TypeKind Polygon.
        TypeNode shapeType = context.CIRCLE() != null ? new TypeNode(TypeNode.TypeKind.Circle, GetSourceLocation(context.CIRCLE().Symbol)) : new TypeNode(TypeNode.TypeKind.Polygon, GetSourceLocation(context.POLYGON().Symbol)); 
        Dictionary<string, IASTNode> arguments = new Dictionary<string, IASTNode>(); //Create a dictionary to store the arguments of the shape initialization for either a circle or a polygon.

        for (int i = 0; i < context.argName().Length; i++)
        {
            string argName = context.argName(i).GetText().TrimEnd(':'); //Get the name of the argument and remove the colon at the end.
            IASTNode argValue = Visit(context.arg(i)); //Visit the argument and store the value of the argument in a variable.
            arguments.Add(argName, argValue);
        }

        SourceLocation sourceLocation = GetSourceLocation(context.Start);
        return new ShapeInitNode(shapeType, arguments, sourceLocation);
    }


    public override IASTNode VisitTuple(AnimationLanguageRulesParser.TupleContext context)
    {
        Dictionary<string, IASTNode> arguments = new Dictionary<string, IASTNode>(); // Create a dictionary to store the tuple arguments.

        for (int i = 0; i < context.argName().Length; i++) // Loop through the arguments of the tuple. argName.Length retrieves the amount of times an argument rule appears in the tuple context
        {
            string argName = context.argName(i).GetText().TrimEnd(':'); // Get the name of the argument and remove the colon at the end.
            IASTNode argValue = Visit(context.arg(i)); // Visit the argument and store the value of the argument in a variable.
            arguments.Add(argName, argValue);
        }

        SourceLocation sourceLocation = GetSourceLocation(context.Start);
        return new TupleNode(arguments, sourceLocation);
    }
    
    
    //This method visits the individual prototype rule.
    public override IASTNode VisitPrototype(AnimationLanguageRulesParser.PrototypeContext context)
    {
        if (context.type() == null)
        {
            Console.WriteLine("Prototype type context is null");
        }

        DataType returnType = GetDataTypeFromTypeContext(context.type());
        string functionName = context.IDENTIFIER().GetText(); // Get the name of the function.
        SourceLocation sourceLocation = GetSourceLocation(context.Start);
        NodeList<ParameterNode> parameters = new NodeList<ParameterNode>(new List<ParameterNode>(), sourceLocation); // Create a NodeList of ParameterNodes with an empty list and source location.

        if (context.parameters() != null) // If the prototype has parameters, visit them and add them to the list.
        {
            List<ParameterNode> visitedParameters = (List<ParameterNode>)VisitParameters(context.parameters());
            parameters = new NodeList<ParameterNode>(visitedParameters, sourceLocation);
        }

        return new PrototypeNode(returnType, functionName, parameters, sourceLocation);
    }


    public override IASTNode VisitParenthesizedExpression(AnimationLanguageRulesParser.ParenthesizedExpressionContext context)
    {
        IASTNode innerExpression = VisitExpression(context.expression());
        return innerExpression;
    }








    //This method is used to return the DataType of a type context.
    private DataType GetDataTypeFromTypeContext(AnimationLanguageRulesParser.TypeContext context)
    {
        if (context == null)
        {
            throw new NotSupportedException($"Type '{context?.GetText()}' is not supported.");
        }
        switch (context.GetText())
        {
            case "int":
                return DataType.Int;
            case "float":
                return DataType.Float;
            case "string":
                return DataType.String;
            case "bool":
                return DataType.Bool;
            case "Circle":
                return DataType.Circle;
            case "Polygon":
                return DataType.Polygon;
            case "group": 
                return DataType.Group;
            default:
                throw new NotSupportedException($"Type '{context.GetText()}' is not supported.");
        }
    }

    
    //This method is called when the parameters rule is encountered in the code.
    public IList<ParameterNode> VisitParameters(AnimationLanguageRulesParser.ParametersContext context)
    {
        List<ParameterNode> parameterNodes = new List<ParameterNode>();

        foreach (var parameterContext in context.GetRuleContexts<AnimationLanguageRulesParser.ParameterContext>())
        {
            ParameterNode parameterNode = (ParameterNode)Visit(parameterContext);
            if (parameterNode != null)
            {
                parameterNodes.Add(parameterNode);
            }
        }

        return parameterNodes;
    }


    //This method is called when the parameter rule is encountered in the code.
    public override IASTNode VisitParameter(AnimationLanguageRulesParser.ParameterContext context)
    {
        if (context.type() == null)
        {
            Console.WriteLine("Parameter type context is null");
        }
    
        DataType dataType = GetDataTypeFromTypeContext(context.type());
        
        string parameterName = context.IDENTIFIER().GetText(); //Get the namew of the parameter.
        SourceLocation sourceLocation = GetSourceLocation(context.Start); 

        return new ParameterNode(dataType, parameterName, sourceLocation);
    }


    // This method is called when a function is being declared in the code
    public override IASTNode VisitFuncDecl(AnimationLanguageRulesParser.FuncDeclContext context)
    {
        string functionName = context.IDENTIFIER().GetText();
        IdentifierNode identifierNode = new IdentifierNode(functionName, GetSourceLocation(context.IDENTIFIER().Symbol));

        IList<ParameterNode> parameters = new List<ParameterNode>();
        if (context.parameters() != null)
        {
            parameters = VisitParameters(context.parameters());
        }

        TypeNode.TypeKind returnTypeKind = GetTypeFromString(context.GetChild(0).GetText());
        SourceLocation returnTypeLocation = GetSourceLocation(context.Start);
        TypeNode returnTypeNode = new TypeNode(returnTypeKind, returnTypeLocation);

        BlockNode blockNode = (BlockNode)VisitBlock(context.block());
        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        return new FunctionDeclarationNode(returnTypeNode, identifierNode, parameters, blockNode, sourceLocation);
    }




    
    // this method is being called when a block is being declared in the code.
    public override IASTNode VisitBlock(AnimationLanguageRulesParser.BlockContext context)
    {
        IList<StatementNode> statementNodes = new List<StatementNode>(); //Create a list of StatementNodes to store the statements of the block.
        ReturnNode returnNode = new ReturnNode(null, GetSourceLocation(context.Start)); //Create a return node with a null value and the source location of the block.
        if (context.statements() != null) 
        {
            statementNodes = VisitStatements(context.statements()); //Visit the statements of the block and add them to the list.
        }

        if (context.@return() != null)
        {
            returnNode = (ReturnNode)VisitReturn(context.@return()); //Visit the return statement of the block and store it in a variable.
        }

        BlockNode blockNode = new BlockNode(statementNodes, returnNode, GetSourceLocation(context.Start)); //Create a block node with the list of statements and the source location of the block
        return blockNode;
    }


    // The VisitStatements method is called when the statements rule is encountered in the code.
    public IList<StatementNode> VisitStatements(AnimationLanguageRulesParser.StatementsContext context)
    {
        List<StatementNode> statementNodes = new List<StatementNode>(); //Create a list of StatementNodes to store the statements.

        foreach (var statementContext in context.GetRuleContexts<AnimationLanguageRulesParser.StatementContext>()) 
        {
            StatementNode statementNode = (StatementNode)Visit(statementContext); 
            if (statementNode != null)
            {
                statementNodes.Add(statementNode); //Visit each statement and add it to the list.
            }
        }
        return statementNodes;
    }


    // this method visits the individual statement rule.
    public override IASTNode VisitStatement(AnimationLanguageRulesParser.StatementContext context)
    {
        if (context.assignment() != null) //If the statement is an assignment, visit it
        {
            return Visit(context.assignment());
        }
        else if (context.identifierGrouping() != null)  //If the statement is an identifier, visit the following grouping
        {
            return Visit(context.identifierGrouping());
        }
        else if (context.loop() != null) //If the statement is a loop, visit it
        {
            return Visit(context.loop());
        }
        else if (context.conditional() != null) //If the statement is a conditional, visit it
        {
            return Visit(context.conditional());
        }
        else
        {
            throw new InvalidOperationException($"Unrecognized statement at {GetSourceLocation(context.Start)}");
        }
    }

    //The VisitIdentifierGrouping method is called when the identifierGrouping rule is encountered in the code.
    public override IASTNode VisitIdentifierGrouping(AnimationLanguageRulesParser.IdentifierGroupingContext context)
    {
        IdentifierNode identifier = new IdentifierNode(context.IDENTIFIER().GetText(), GetSourceLocation(context.IDENTIFIER().Symbol)); //Create an identifier node for the identifier.
        GroupingElementsNode groupingElements = (GroupingElementsNode)VisitGroupingElements(context.groupingElements()); //Visit the grouping elements of the identifier.
        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        return new IdentifierGroupingNode(identifier, groupingElements, sourceLocation);
    }

    // This method is called when a return statement is encountered in the code.
    public override IASTNode VisitReturn(AnimationLanguageRulesParser.ReturnContext context)
    {
        SourceLocation sourceLocation = GetSourceLocation(context.Start);
        ExpressionNode? returnExpression = null;

        if (context.expression() != null)
        {
            returnExpression = Visit(context.expression()) as ExpressionNode;
        }

        if (context.grouping() != null)
        {
            // Visit the grouping context and get the GroupingElementsNode.
            GroupingElementsNode groupingElements = (GroupingElementsNode)VisitGroupingElements(context.grouping().groupingElements());
            return new ReturnNode(groupingElements, sourceLocation);
        }

        
        return new ReturnNode(returnExpression, sourceLocation);
    }


    
    //VisitLoop is a method used to handle the loop rule in the code.
    public override IASTNode VisitLoop(AnimationLanguageRulesParser.LoopContext context)
    {
        if (context.for_loop() != null) //If the loop is a for loop, visit it.
        {
            return VisitFor_loop(context.for_loop());
        }
        else if (context.while_loop() != null)
        {
            return VisitWhile_loop(context.while_loop()); //If the loop is a while loop, visit it.
        }
        else
        {
            throw new InvalidOperationException($"Unrecognized loop type at {GetSourceLocation(context.Start)}");
        }
    }

    //This method is called when a for loop is encountered in the code.
    public override IASTNode VisitFor_loop(AnimationLanguageRulesParser.For_loopContext context)
    {
        AssignmentNode startExpression = (AssignmentNode)VisitAssignment(context.assignment(0));
        ExpressionNode condition = (ExpressionNode)VisitExpression(context.expression());
        IASTNode endExpression;

        if (context.assignment().Length == 2)
        {
            endExpression = (AssignmentNode)VisitAssignment(context.assignment(1));
        }
        else
        {
            endExpression = (UnaryOperationNode)Visit(context.unaryOperation());
        }

        BlockNode block = (BlockNode)VisitBlock(context.block());
        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        return new ForLoopNode(startExpression, condition, endExpression, block, sourceLocation);
    }


    public override IASTNode VisitUnaryOperation(AnimationLanguageRulesParser.UnaryOperationContext context)
    {
        UnaryOperator unaryOperator = GetUnaryOperator(context.unary());
        int identifierChildIndex = context.unary().Start.Text == context.GetText() ? 1 : 0;
        IdentifierNode identifierNode = (IdentifierNode)Visit(context.GetChild(identifierChildIndex));
        UnaryOperationNode unaryOperationNode = new UnaryOperationNode(identifierNode, unaryOperator, GetSourceLocation(context.Start));
        return unaryOperationNode;
    }
    
    
    public override IASTNode VisitUnary(AnimationLanguageRulesParser.UnaryContext context)
    {
        return VisitChildren(context);
    }
    
    
    
    //This method in run when a while loop is met in the code.
    public override IASTNode VisitWhile_loop(AnimationLanguageRulesParser.While_loopContext context)
    {
        ExpressionNode condition = (ExpressionNode)VisitExpression(context.expression());
        BlockNode body = (BlockNode)VisitBlock(context.block());
        SourceLocation sourceLocation = GetSourceLocation(context.Start);
        
        return new WhileLoopNode(condition, body, sourceLocation);
    }
    

    
    //Visits the comparator operator of a condition eg: '==', '<='.
    public override IASTNode VisitComparator(AnimationLanguageRulesParser.ComparatorContext context)
    {
        string operatorSymbol = context.GetText();
        SourceLocation sourceLocation = GetSourceLocation(context.Start);
    
        OperatorNode operatorNode = new OperatorNode(operatorSymbol, sourceLocation);
    
        return operatorNode;
    }

    
    //Visits the logical operator of a condition eg: 'and', 'or'
    public override IASTNode VisitLogicOpp(AnimationLanguageRulesParser.LogicOppContext context)
    {
        string operatorSymbol = context.GetText();
        SourceLocation sourceLocation = GetSourceLocation(context.Start);
    
        OperatorNode operatorNode = new OperatorNode(operatorSymbol, sourceLocation);
    
        return operatorNode;
    }
    

    //This method is called when a conditional is encountered in the code.
    public override IASTNode VisitConditional(AnimationLanguageRulesParser.ConditionalContext context)
    {
        ExpressionNode condition = (ExpressionNode)VisitExpression(context.expression());
        BlockNode ifBlock = (BlockNode)VisitBlock(context.block());

        IList<ElseIfNode> elseIfBranches = new List<ElseIfNode>();
        for (int i = 0; i < context.elseif().Length; i++)
        {
            elseIfBranches.Add((ElseIfNode)VisitElseif(context.elseif(i)));
        }

        ElseNode? elseBranch = null;
        if (context.@else() != null)
        {
            elseBranch = (ElseNode)VisitElse(context.@else());
        }
        
        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        return new IfStatementNode(condition, ifBlock, elseIfBranches, elseBranch, sourceLocation);
    }




    //This method is called when an else if branch is encountered in the code.
    public override IASTNode VisitElseif(AnimationLanguageRulesParser.ElseifContext context)
    {
        ExpressionNode condition = (ExpressionNode)VisitExpression(context.expression());
        BlockNode block = (BlockNode)VisitBlock(context.block());
        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        return new ElseIfNode(condition, block, sourceLocation);
    }


    //This method is called when an else branch is encountered in the code.
    public override IASTNode VisitElse(AnimationLanguageRulesParser.ElseContext context)
    {
        BlockNode block = (BlockNode)VisitBlock(context.block()); //Visit the block of the else branch.
        SourceLocation sourceLocation = GetSourceLocation(context.Start); //Get the source location of the else branch.

        return new ElseNode(block, sourceLocation);
    }

    
    //This method is called when the sequences rule is encountered in the code.
    
    
    
    // public override IASTNode VisitSequences(AnimationLanguageRulesParser.SequencesContext context)
    // {
    //     List<SequenceNode> sequences = new List<SequenceNode>();
    //
    //     foreach (var sequenceContext in context.sequence())
    //     {
    //         SequenceNode sequenceNode = (SequenceNode)VisitSequence(sequenceContext);
    //         sequences.Add(sequenceNode);
    //     }
    //
    //     return new SequencesNode(sequences, GetSourceLocation(context.Start));
    // }
    
    public List<SequenceNode> VisitAndGetSequences(AnimationLanguageRulesParser.SequencesContext context) 
    {
        List<SequenceNode> sequenceNodes = new List<SequenceNode>();

        // Visit the first sequence
        SequenceNode firstSequence = (SequenceNode)VisitSequence(context.sequence());
        sequenceNodes.Add(firstSequence);

        // If there are more sequences, visit them as well
        if (context.sequences() != null)
        {
            List<SequenceNode> otherSequences = VisitAndGetSequences(context.sequences());
            sequenceNodes.AddRange(otherSequences);
        }

        return sequenceNodes;
    }
    
    // public List<SequenceNode> VisitAndGetSequences(AnimationLanguageRulesParser.SequencesContext context)
    // {
    //     var sequences = new List<SequenceNode>(); // Create a list to contain all of the individual sequences.
    //     
    //     
    //     
    //     foreach (var child in context.children) // For each child of the sequences rule, visit it.
    //     {
    //
    //         IASTNode node = Visit(child);
    //         if (node is SequenceNode sequenceNode) 
    //         {
    //             sequences.Add(sequenceNode); // If the child is a sequence, visit it and add it to the list.
    //         }
    //     }
    //     return sequences;
    // }



    //this method in run when a sequence is met in the code.
    public override IASTNode VisitSequence(AnimationLanguageRulesParser.SequenceContext context)
    {
        IdentifierNode name = new IdentifierNode(context.IDENTIFIER().GetText(), GetSourceLocation(context.IDENTIFIER().Symbol));

        List<ParameterNode> parameters = new List<ParameterNode>();
        if (context.parameters() != null)
        {
            foreach (var param in context.parameters().parameter())
            {
                ParameterNode paramNode = (ParameterNode)VisitParameter(param);
                parameters.Add(paramNode);
            }
        }

        SeqBlockNode block = (SeqBlockNode)VisitSeqBlock(context.seqBlock());

        SequenceNode sequence = new SequenceNode(name, parameters, block, GetSourceLocation(context.Start));
        return sequence;
    }


    //This method is called when a sequence block is encountered in the code.
    public override IASTNode VisitSeqBlock(AnimationLanguageRulesParser.SeqBlockContext context)
    {
        List<StatementNode> statements = new List<StatementNode>();
        List<AnimationNode> animations = new List<AnimationNode>();

        var seqBlockElements = context.seqBlockPart(); // Get all of the children of the sequence block rule.
    
        foreach (var seqBlockElement in seqBlockElements)
        {
            if (seqBlockElement.statement() != null)
            {
                StatementNode statementNode = (StatementNode)VisitStatement(seqBlockElement.statement());
                statements.Add(statementNode);
            }
            else if (seqBlockElement.animation() != null)
            {
                animations.Add((AnimationNode)VisitAnimation(seqBlockElement.animation()));
            }
        }

        SeqBlockNode seqBlock = new SeqBlockNode(statements, animations, GetSourceLocation(context.Start));
        return seqBlock;
    }






//This method is called when a sequence block part is encountered in the code.
    // public List<IASTNode> VisitSeqBlockPart(AnimationLanguageRulesParser.SeqBlockPartsContext context)
    // {
    //     var childrenNodes = new List<IASTNode>();
    //
    //     foreach (var child in context.children)
    //     {
    //         if (child == null)
    //         {
    //             Console.WriteLine("Child context is null");
    //         }
    //         else
    //         {
    //             if (child.GetType() == typeof(AnimationLanguageRulesParser.StatementContext))
    //             {
    //                 Console.WriteLine("Statement context " + child.GetText());
    //                 childrenNodes.Add(VisitStatement((AnimationLanguageRulesParser.StatementContext)child));
    //             }
    //             else if (child.GetType() == typeof(AnimationLanguageRulesParser.AnimationContext))
    //             {
    //                 Console.WriteLine("Animation context " + child.GetText());
    //                 childrenNodes.Add(VisitAnimation((AnimationLanguageRulesParser.AnimationContext)child));
    //             }
    //         }
    //     }
    //
    //     return childrenNodes;
    // }



    //This method is called when a sequence call is encountered in the code.
    public override IASTNode VisitSequenceCall(AnimationLanguageRulesParser.SequenceCallContext context)
    {
        IdentifierNode name = new IdentifierNode(context.IDENTIFIER().GetText(), GetSourceLocation(context.IDENTIFIER().Symbol));

        List<ExpressionNode> arguments = new List<ExpressionNode>();
        if (context.call_parameter() != null)
        {
            var callParameterContexts = context.call_parameter();
            foreach (var arg in callParameterContexts)
            {
                if (arg.arg() != null)
                {
                    ExpressionNode argNode = (ExpressionNode)VisitArg(arg.arg());
                    arguments.Add(argNode);
                }
                else if (arg.argName() != null && arg.arg() != null)
                {
                    // Handle named arguments if needed.
                }
            }
        }
        SequenceCallNode sequenceCall = new SequenceCallNode(name, arguments, GetSourceLocation(context.Start));
        return sequenceCall;
    }
    
    
    public override IASTNode VisitArg(AnimationLanguageRulesParser.ArgContext context)
    {
        if (context.argName() != null)
        {
            string argName = context.argName().IDENTIFIER().GetText();
        
            if (context.expression() != null)
            {
                return new KeyValuePairNode(argName, VisitExpression(context.expression()), GetSourceLocation(context.Start));
            }
            else if (context.IDENTIFIER() != null)
            {
                return new KeyValuePairNode(argName, new IdentifierNode(context.IDENTIFIER().GetText(), GetSourceLocation(context.IDENTIFIER().Symbol)), GetSourceLocation(context.Start));
            }
            else if (context.tuple() != null)
            {
                return new KeyValuePairNode(argName, VisitTuple(context.tuple()), GetSourceLocation(context.Start));
            }
        }
        else
        {
            if (context.expression() != null)
            {
                return VisitExpression(context.expression());
            }
            else if (context.IDENTIFIER() != null)
            {
                return new IdentifierNode(context.IDENTIFIER().GetText(), GetSourceLocation(context.IDENTIFIER().Symbol));
            }
            else if (context.tuple() != null)
            {
                return VisitTuple(context.tuple());
            }
        }
        
        throw new InvalidOperationException($"Unrecognized argument at {GetSourceLocation(context.Start)}");
    }



    //This method is called when an animation is encountered in the code.
    public override IASTNode VisitAnimation(AnimationLanguageRulesParser.AnimationContext context)
    {
        var identifierNode = (IdentifierNode)VisitTerminal(context.IDENTIFIER()); // Visit the identifier of the animation.
        CommandNode? commandNode = null; // Create a variable to contain the command of the animation. It's nullable, as an animation can both have a command, and not have a command.

        var transitions = new List<TransitionNode>();
        var commands = new List<CommandNode>();

        foreach (var animationPartContext in context.animationPart())
        {
            var (transitionNodes, commandNodes) = VisitAndGetTransitionsFromAnimationPart(animationPartContext);
            transitions.AddRange(transitionNodes);
            commands.AddRange(commandNodes);

            // Find commandNode if it exists
            if (commandNode == null && commandNodes.Count > 0)
            {
                commandNode = commandNodes.First();
            }
        }

        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        return new AnimationNode(identifierNode, commandNode, transitions, sourceLocation);
    }



    public override IASTNode VisitCall_parameters(AnimationLanguageRulesParser.Call_parametersContext context)
    {
        List<IASTNode> arguments = new List<IASTNode>();
        
        foreach(var child in context.children)
        {
            if (child is AnimationLanguageRulesParser.Call_parameterContext callParameterContext)
            {
                IASTNode callParameterNode = VisitCall_parameter(callParameterContext);
                arguments.Add(callParameterNode);
            }
        }
        
        return new CallParameterNode(arguments, GetSourceLocation(context.Start));
    }

    
    public override IASTNode VisitCall_parameter(AnimationLanguageRulesParser.Call_parameterContext context)
    {
        if (context.argName() != null && context.arg() != null)
        {
            // Handle named arguments if needed.
            string argName = context.argName().GetText();
            IASTNode argValue = VisitArg(context.arg());
            return new ArgumentNode(argName, argValue, GetSourceLocation(context.Start));
        }
        else
        {
            IASTNode argValue = VisitArg(context.arg());
            return argValue;
        }
    }



    public (List<TransitionNode>, List<CommandNode>) VisitAndGetTransitions(AnimationLanguageRulesParser.SeqBlockPartContext context)
{
    var transitions = new List<TransitionNode>();
    var commands = new List<CommandNode>();

    foreach (var child in context.children)
    {
        if (child is AnimationLanguageRulesParser.TransitionContext transitionContext)
        {
            transitions.Add((TransitionNode)VisitTransition(transitionContext));
        }
        else if (child is AnimationLanguageRulesParser.CommandContext commandContext)
        {
            commands.Add((CommandNode)VisitCommand(commandContext));
        }
    }

    return (transitions, commands);
}



    public override IASTNode VisitTransition(AnimationLanguageRulesParser.TransitionContext context)
    {
        List<IASTNode> arguments = new List<IASTNode>();

        var callParameterContexts = context.GetRuleContexts<AnimationLanguageRulesParser.Call_parameterContext>();

        foreach (var callParameterContext in callParameterContexts)
        {
            IASTNode callParameterNode = VisitCall_parameter(callParameterContext);
            arguments.Add(callParameterNode);
        }

        return new TransitionNode(arguments, GetSourceLocation(context.Start));
    }

    
    
    //This method is called when a command is encountered in the code.
    public override IASTNode VisitCommand(AnimationLanguageRulesParser.CommandContext context)
    {
        IdentifierNode identifierNode = (IdentifierNode)VisitTerminal(context.IDENTIFIER());

        IList<IASTNode> parameters = new List<IASTNode>();
        if (context.call_parameters() != null)
        {
            CallParameterNode visitedParametersNode = (CallParameterNode)Visit(context.call_parameters());
            parameters = new List<IASTNode>(visitedParametersNode.Children);
        }

        CommandNode commandNode = new CommandNode(identifierNode, parameters, new SourceLocation(context.Start.Line, context.Start.Column));
        return commandNode;
    }



    //This method is called when a TimelineBlock is encountered in the code. TODO: maybe rewrite this method
    public override IASTNode VisitTimelineBlock(AnimationLanguageRulesParser.TimelineBlockContext context)
    {
        int startTime = 0; // Create a variable to contain the start time of the timeline block
        int endTime = 0; // Create a variable to contain the end time of the timeline block
        IList<FrameDefNode> frameDefinitions = new List<FrameDefNode>(); // Create a list to contain the frame definitions of the timeline block

        foreach (var frameDefContext in context.frameDef())
        {
            FrameDefNode frameDefNode = (FrameDefNode)VisitFrameDef(frameDefContext); // Visit the frame definition
            frameDefinitions.Add(frameDefNode); // Add the visited frame definition to the list of frame definitions

            int frameTime = int.Parse(frameDefContext.INTEGER().GetText()); // Get the time of the frame definition
            startTime = Math.Min(startTime, frameTime); // Update the start time of the timeline block
            endTime = Math.Max(endTime, frameTime); // Update the end time of the timeline block
        }

        TimelineBlockNode timelineBlockNode = new TimelineBlockNode(startTime, endTime, frameDefinitions, new SourceLocation(context.Start.Line, context.Start.Column)); // Create a timeline block node

        return timelineBlockNode;
    }


    //This method is called when a frame definition is encountered in the code.
    public override IASTNode VisitFrameDef(AnimationLanguageRulesParser.FrameDefContext context)
    {
        int frameTime = int.Parse(context.INTEGER().GetText());// Get the time of the frame definition
        SequenceCallNode sequenceCallNode = (SequenceCallNode)VisitSequenceCall(context.sequenceCall()); // Visit the sequence call of the frame definition

        FrameDefNode frameDefNode = new FrameDefNode(frameTime, sequenceCallNode, GetSourceLocation(context.INTEGER().Symbol));

        return frameDefNode;
    }

    

    //---------------------------Helper methods---------------------------//
    private SourceLocation GetSourceLocation(IToken token)
    {
        return new SourceLocation(token.Line, token.Column);
    }
    
    //TODO: Add all of the remaning helper methods that do not override a method from the base class here
    
    
    private TypeNode.TypeKind GetTypeFromString(string typeString)
    {
        return typeString.ToLower() switch
        {
            "int" => TypeNode.TypeKind.Int,
            "float" => TypeNode.TypeKind.Float,
            "string" => TypeNode.TypeKind.String,
            "bool" => TypeNode.TypeKind.Bool,
            "circle" => TypeNode.TypeKind.Circle,
            "polygon" => TypeNode.TypeKind.Polygon,
            "group" => TypeNode.TypeKind.Group,
            _ => throw new InvalidOperationException($"Invalid type string: {typeString}")
        };
    }
    
    
    private UnaryOperator GetUnaryOperator(AnimationLanguageRulesParser.UnaryContext context)
    {
        if (context.INC() != null)
        {
            return UnaryOperator.Increment;
        }
        else if (context.DEC() != null)
        {
            return UnaryOperator.Decrement;
        }
        else
        {
            throw new NotSupportedException($"Unsupported unary operator: {context.GetText()}");
        }
    }
    
    
    public (List<TransitionNode>, List<CommandNode>) VisitAndGetTransitionsFromAnimationPart(AnimationLanguageRulesParser.AnimationPartContext context)
    {
        var transitions = new List<TransitionNode>();
        var commands = new List<CommandNode>();

        if (context.transition() != null)
        {
            transitions.Add((TransitionNode)VisitTransition(context.transition()));
        }
        else if (context.command() != null)
        {
            commands.Add((CommandNode)VisitCommand(context.command()));
        }

        return (transitions, commands);
    }
    
    
    private VariableType TypeKindToVariableType(TypeNode.TypeKind typeKind)
    {
        return typeKind switch
        {
            TypeNode.TypeKind.Int => VariableType.Int,
            TypeNode.TypeKind.Float => VariableType.Float,
            TypeNode.TypeKind.String => VariableType.String,
            TypeNode.TypeKind.Bool => VariableType.Bool,
            _ => VariableType.Null
        };
    }

}




