using AnimationLanguage.ASTCommon;
using AnimationLanguage.ASTNodes;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

public class AnimationLanguageVisitor : AnimationLanguageRulesBaseVisitor<object>
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
        IdentifierNode identifier = (IdentifierNode)Visit(context.IDENTIFIER());
        ExpressionNode expression = (ExpressionNode)Visit(context.expression());

        // Create a new KeyValuePairNode with the identifier and expression.
        KeyValuePairNode keyValuePairNode = new KeyValuePairNode(identifier, expression, GetSourceLocation(context.Start));

        return keyValuePairNode;
    }
    
    
    public override IASTNode VisitAssignment(AnimationLanguageRulesParser.AssignmentContext context)
    {
        IdentifierNode identifier = (IdentifierNode)VisitIdentifier(context.IDENTIFIER()); // Visit the identifier of the assignment, and store the result as an identifier node.
        ExpressionNode expression = (ExpressionNode)VisitExpression(context.expression()); // Do the same with the expression
        SourceLocation sourceLocation = GetSourceLocation(context.Start);

        AssignmentNode assignmentNode = new AssignmentNode(identifier, expression, sourceLocation);
        return assignmentNode;
    }


    public override IASTNode VisitIdentifier(AnimationLanguageRulesParser.IdentifierContext context)
    {
        // Get the identifier text from the context
        string identifierText = context.GetText();

        // Create an IdentifierNode with the identifier text and set its SourceLocation
        IdentifierNode identifierNode = new IdentifierNode(identifierText, GetSourceLocation(context.Start));

        return identifierNode;
    }

    
    //---------------------------Helper methods---------------------------//
    private SourceLocation GetSourceLocation(IToken token)
    {
        return new SourceLocation(token.Line, token.Column);
    }

    

}
