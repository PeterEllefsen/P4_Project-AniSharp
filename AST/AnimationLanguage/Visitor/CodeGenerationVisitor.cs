using System.Reflection.Emit;
using AnimationLanguage.ASTCommon;
using AnimationLanguage.ASTNodes;
using systemio = System.IO;

namespace AnimationLanguage.Visitor;

public class CodeGenerationVisitor : ASTVisitor<IASTNode>
{
    private void CreateFilesForCompilation()
    {
        //if file exists delete it
        if (File.Exists("../../codegen/Program.txt")) File.Delete("../../codegen/Program.txt");

        //files to create
        //Program.cs for main
        //function class for all functions
        //Sequence class containing methods that create sequences
        using (var fs = File.Create("../../../codegen/Program.txt", 1024))
        {
        }
        // using (FileStream fs = File.Create("../../codegen/Functions.cs", 1024))
        // {
        // }
        // using (FileStream fs = File.Create("../../codegen/Sequences.cs", 1024))
        // {
        // }
    }

    private void codeBuilder(string p, string appendingString)
    {
        var path = "";
        path = "../../../codegen/Program.txt";


        if (p == "a")
        {
            using (var sw = File.AppendText(path))
            {
                sw.Write(appendingString);
            }
        }
        else if (p == "w")
        {
            using (var sw = File.AppendText(path))
            {
                sw.WriteLine(appendingString);
            }
        }
    }

    public override IASTNode? Visit(ProgramNode node)
    {
        CreateFilesForCompilation();
        codeBuilder("w","using System;");
        codeBuilder("w","");
        codeBuilder("w","namespace AnimationLanguage");
        codeBuilder("w","{");
        codeBuilder("w","   public static class Program");
        codeBuilder("w","   {");
        codeBuilder("w","       public static void Main()");
        codeBuilder("w","       {");

        foreach (var child in node.GetChildren())
        {
            if (child is FunctionDeclarationNode functionDeclarationNode)
            {
                Visit(functionDeclarationNode);
                codeBuilder("w", "");
            }
        }

        
        codeBuilder("w","       }");
        codeBuilder("w","    }");
        codeBuilder("w","}");
            
        
    
        
            //Console.WriteLine(Child.GetType());
            
        return node;
    }
    
    public override IASTNode? Visit(SetupNode node)
    {
        
        
        foreach (var Child in node.GetChildren())
        {
            if (Child is GroupingElementsNode groupingElementsNode)
            {
                Visit(groupingElementsNode);
            }
            
            
        }
        
        return node;
    }

    public override IASTNode? Visit(GroupingElementsNode node)
    {
        foreach (var Child in node.GetChildren())
        {
            if (Child is KeyValuePairNode keyValuePair)
            {
                Visit(keyValuePair);
            }
        }
        return node;
    }

    public override IASTNode? Visit(KeyValuePairNode node)
    {

        foreach (var Child in node.GetChildren())
        {
            if (Child is IntegerLiteralNode integerLiteralNode)
            {
                //AppendToFile("main", Child.ToString() + ";\n");
            } else if (Child is IdentifierNode identifierNode)
            {
                //AppendToFile("main", "int ");
                Visit(identifierNode);
            }
        }
        return node;
    }

    public override IASTNode? Visit(IdentifierNode node)
    {
        //AppendToFile("main", node.ToString() + " = ");
        
        return node;
    }

    public override IASTNode? Visit(AssignmentNode node)
    {
        
        if (node.VariableType != VariableType.Null)
        {
            if (node.IsDeclaration)
            {
                codeBuilder("a",$"            {node.VariableType.ToString().ToLower()} {node.Identifier}");
            }
            else
            {
                codeBuilder("a",$"            {node.Identifier}");
            }

            //assigment operator insert
            switch (node.AssignmentOperator)
            {
                case AssignmentOperator.Assign:
                    codeBuilder("a", " = ");
                    break;
                case AssignmentOperator.PlusEqual:
                    codeBuilder("a", " += ");
                    break;
                case AssignmentOperator.MinusEqual:
                    codeBuilder("a", " -= ");
                    break;
                // Add cases for other assignment operators as needed
                default:
                    // Handle the default case, if necessary
                    break;
            }

            //insert expression
            Visit(node.Expression);

            codeBuilder("w",";");

        }else 
        {
            Console.WriteLine("Variable type is null");
        }
        
        
        return node;
    }

