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
                PrototypeNode prototypeNode = (PrototypeNode)VisitPrototypes(prototypesContext); // If the child is a prototype, visit it and store the result as a prototype node.
                prototypeNodes.Add(prototypeNode); // Add the prototype node to the list of prototype nodes.
            }
            else if (child is AnimationLanguageRulesParser.FuncDeclContext funcDeclContext) 
            {
                functionDeclarations.Add((FunctionDeclarationNode)VisitFuncDecl(funcDeclContext)); // If the child is a function declaration, visit it and store the result as a function declaration node.
            }
            else if (child is AnimationLanguageRulesParser.SequencesContext sequencesContext)
            {
                SequenceNode sequenceNode = (SequenceNode)VisitSequences(sequencesContext); // If the child is a sequence, visit it and store the result as a sequence node.
                sequenceNodes.Add(sequenceNode); // Add the sequence node to the list of sequence nodes.
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
                KeyValuePairNode keyValuePairNode = (KeyValuePairNode)VisitKeyValuePair(keyValuePairContext);
                keyValuePairs.Add(keyValuePairNode);
            }
            else if (item is AnimationLanguageRulesParser.ExpressionContext expressionContext) // If the visited item is an expression, visit it and add the result to the list of expressions.
            {
                //ExpressionNode expressionNode = (ExpressionNode)VisitExpression(expressionContext);
                //expressions.Add(expressionNode);
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
        ExpressionNode expression = (ExpressionNode)Visit(context.expression()); // Visit the expression context and get the ExpressionNode.

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
        IdentifierNode identifierNode = (IdentifierNode)VisitTerminal((ITerminalNode)context.GetChild(0));
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
    public override IASTNode VisitType(AnimationLanguageRulesParser.TypeContext context)
    {
        TypeNode.TypeKind typeKind; //The TypeKind enum is used to determine what types are allowed.

        // Check the type of the context and set the corresponding TypeKind.
        if (context.INT() != null)
        {
            typeKind = TypeNode.TypeKind.Int;
        }
        else if (context.FLOAT_TYPE() != null)
        {
            typeKind = TypeNode.TypeKind.Float;
        }
        else if (context.STRING_TYPE() != null)
        {
            typeKind = TypeNode.TypeKind.String;
        }
        else if (context.BOOL() != null)
        {
            typeKind = TypeNode.TypeKind.Bool;
        }
        else if (context.CIRCLE() != null)
        {
            typeKind = TypeNode.TypeKind.Circle;
        }
        else if (context.POLYGON() != null)
        {
            typeKind = TypeNode.TypeKind.Polygon;
        }
        else
        {
            // TODO: mby return EmptyNode instead.
            throw new NotSupportedException($"Unsupported type encountered at line {context.Start.Line} column {context.Start.Column}");
        }

        // Create a new TypeNode with the determined TypeKind and SourceLocation.
        TypeNode typeNode = new TypeNode(typeKind, GetSourceLocation(context.Start));

        return typeNode;
    }


    //This is a custom method that visits an expression context and returns an ExpressionNode. Since it is not present in the parser, it does not need to be overridden.
    public IASTNode VisitExpression(AnimationLanguageRulesParser.ExpressionContext context)
    {
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
        int value = int.Parse(context.INTEGER().GetText()); //Store the value of the context in a variable.
        SourceLocation sourceLocation = GetSourceLocation(context.Start);
        return new IntegerLiteralNode(value, sourceLocation); //Create a new IntegerLiteralNode with the value and SourceLocation.
    }

    public override IASTNode VisitFloatExpression(AnimationLanguageRulesParser.FloatExpressionContext context)
    {
        float value = float.Parse(context.FLOAT().GetText());
        SourceLocation sourceLocation = GetSourceLocation(context.Start);
        return new FloatLiteralNode(value, sourceLocation);
    }

    public override IASTNode VisitStringExpression(AnimationLanguageRulesParser.StringExpressionContext context)
    {
        string value = context.STRING().GetText();
        value = value.Substring(1, value.Length - 2); // Removes the quotes surrounding the string, suchh that it is only the relevant data we gather
        SourceLocation sourceLocation = GetSourceLocation(context.Start);
        return new StringLiteralNode(value, sourceLocation); 
    }

    public override IASTNode VisitBooleanExpression(AnimationLanguageRulesParser.BooleanExpressionContext context)
    {
        bool value = context.boolean().TRUE() != null;
        SourceLocation sourceLocation = GetSourceLocation(context.Start);
        return new BooleanLiteralNode(value, sourceLocation);
    }

    //This method is called when a function call is encountered in the code.
    public override IASTNode VisitFuncCall(AnimationLanguageRulesParser.FuncCallContext context)
    {
        IdentifierNode functionName = (IdentifierNode)VisitTerminal((ITerminalNode)context.IDENTIFIER()); //Visit the IDENTIFIER terminal and cast the result to an IdentifierNode.
        List<ArgumentNode> arguments = new List<ArgumentNode>(); //Create a list of ArgumentNodes to store the arguments of the function call.

        if (context.call_parameters() != null) //If the function call has arguments, visit them and add them to the list.
        { 
            int numParameters = context.call_parameters().ChildCount; //ChildCountt represents the number of arguments in a function call
            for (int i = 0; i < numParameters; i++)
            {
                if (context.call_parameters().GetChild(i) is AnimationLanguageRulesParser.Call_parameterContext parameterContext) //If the child is a call_parameter context, visit it and add it to the list.
                {
                    arguments.Add((ArgumentNode)Visit(parameterContext));
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
        DataType returnType = GetDataTypeFromTypeContext(context.type()); //Get the return type of the prototype.
        string functionName = context.IDENTIFIER().GetText(); //Get the name of the function.
        List<ParameterNode> parameters = new List<ParameterNode>(); //Create a list of ParameterNodes to store the parameters of the prototype.

        if (context.parameters() != null) //If the prototype has parameters, visit them and add them to the list.
        {
            parameters = ((NodeList<ParameterNode>)VisitParameters(context.parameters())).ToList();
        }

        SourceLocation sourceLocation = GetSourceLocation(context.Start);
        return new PrototypeNode(returnType, functionName, parameters, sourceLocation);
    }

    //This method is used to return the DataType of a type context.
    private DataType GetDataTypeFromTypeContext(AnimationLanguageRulesParser.TypeContext context)
    {
        if (context.INT() != null)
        {
            return DataType.Int;
        }
        else if (context.FLOAT_TYPE() != null)
        {
            return DataType.Float;
        }
        else if (context.STRING_TYPE() != null)
        {
            return DataType.String;
        }
        else if (context.BOOL() != null)
        {
            return DataType.Bool;
        }
        else if (context.CIRCLE() != null)
        {
            return DataType.Circle;
        }
        else if (context.POLYGON() != null)
        {
            return DataType.Polygon;
        }
        else
        {
            // TODO: Maybe return EmptyNode.
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
        DataType dataType = GetDataTypeFromTypeContext(context.type()); //Get the data type of the parameter.
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

    public IASTNode VisitIdentifierGrouping(AnimationLanguageRulesParser.IdentifierGroupingContext context)
    {
        IdentifierNode identifier = new IdentifierNode(context.IDENTIFIER().GetText(), GetSourceLocation(context.IDENTIFIER().Symbol));
        GroupingElementsNode groupingElements = (GroupingElementsNode)VisitGroupingElements(context.groupingElements());
        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        return new IdentifierGroupingNode(identifier, groupingElements, sourceLocation);
    }

    
    
    //---------------------------Helper methods---------------------------//
    private SourceLocation GetSourceLocation(IToken token)
    {
        return new SourceLocation(token.Line, token.Column);
    }

    

}
