using System.Reflection;
using System.Threading.Channels;
using AnimationLanguage.ASTNodes;
using systemio = System.IO;

namespace AnimationLanguage.Visitor;
using ASTCommon;


public class CodeGenerationVisitor : ASTVisitor<IASTNode>
{
    private void CreateFilesForCompilation() {

        //if file exists delete it
        if (File.Exists("../../codegen/Program.txt"))
        {
            File.Delete("../../codegen/Program.txt");
        }
        //files to create
        //Program.cs for main
        //function class for all functions
        //Sequence class containing methods that create sequences
        using (FileStream fs = File.Create("../../../codegen/Program.txt", 1024))
        {
        }
        // using (FileStream fs = File.Create("../../codegen/Functions.cs", 1024))
        // {
        // }
        // using (FileStream fs = File.Create("../../codegen/Sequences.cs", 1024))
        // {
        // }
    }

    private void AppendToFile(string file, string appendingString) {
        string path = "";
        if (file == "main") {
            path = "../../../codegen/Program.txt";
        }

        if (path != "") {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(appendingString);
            }	
        }

    }

    public CodeGenerationVisitor()
    {
    }

    public override IASTNode? Visit(ProgramNode node)
    {

        CreateFilesForCompilation();
        
        string programCode = @"
        namespace GeneratedCode
        {
            class Program
            {
                static void Main(string[] args)
                {

                }
            }
        }           
        ";
        
        AppendToFile("main", programCode);
        
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(SetupNode node)
    {
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(GroupingElementsNode node)
    {
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(KeyValuePairNode node)
    {
        return VisitChildren(node);
    }
    
    public override IASTNode? Visit(IdentifierNode node)
    {
        return VisitChildren(node);
    }

    public override IASTNode? Visit(AssignmentNode node)
    {
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