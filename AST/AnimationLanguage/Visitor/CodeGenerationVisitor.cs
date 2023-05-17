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

    private void AppendToFile(string file, string appendingString)
    {
        var path = "";
        if (file == "main") path = "../../../codegen/Program.txt";

        if (path != "")
            using (var sw = File.AppendText(path))
            {
                sw.WriteLine(appendingString);
            }
    }

    public override IASTNode? Visit(ProgramNode node)
    {
        CreateFilesForCompilation();

//         string programCode = @"
//         namespace GeneratedCode
//         {
//             class Program
//             {
//                 static void Main(string[] args)
//                 {
//
//                 }
//             }
//         }           
//         ";
//         
//         AppendToFile("main", programCode);
//         
        foreach (var Child in node.GetChildren())
            //Console.WriteLine(Child.GetType());
            if (Child is SetupNode setupNode)
            {
                Console.WriteLine("Found a SetupNode from ProgramNode: " + setupNode + "\n");
                   Visit(setupNode);
            }
            else if (Child is FunctionDeclarationNode funcDecNode)
            {
                //Console.WriteLine("\n Found a Function Declaration from ProgramNode: " + funcDecNode.Identifier + "\n\n");
                 Visit(funcDecNode);
            }
            else if (Child is SequenceNode seqDecNode)
            {
                //Console.WriteLine("\n Found a Sequence Declaration from ProgramNode: " + seqDecNode.Name + "\n\n");
                 Visit(seqDecNode);
            }
            else if (Child is TimelineBlockNode timelineBlockNode) 
            {
                    //Console.WriteLine("\n Found a Timeline from ProgramNode: " + "\n\n");
                     Visit(timelineBlockNode);
            } 
        return node;
    }


    //return VisitChildren(node);
    public override IASTNode? Visit(SetupNode node)
    {
        Console.WriteLine("The following nodes are present in SetupNode:");

        foreach (var Child in node.GetChildren())
        {
            Console.WriteLine(Child.GetType()+"\n");
            if (Child is GroupingElementsNode groupingElementsNode ) 
                Visit(groupingElementsNode);
        }
        return node;
    }

    public override IASTNode? Visit(GroupingElementsNode node)
    {
        // Cant be bothered with this right now. W.I.P
        // I get a list of KeyValuePairsNodes and IdentifierNodes. We know this works from a previous example (Daniel knows)
        //Also how do we handle the values? Do they get converted to dictionaries or arrays? In that case, we'd need to know the size of the array.
        // but we cannot use .Count on the number of keyvaluepairs nodes?
        Console.WriteLine("The following nodes are present in GroupingElementsNode:");
        foreach (var Child in node.GetChildren())
        {
            Console.WriteLine(Child.GetType());
        }
        return VisitChildren(node);
    }

    public override IASTNode? Visit(KeyValuePairNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(IdentifierNode node)
    {
        //Console.WriteLine(node.Type);
        return VisitChildren(node);
    }

    public override IASTNode? Visit(AssignmentNode node)
    {
        // foreach (var Child in node.GetChildren())
        // {
        //     Console.WriteLine(Child);
        //     if (Child is IdentifierNode idNode)
        //     {
        //         
        //         Visit(idNode);
        //     }
        //
        // }

        return VisitChildren(node);
    }

    public override IASTNode? Visit(OperatorNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(IdentifierGroupingNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(ArgumentNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(FunctionDeclarationNode node)
    {
        // foreach (var Child in node.GetChildren())
        // {
        //     if (Child is BlockNode blockNode)
        //     {
        //         //Console.WriteLine("Entering SetupNode");
        //         Visit(blockNode);
        //     }
        // }

        return VisitChildren(node);
    }

    public override IASTNode? Visit(FunctionCallNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(ParameterNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(CallParameterNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(PrototypeNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(StatementNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(IfStatementNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(ElseIfNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(ElseNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(WhileLoopNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(ForLoopNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(BlockNode node)
    {
        // foreach (var child in node.GetChildren())
        // {
        //     Console.WriteLine(child.GetType());
        //     if (child is AssignmentNode assnode)
        //     {
        //         Visit(assnode);
        //     }
        // }

        return VisitChildren(node);
    }

    public override IASTNode? Visit(SeqBlockNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(AnimationNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(CommandNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(ConditionNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(ExpressionNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(FrameDefNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(IntegerLiteralNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(FloatLiteralNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(StringLiteralNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(BooleanLiteralNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(ReturnNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(SeqBlockPartNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(SequenceCallNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(SequenceNode node)
    {
        return VisitChildren(node);
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
        return VisitChildren(node);
    }

    public override IASTNode? Visit(TransitionNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(TupleNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(TypeNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(UnaryOperationNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(NodeList<IASTNode> node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(ShapeInitNode node)
    {
        return VisitChildren(node);
    }
}