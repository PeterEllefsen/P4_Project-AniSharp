using AnimationLanguage.ASTCommon;
using AnimationLanguage.ASTNodes;
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
            else if (child is AnimationLanguageRulesParser.PrototypesContext prototypesContext)
            {
                NodeList<IASTNode> prototypeNodeList = (NodeList<IASTNode>)VisitPrototypes(prototypesContext); // If the child is a prototype, visit it and store the result as a list of IASTNodes.
                foreach (var node in prototypeNodeList)
                {
                    if (node is PrototypeNode prototypeNode)
                    {
                        prototypeNodes.Add(prototypeNode); // Add the prototype node to the list of prototype nodes if it is a prototype
                    }
                }
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
        GroupingElementsNode groupingElements = (GroupingElementsNode)VisitGrouping(context.grouping());

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
    
        if (!(expressionNode is ExpressionNode expression))
        {
            throw new InvalidOperationException($"Expected an ExpressionNode but got {expressionNode.GetType().Name}");
        }

        // Create a new KeyValuePairNode with the retrieved identifier and expression data.
        KeyValuePairNode keyValuePairNode = new KeyValuePairNode(identifier, expression, GetSourceLocation(context.Start));

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
        IdentifierNode identifierNode = (IdentifierNode)Visit(context.GetChild(0)); // Visit the first child of the context and get the IdentifierNode
        ExpressionNode expression = (ExpressionNode)VisitExpression(context.expression()); // Call the custom VisitExpression method
        AssignmentNode assignmentNode = new AssignmentNode(identifierNode, assignmentOperator, expression, GetSourceLocation(context.Start));
        return assignmentNode;
    }


    //This method visits the assignment operator context and returns the current AssignmentOperator.
    public AssignmentOperator VisitAssOps(AnimationLanguageRulesParser.AssOpsContext context)
    {
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
        //Mby convert this code to a switch??
        if (context is AnimationLanguageRulesParser.IntegerExpressionContext)
        {
            return VisitIntegerExpression((AnimationLanguageRulesParser.IntegerExpressionContext)context); //Visit the IntegerExpressionContext and return the result.
        }
        else if (context is AnimationLanguageRulesParser.FloatExpressionContext)
        {
            return VisitFloatExpression((AnimationLanguageRulesParser.FloatExpressionContext)context); //Visit the FloatExpressionContext and return the result.
        }
        else if (context is AnimationLanguageRulesParser.StringExpressionContext)
        {
            return VisitStringExpression((AnimationLanguageRulesParser.StringExpressionContext)context); //Visit the BooleanExpressionContext and return the result.
        }
        else if (context is AnimationLanguageRulesParser.BooleanExpressionContext)
        {
            return VisitBooleanExpression((AnimationLanguageRulesParser.BooleanExpressionContext)context); //Visit the BooleanExpressionContext and return the result.
        }
        else if (context is AnimationLanguageRulesParser.BinaryExpressionContext)
        {
            return VisitBinaryExpression((AnimationLanguageRulesParser.BinaryExpressionContext)context); //Visit the BinaryExpressionContext and return the result.
        }
        else if (context is AnimationLanguageRulesParser.IdentifierExpressionContext)
        {
            ITerminalNode identifierNode = ((AnimationLanguageRulesParser.IdentifierExpressionContext)context).IDENTIFIER(); //Store the IDENTIFIER terminal in a variable.
            return VisitTerminal(identifierNode); // Call VisitTerminal with the IDENTIFIER terminal
        }
        else if (context is AnimationLanguageRulesParser.FunctionCallExpressionContext)
        {
            AnimationLanguageRulesParser.FuncCallContext funcCallContext = ((AnimationLanguageRulesParser.FunctionCallExpressionContext)context).funcCall(); //Store the funcCall context in a variable.
            return VisitFuncCall(funcCallContext); // Call VisitFuncCall with the funcCall context.
        }
        else if (context is AnimationLanguageRulesParser.ShapeInitExpressionContext)
        {
            AnimationLanguageRulesParser.ShapeinitContext shapeInitContext = ((AnimationLanguageRulesParser.ShapeInitExpressionContext)context).shapeinit(); //Store the shapeInit context in a variable.
            return VisitShapeinit(shapeInitContext); // Call VisitShapeinit with the shapeInit context.
        }
        else if (context is AnimationLanguageRulesParser.TermExpressionContext)
        {
            AnimationLanguageRulesParser.TermContext termContext = ((AnimationLanguageRulesParser.TermExpressionContext)context).term(); //Store the term context in a variable.
            return VisitTerm(termContext); // Call VisitTerm with the term context.
        }
        else
        {
            // TODO: mby consider the thought of having an EmptyNode node instead of throwing exceptions.
            throw new InvalidOperationException($"Unexpected expression type in VisitExpression at {GetSourceLocation(context.Start)}");
        }
    }

    public override IASTNode VisitIntegerExpression(AnimationLanguageRulesParser.IntegerExpressionContext context)
    {
        int value = int.Parse(context.INTEGER().GetText());
        return new IntegerLiteralNode(value, GetSourceLocation(context.Start)); //The sourcelocation is retrieved from the context.
    }


    public override IASTNode VisitFloatExpression(AnimationLanguageRulesParser.FloatExpressionContext context)
    {
        float value = float.Parse(context.FLOAT().GetText());
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
        bool value = context.boolean().TRUE() != null;
        return new BooleanLiteralNode(value, GetSourceLocation(context.Start));
    }

    //This method is called when a function call is encountered in the code.
    public override IASTNode VisitFuncCall(AnimationLanguageRulesParser.FuncCallContext context)
    {
        IdentifierNode functionName = (IdentifierNode)VisitTerminal((ITerminalNode)context.IDENTIFIER());
        List<ExpressionNode> arguments = new List<ExpressionNode>();

        if (context.call_parameters() != null)
        {
            int numParameters = context.call_parameters().ChildCount;
            for (int i = 0; i < numParameters; i++)
            {
                if (context.call_parameters().GetChild(i) is AnimationLanguageRulesParser.Call_parameterContext parameterContext)
                {
                    arguments.Add((ExpressionNode)Visit(parameterContext));
                }
            }
        }
        SourceLocation sourceLocation = GetSourceLocation(context.Start);
        return new FunctionCallNode(functionName, arguments, sourceLocation);
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

    
    //This method is called when a term is encountered in the code.
    public override IASTNode VisitTerm(AnimationLanguageRulesParser.TermContext context)
    {
        IASTNode innerExpression = Visit(context.GetChild(1)); //Visit the inner expression of the term and store the result in a variable.
        return innerExpression;
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


    //This method is called when the prototype rule is encountered in the code.
    public override IASTNode VisitPrototypes(AnimationLanguageRulesParser.PrototypesContext context)
    {
        List<IASTNode> prototypes = new List<IASTNode>(); //Create a list of IASTNodes to store the prototypes.

        foreach (var prototypeContext in context.GetRuleContexts<AnimationLanguageRulesParser.PrototypeContext>())
        {
            prototypes.Add(VisitPrototype(prototypeContext)); //Visit each prototype and add it to the list.
        }

        return new NodeList<IASTNode>(prototypes, GetSourceLocation(context.Start)); //Return a NodeList containing the prototypes.
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
        string functionName = context.IDENTIFIER().GetText(); //Get the name of the function.
        IdentifierNode identifierNode = new IdentifierNode(functionName, GetSourceLocation(context.IDENTIFIER().Symbol)); //Create an identifier node for the function name.

        IList<ParameterNode> parameters = new List<ParameterNode>(); //Create a list of ParameterNodes to store the parameters of the function.
        if (context.parameters() != null) //If the function has any parameters, visit them and add them to the list.
        {
            parameters = VisitParameters(context.parameters());
        }

        BlockNode blockNode = (BlockNode)VisitBlock(context.block()); //Visit the block of the function and store it in a variable.
        SourceLocation sourceLocation = GetSourceLocation(context.Start); 

        return new FunctionDeclarationNode(identifierNode, parameters, blockNode, sourceLocation);
    }

    
    // this method is being called when a block is being declared in the code.
    public override IASTNode VisitBlock(AnimationLanguageRulesParser.BlockContext context)
    {
        IList<StatementNode> statementNodes = new List<StatementNode>(); //Create a list of StatementNodes to store the statements of the block.
        if (context.statements() != null) 
        {
            statementNodes = VisitStatements(context.statements()); //Visit the statements of the block and add them to the list.
        }

        BlockNode blockNode = new BlockNode(statementNodes, GetSourceLocation(context.Start)); //Create a block node with the list of statements and the source location of the block
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
        ExpressionNode? returnExpression = null;

        if (context.expression() != null)
        {
            returnExpression = Visit(context.expression()) as ExpressionNode;
        }

        SourceLocation sourceLocation = GetSourceLocation(context.Start);
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
        AssignmentNode startExpression = (AssignmentNode)VisitAssignment(context.assignment(0)); //Visit the start expression of the for loop.
        ConditionNode condition = (ConditionNode)VisitCondition(context.condition()); //Visit the condition of the for loop.
        AssignmentNode endExpression = (AssignmentNode)VisitAssignment(context.assignment(1)); //Visit the end expression of the for loop.
        BlockNode block = (BlockNode)VisitBlock(context.block()); //Visit the block of the for loop.
        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        return new ForLoopNode(startExpression, condition, endExpression, block, sourceLocation);
    }

    
    //This method in run when a while loop is met in the code.
    public override IASTNode VisitWhile_loop(AnimationLanguageRulesParser.While_loopContext context)
    {
        ConditionNode condition = (ConditionNode)VisitCondition(context.condition()); //Visit the condition of the while loop.
        BlockNode body = (BlockNode)VisitBlock(context.block()); //Visit the block of the while loop.
        SourceLocation sourceLocation = GetSourceLocation(context.Start); 

        return new WhileLoopNode(condition, body, sourceLocation);
    }


    
    //he VisitCondition method is used when a condition is met in the code. 
    public IASTNode VisitCondition(AnimationLanguageRulesParser.ConditionContext context)
    {
        IASTNode leftExpression = Visit(context.expression(0));
        IASTNode rightExpression = Visit(context.expression(1));
        OperatorNode? comparisonOperator = context.comparator().Length > 0 ? (OperatorNode)Visit(context.comparator(0)) : null; //Visit the comparator of the condition if it exists. (0)) : null means that if there is no comparator, it will be null.
        OperatorNode? logicalOperator = context.logicOpp().Length > 0 ? (OperatorNode)Visit(context.logicOpp(0)) : null; //Visit the logical operator of the condition if it exists. (0)) : null means that if there is no logical operator, it will be null.

        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        return new ConditionNode(leftExpression, rightExpression, comparisonOperator, logicalOperator, sourceLocation);
    }


    //This method is called when a conditional is encountered in the code.
    public override IASTNode VisitConditional(AnimationLanguageRulesParser.ConditionalContext context)
    {
        ExpressionNode condition = (ExpressionNode)Visit(context.condition()); //Visit the condition of the conditional.
        BlockNode ifBlock = (BlockNode)VisitBlock(context.block()); //Visit the block of the conditional.

        IList<ElseIfNode> elseIfBranches = new List<ElseIfNode>(); //Create a list to contain all of the else if branches related to the conditional.
        for (int i = 0; i < context.elseif().Length; i++)
        {
            elseIfBranches.Add((ElseIfNode)VisitElseif(context.elseif(i))); //Visit each of the else if branches.
        }

        ElseNode? elseBranch = null; //Creates a variable to contain the else branch of the conditional. Its set to null, as an if-statement can both have an else, and not have an else.
        if (context.@else() != null) 
        {
            elseBranch = (ElseNode)VisitElse(context.@else()); //Visit the else branch if it exists.
        }

        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        return new IfStatementNode(condition, ifBlock, elseIfBranches, elseBranch, sourceLocation);
    }


    //This method is called when an else if branch is encountered in the code.
    public override IASTNode VisitElseif(AnimationLanguageRulesParser.ElseifContext context)
    {
        ExpressionNode condition = (ExpressionNode)Visit(context.condition()); //Visit the condition of the else if branch.
        BlockNode block = (BlockNode)VisitBlock(context.block()); //Visit the block of the else if branch.
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
    public List<SequenceNode> VisitAndGetSequences(AnimationLanguageRulesParser.SequencesContext context)
    {
        var sequences = new List<SequenceNode>(); //Create a list to contain all of the individual sequences.

        foreach (var child in context.children) //For each child of thesequences rule, visit it.
        {
            if (child is AnimationLanguageRulesParser.SequenceContext sequenceContext) 
            {
                sequences.Add((SequenceNode)Visit(sequenceContext)); //If the child is a sequence, visit it and add it to the list.
            }
        }
        return sequences;
    }


    //this method in run when a sequence is met in the code.
    public override IASTNode VisitSequence(AnimationLanguageRulesParser.SequenceContext context)
    {
        var identifierNode = (IdentifierNode)VisitTerminal(context.IDENTIFIER()); //Visit the identifier of the sequence.
        var parameters = context.parameters() != null ? VisitParameters(context.parameters()) : new List<ParameterNode>(); //Visit the parameters of the sequence if it has any.
        var seqBlockNode = VisitSeqBlock(context.seqBlock()); //Visit the block of the sequence.
        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        return new SequenceNode(identifierNode, parameters, (SeqBlockNode)seqBlockNode, sourceLocation);
    }


    //This method is called when a sequence block is encountered in the code.
    public override IASTNode VisitSeqBlock(AnimationLanguageRulesParser.SeqBlockContext context)
    {
        var seqBlockParts = new List<SeqBlockPartNode>(); //Create a list to contain all of the parts of the sequence block.

        foreach (var child in context.children) //For each child of the sequence block, visit it.
        {
            if (child is AnimationLanguageRulesParser.SeqBlockPartsContext seqBlockPartsContext)
            {
                seqBlockParts.Add((SeqBlockPartNode)VisitSeqBlockPart(seqBlockPartsContext)); //If the child is a sequence block part, visit it and add it to the list.
            }
        }

        return new SeqBlockNode(seqBlockParts, GetSourceLocation(context.Start));
    }

    //This method is called when a sequence block part is encountered in the code.
    public SeqBlockPartNode VisitSeqBlockPart(AnimationLanguageRulesParser.SeqBlockPartsContext context)
    {
        IASTNode child;

        if (context.statement() != null) //If the sequence block part is a statement, visit it.
        {
            child = VisitStatement(context.statement());
        }
        else if (context.animation() != null) //If the sequence block part is an animation, visit it.
        {
            child = VisitAnimation(context.animation());
        }
        else
        {
            throw new InvalidOperationException($"Unexpected child in SeqBlockPartsContext: {context.GetText()}");
        }

        return new SeqBlockPartNode(child, GetSourceLocation(context.Start));
    }


    //This method is called when a sequence call is encountered in the code.
    public override IASTNode VisitSequenceCall(AnimationLanguageRulesParser.SequenceCallContext context)
    {
        IdentifierNode identifierNode = (IdentifierNode)VisitTerminal(context.IDENTIFIER()); //Visit the identifier of the sequence call.
        IList<IASTNode> arguments = new List<IASTNode>(); //Create a list to contain all of the arguments of the sequence call.

        if (context.call_parameters() != null)
        {
            IList<IASTNode> visitedArguments = (IList<IASTNode>)Visit(context.call_parameters()); //Visit the arguments of the sequence call if it has any.
            foreach (IASTNode argumentNode in visitedArguments)
            {
                arguments.Add(argumentNode); //Add each of the arguments to the list.
            }
        }

        return new SequenceCallNode(identifierNode, arguments.Cast<ArgumentNode>(), GetSourceLocation(context.IDENTIFIER().Symbol));
    }

    
    
    //This method is called when an animation is encountered in the code.
    public override IASTNode VisitAnimation(AnimationLanguageRulesParser.AnimationContext context)
    {
        var identifierNode = (IdentifierNode)VisitTerminal(context.IDENTIFIER()); //Visit the identifier of the animation.
        CommandNode? commandNode = null; //Create a variable to contain the command of the animation. Its nullable, as an animation can both have a command, and not have a command.

        if (context.command() != null)
        {
            commandNode = (CommandNode)VisitCommand(context.command()); //Visit the command of the animation if it has one.
        }

        var transitions = VisitAndGetTransitions(context.transitions());
        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        return new AnimationNode(identifierNode, commandNode, transitions, sourceLocation);
    }



    public List<TransitionNode> VisitAndGetTransitions(AnimationLanguageRulesParser.TransitionsContext context)
    {
        var transitions = new List<TransitionNode>();

        foreach (var child in context.children)
        {
            if (child is AnimationLanguageRulesParser.TransitionContext transitionContext)
            {
                transitions.Add((TransitionNode)VisitTransition(transitionContext));
            }
            else if (child is AnimationLanguageRulesParser.CommandContext commandContext)
            {
                transitions.Add((TransitionNode)VisitCommand(commandContext));
            }
        }

        return transitions;
    }


    public override IASTNode VisitTransition(AnimationLanguageRulesParser.TransitionContext context)
    {
        var callParameters = context.call_parameters() != null ? (List<IASTNode>)VisitCall_parameters(context.call_parameters()) : new List<IASTNode>();


        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        return new TransitionNode(callParameters, sourceLocation);
    }
    
    
    //This method is called when a command is encountered in the code.
    public override IASTNode VisitCommand(AnimationLanguageRulesParser.CommandContext context)
    {
        // Get the identifier of the command
        IdentifierNode identifierNode = (IdentifierNode)VisitTerminal(context.IDENTIFIER());
        
        IList<IASTNode> parameters = new List<IASTNode>(); // Create a list to contain the parameters of the command
        if (context.call_parameters() != null)
        {
            IList<IASTNode> visitedParameters = (IList<IASTNode>)Visit(context.call_parameters()); // Visit the parameters of the command
            foreach (IASTNode parameterNode in visitedParameters)
            {
                parameters.Add(parameterNode); // Add the visited parameters to the list
            }
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
}
