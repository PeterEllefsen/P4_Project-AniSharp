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
            codeBuilder("a",$"            {node.VariableType.ToString().ToLower()} {node.Identifier}");
            
            
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


        return node;
    }

    public override IASTNode? Visit(IfStatementNode node)
    {
        return node;
    }

    public override IASTNode? Visit(ElseIfNode node)
    {
        return node;
    }

    public override IASTNode? Visit(ElseNode node)
    {
        return node;
    }

    public override IASTNode? Visit(WhileLoopNode node)
    {
        return node;
    }

    public override IASTNode? Visit(ForLoopNode node)
    {
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
        if (node.OperatorNode != null)
        {
            var operatorNodeOperatorSymbol = node.OperatorNode.OperatorSymbol;
            Console.WriteLine(node.LeftOperand + " " + operatorNodeOperatorSymbol + " " + node.LeftOperand);
            codeBuilder("a",node.LeftOperand + " " + operatorNodeOperatorSymbol + " " + node.RightOperand);
        }
        else
        {
            codeBuilder("a",node.ToString());
        }

        // if (node.LeftOperand is ExpressionNode leftOperand)
        // {
        //     Visit(leftOperand);
        // }
        //
        // // Visit the right operand, if it exists and is an ExpressionNode
        // if (node.RightOperand is ExpressionNode rightOperand)
        // {
        //     Visit(rightOperand);
        // }
        
        // Visit any other children or sub-expressions, if applicable
    
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