    public override IASTNode? Visit(OperatorNode node)
    {
        return node;
    }

    public override IASTNode? Visit(IdentifierGroupingNode node)
    {
        return node;
    }

    public override IASTNode? Visit(ArgumentNode node)
    {
        return node;
    }

    public override IASTNode? Visit(FunctionDeclarationNode node)
    {
        codeBuilder("a",$"           public static {node.ReturnType.ToString().ToLower()} {node.Identifier}(");
        foreach (var Child in node.GetChildren())
        {
            if (Child is ParameterNode parameterNode)
            {
                Visit(parameterNode);
            }
        }
        codeBuilder("w",")");
        codeBuilder("w","           {");
        foreach (var Child in node.GetChildren())
        {
            if (Child is BlockNode blockNode)
            {
                Visit(blockNode);
            }
        }
        codeBuilder("w","           }");

        return node;
    }

    public override IASTNode? Visit(FunctionCallNode node)
    {
        return node;
    }

    public override IASTNode? Visit(ParameterNode node)
    {
        codeBuilder("a",$"{node.DataType.ToString().ToLower()} {node.Name}");
        return node;
    }

    public override IASTNode? Visit(CallParameterNode node)
    {
        return node;
    }

    public override IASTNode? Visit(PrototypeNode node)
    {
        return node;
    }

    public override IASTNode? Visit(StatementNode node)
    {
        if (node is AssignmentNode assignmentNode)
        {
            Visit(assignmentNode);
        }

        if (node is IdentifierGroupingNode identifierNode)
        {
            Visit(identifierNode);
        }

        if (node is IfStatementNode ifStatementNode)
        {
            Visit(ifStatementNode);
        }

        if (node is ForLoopNode forLoopNode)
        {
            Visit(forLoopNode);
        }

        if (node is WhileLoopNode whileLoopNode)
        {
            Visit(whileLoopNode);
        }
        
        

        return node;
    }

    public override IASTNode? Visit(IfStatementNode node)
    {
        codeBuilder("w", ""); 
        codeBuilder("a","            if ");
        codeBuilder("w",$"{node.Condition}" + "{");
        codeBuilder("a", "   ");
        Visit(node.IfBlock);
        codeBuilder("w","            }");

        foreach (var child in node.ElseIfBranches)
        {
            Visit(child);
        }

        if (node.ElseBranch != null)
        {
            Visit(node.ElseBranch);
        }
        
        

        
        //codeBuilder("w", node.ToString());
        
        
        return node;
    }

    public override IASTNode? Visit(ElseIfNode node)
    {
        codeBuilder("a","            else if ");
        codeBuilder("w",$"{node.Condition}" + "{");
        codeBuilder("a", "   ");
        Visit(node.ElseIfBlock);
        codeBuilder("w","            }");
        
        return node;
    }

    public override IASTNode? Visit(ElseNode node)
    {
        codeBuilder("a","            else");
        codeBuilder("w","{");
        codeBuilder("a", "   ");
        Visit(node.ElseBlock);
        codeBuilder("w","            }");
        
        return node;
    }

    public override IASTNode? Visit(WhileLoopNode node)
    {   
        codeBuilder("a", "\n\n");
        codeBuilder("a", "            while(");
        codeBuilder("w", $"{node.Condition}" + ")" + "\n" + "            {");
        codeBuilder("a", "   ");
        Visit(node.Body);
        codeBuilder("w", "            }");
        return node;
    }

