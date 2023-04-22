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
        AssignmentOperator assignmentOperator = VisitAssOps(context.assOps()); // Visit the assignment operator context and get the current AssignmentOperator.
        IdentifierNode identifierNode = (IdentifierNode)VisitTerminal((ITerminalNode)context.GetChild(0)); // Visit the IDENTIFIER context and get the IdentifierNode.
        ExpressionNode expression = (ExpressionNode)Visit(context.GetChild(2)); // Visit the expression context and get the ExpressionNode.

        //Create a new AssignmentNode with the retrieved identifier, assignment operator and expression data.
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
            throw new InvalidOperationException("Unexpected assignment operator");
        }
    }







    //---------------------------Helper methods---------------------------//
    private SourceLocation GetSourceLocation(IToken token)
    {
        return new SourceLocation(token.Line, token.Column);
    }

    

}