    public override IASTNode? Visit(ForLoopNode node)
    {
        Console.WriteLine(node.ToString());
        codeBuilder("a", "\n\n");
        codeBuilder("a", "            for(");
        
        if (node.Initialization is AssignmentNode assignmentNode)
        {
            Visit(assignmentNode);
        }


        codeBuilder("a", node.Condition.ToString());
        codeBuilder("a", ";");
        
        Console.WriteLine(node.Update);
        if (node.Update is UnaryOperationNode unaryOperationNode)
        {
            Visit(unaryOperationNode);
            codeBuilder("a", ")");
        }
        codeBuilder("a", "{");
        if (node.Body is BlockNode blockNode)
        {
            Visit(blockNode);
        }
        codeBuilder("a", "}");
        return node;
    }

    public override IASTNode? Visit(BlockNode node)
    {
        foreach (var Child in node.GetChildren())
        {
            if (Child is StatementNode statementNode)
            {
                //Console.WriteLine(Child.GetType());
                Visit(statementNode);

            }else if (Child is ReturnNode returnNode)
            {
                Visit(returnNode);
            }
        }

        return node;
    }

    public override IASTNode? Visit(SeqBlockNode node)
    {
        return node;
    }

    public override IASTNode? Visit(AnimationNode node)
    {
        return node;
    }

    public override IASTNode? Visit(CommandNode node)
    {
        return node;
    }

    public override IASTNode? Visit(ConditionNode node)
    {
        return node;
    }

    public override IASTNode? Visit(ExpressionNode node)
    {
        
        if (node.RightOperand != null)
        {
            
            if (node.LeftOperand is ExpressionNode leftOperand)
            {
                codeBuilder("a", leftOperand.ToString());
            }
        
            if (node.OperatorNode is OperatorNode operatorNode)
            {
                codeBuilder("a", " " + operatorNode.OperatorSymbol + " ");
            }
        
            if (node.RightOperand is ExpressionNode rightOperand)
            {
                
                codeBuilder("a", rightOperand.ToString());
                
            }
            
        }
        else
        {
            codeBuilder("a", $"{node.ToString()}");
        }

    
        return node;
    }

    public override IASTNode? Visit(FrameDefNode node)
    {
        return node;
    }

    public override IASTNode? Visit(IntegerLiteralNode node)
    {
        return node;
    }

    public override IASTNode? Visit(FloatLiteralNode node)
    {
        return node;
    }

    public override IASTNode? Visit(StringLiteralNode node)
    {
        return node;
    }

    public override IASTNode? Visit(BooleanLiteralNode node)
    {
        return node;
    }

    public override IASTNode? Visit(ReturnNode node)
    {
        if(node.ToString() != "")
        {
            codeBuilder("w", $"");
            codeBuilder("w", $"            return {node};");
        }
        
        
        return node;
    }

    public override IASTNode? Visit(SeqBlockPartNode node)
    {
        return node;
    }

    public override IASTNode? Visit(SequenceCallNode node)
    {
        return node;
    }

    public override IASTNode? Visit(SequenceNode node)
    {
        return node;
    }

    public override IASTNode? Visit(PolygonNode node)
    {
        return node;
    }

    public override IASTNode? Visit(CircleNode node)
    {
        return node;
    }

    public override IASTNode? Visit(TimelineBlockNode node)
    {
        return node;
    }

    public override IASTNode? Visit(TransitionNode node)
    {
        return node;
    }

    public override IASTNode? Visit(TupleNode node)
    {
        return node;
    }

    public override IASTNode? Visit(TypeNode node)
    {
        return node;
    }

    public override IASTNode? Visit(UnaryOperationNode node)
    {
        switch (node.Operator)
        {
            case UnaryOperator.Increment:
                codeBuilder("a", $"{node.Identifier}++");
                break;
            case UnaryOperator.Decrement:
                codeBuilder("a", $"{node.Identifier}--");
                break;
        }
        return node;
    }

    public override IASTNode? Visit(NodeList<IASTNode> node)
    {
        return node;
    }

    public override IASTNode? Visit(ShapeInitNode node)
    {
        return node;
    }
    


}

