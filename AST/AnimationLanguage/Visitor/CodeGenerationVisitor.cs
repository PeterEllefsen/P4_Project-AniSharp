using System.Reflection.Emit;
using System.Text.RegularExpressions;
using AnimationLanguage.ASTCommon;
using AnimationLanguage.ASTNodes;
using systemio = System.IO;

namespace AnimationLanguage.Visitor;

public class CodeGenerationVisitor : ASTVisitor<IASTNode>
{
    private void CreateFilesForCompilation()
    {
        //if file exists delete it
        if (File.Exists("../../codegen/Program.cs")) File.Delete("../../codegen/Program.cs");
        //files to create
        //Program.cs for main
        //function class for all functions
        //Sequence class containing methods that create sequences
        using (var fs = File.Create("../../../codegen/Program.cs", 1024))
        {
        }
    }

    private void codeBuilder(string p, string appendingString)
    {
        var path = "";
        path = "../../../codegen/Program.cs";


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

    private void insertBoilerplate()
    {
        codeBuilder("w", @"public class Circle{
      public (double, double) center { get; set; }
    public double radius { get; set; }
    public string color { get; set; }
    public double borderWidth { get; set; }

         public Circle()
        {
        center = (0,0); // Default center
        radius = 1; // Default radius
        color = ""#000000""; // Default color
        borderWidth = 1; // Default border width
        }

}
public class Polygon{
    public List<double[]> points { get; set; }
    public string color { get; set; }
    public int borderWidth { get; set; }

     public Polygon()
    {
        points = new List<double[]>();
        color = ""#000000""; // Default color
        borderWidth = 1; // Default border width
    }

}
public class group : Dictionary<string, object>
{
   public void Add(string key, object value)
   {
      base.Add(key, value);
   }
}
");
        
        codeBuilder("w", @"public class Animation
{
    public int endframe { get; set; }
    public int? x { get; set; }
    public int? y { get; set; }
    public int? radius { get; set; }
    public string? color { get; set; }
    public int? borderWidth { get; set; }
}");
    }

    private void insertFuncBoilerplate()
    {
        codeBuilder("w", @"public static string rgb(int red, int green, int blue)
{
    string hex = $""#{red:X2}{green:X2}{blue:X2}"";
    return hex;
}");
        
        codeBuilder("w", @" public static void PrintFramebuffer(List<List<string>> framebuffer)
        {
            for (int frameIndex = 0; frameIndex < framebuffer.Count; frameIndex++)
            {
                Console.WriteLine($""Frame {frameIndex + 1}:"");
        foreach (string details in framebuffer[frameIndex])
        {
            Console.WriteLine(details);
        }

        Console.WriteLine();
    }
}");
        
        codeBuilder("w", @" public static (int, int, int) HexToRgb(string hex)
        {
            // Remove the '#' symbol if present
            hex = hex.TrimStart('#');

            // Convert the hex code to RGB values
            int r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            return (r, g, b);
        }");
        
        
        codeBuilder("w", @" public static List<List<string>> animate(object item, List<Animation> animations, List<List<string>> framebuffer,
        int frameOffset)
    {
        int totalFrames = 0;
    
        foreach (var animation in animations)
        {
            if (animation.endframe > totalFrames)
            {
                totalFrames = animation.endframe + frameOffset;
            }
        }
    
        //wait until start frame
        for (int frame = 0; frame <= totalFrames; frame++)
        {
            framebuffer.Add(new List<string>() { """" }); //wait
        }
    
        if (item is group group)
        {
            // Code to animate a group
            // ...
        }
        else if (item is Circle circle)
        {
            // Code to animate an object of SomeObjectType
    
    
            for (int i = 0; i < animations.Count; i++)
            {
                int animationFrameCount =
                    i == 0 ? animations[i].endframe : animations[i].endframe - animations[i - 1].endframe;
    
                // Create a new list for the frame in the framebuffer
                List<string> frameList = new List<string>();
    
                for (int frame = 0; frame < animationFrameCount; frame++)
                {
                    if (animations[i].color != null)
                    {
                        (int r, int g, int b) startColor = HexToRgb(circle.color);
                        (int r, int g, int b) endColor = HexToRgb(animations[i].color);
    
                        // Calculate the color change per frame for each RGB component
                        int dr = (endColor.r - startColor.r) / (animationFrameCount - 1);
                        int dg = (endColor.g - startColor.g) / (animationFrameCount - 1);
                        int db = (endColor.b - startColor.b) / (animationFrameCount - 1);
    
                        // Animate the color for each frame
                        int currentR = startColor.r + (dr * frame);
                        int currentG = startColor.g + (dg * frame);
                        int currentB = startColor.b + (db * frame);
    
                        // Convert the RGB values back to a hex color code
                        string currentColor = rgb(currentR, currentG, currentB);
    
                        // Animate the color property
                        circle.color = currentColor;
                    }
    
                    if (animations[i].x != null)
                    {
                        double startX = circle.center.Item1;
                        double endX = animations[i].x.Value;
    
                        double dx = (endX - startX) / (animationFrameCount - 1);
    
                        // Animate the x-coordinate for each frame
                        double currentX = startX + (dx * frame);
                        circle.center = (currentX, circle.center.Item2);
                    }
    
                    if (animations[i].y != null)
                    {
                        double startY = circle.center.Item2;
                        double endY = animations[i].y.Value;
    
                        double dy = (endY - startY) / (animationFrameCount - 1);
    
                        // Animate the y-coordinate for each frame
                        double currentY = startY + (dy * frame);
                        circle.center = (circle.center.Item1, currentY);
                    }
    
                    if (animations[i].radius != null)
                    {
                        double startRadius = circle.radius;
                        double endRadius = animations[i].radius.Value;
    
                        double dr = (endRadius - startRadius) / (animationFrameCount - 1);
    
                        // Animate the radius for each frame
                        double currentRadius = startRadius + (dr * frame);
                        circle.radius = currentRadius;
                    }
    
                    if (animations[i].borderWidth != null)
                    {
                        double startBorderWidth = circle.borderWidth;
                        double endBorderWidth = animations[i].borderWidth.Value;
    
                        double dbw = (endBorderWidth - startBorderWidth) / (animationFrameCount - 1);
    
                        // Animate the borderWidth for each frame
                        double currentBorderWidth = startBorderWidth + (dbw * frame);
                        circle.borderWidth = currentBorderWidth;
                    }
                    
                    int frameIndex = frameOffset + (i == 0 ? 0 : animations[i - 1].endframe + frameOffset) + frame;
                    
                    while (framebuffer.Count <= frameIndex)
                    {
                        framebuffer.Add(new List<string>());
                    }
                    

                    //Add the frame to the framebuffer
                    framebuffer[frameOffset + (i == 0 ? 0 : animations[i - 1].endframe) + frame].Add($""circle|{circle.center.Item1}|{circle.center.Item2}|{circle.radius}|{circle.borderWidth}|{circle.color}"");
                    
   
                    
                    
                }
            }

            PrintFramebuffer(framebuffer);
        }
        else if (item is Polygon polygon)
        {
            // Code to animate an object of SomeObjectType
            // ...
        }
    
        return framebuffer;
    }");
        
        
    }


    public override IASTNode? Visit(ProgramNode node)
    {
        CreateFilesForCompilation();

        codeBuilder("w", "using System;");
        codeBuilder("w", "");
        codeBuilder("w", "namespace AnimationLanguage;");
        codeBuilder("w", "");


        insertBoilerplate();

        codeBuilder("w", "public static class Sequences {");

        foreach (var child in node.GetChildren())
        {
            //Console.WriteLine(child.GetType());
            
            if (child is SequenceNode sequenceNode)
            {
                codeBuilder("a", "\tpublic static List<List<string>>" + child.ToString() + " { \n");
                codeBuilder("a", "\t\tList<List<string>> framebuffer = new List<List<string>>(); \n");
                Visit(sequenceNode);
            }
        }

        codeBuilder("w", "}  ");


        codeBuilder("w", "public static class Functions");
        codeBuilder("w", "   {");
        insertFuncBoilerplate();
        foreach (var child in node.GetChildren())
        {
            if (child is FunctionDeclarationNode functionDeclarationNode)
            {
                Visit(functionDeclarationNode);
                codeBuilder("w", "");
            }
        }

        codeBuilder("w", "   }");


        codeBuilder("w", "public static class Program");
        codeBuilder("w", "{");
        codeBuilder("w", "   public static void Main()");
        codeBuilder("w", "   {");

        foreach (var child in node.GetChildren())
        {
            if (child.GetType() == typeof(TimelineBlockNode))
            {
                foreach (var timelineChild in child.GetChildren())
                {
                    Visit((FrameDefNode)timelineChild);
                }
            }
        }

        codeBuilder("w", "   }");
        codeBuilder("w", "}");


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
            }
            else if (Child is IdentifierNode identifierNode)
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
        if (node.IsDeclaration)
        {
            codeBuilder("a", $"            {node.VariableType.ToString().ToLower()} {node.Identifier}");
        }
        else
        {
            codeBuilder("a", $"            {node.Identifier}");
        }

        if (node.IsDeclaration)
        {
            codeBuilder("a", " = ");
        }
        else
        {
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
        }


        //insert expression
        Visit(node.Expression);

        codeBuilder("w", ";");


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
        codeBuilder("a", $"           static {node.ReturnType.ToString().ToLower()} {node.Identifier}(");
        foreach (var Child in node.GetChildren())
        {
            if (Child is ParameterNode parameterNode)
            {
                Visit(parameterNode);
            }
        }

        codeBuilder("w", ")");
        codeBuilder("w", "           {");
        foreach (var Child in node.GetChildren())
        {
            if (Child is BlockNode blockNode)
            {
                Visit(blockNode);
            }
        }

        codeBuilder("w", "           }");

        return node;
    }

    public override IASTNode? Visit(FunctionCallNode node)
    {
        return node;
    }

    public override IASTNode? Visit(ParameterNode node)
    {
        codeBuilder("a", $"{node.DataType.ToString().ToLower()} {node.Name}");
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
        codeBuilder("a", "            if (");

        Visit(node.Condition);
        codeBuilder("a", ")");
        codeBuilder("w", "{");
        codeBuilder("a", "   ");
        Visit(node.IfBlock);
        codeBuilder("w", "            }");

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
        codeBuilder("a", "            else if (");
        Visit(node.Condition);
        codeBuilder("a", $")");
        codeBuilder("w", "{");
        codeBuilder("a", "   ");
        Visit(node.ElseIfBlock);
        codeBuilder("w", "            }");

        return node;
    }

    public override IASTNode? Visit(ElseNode node)
    {
        codeBuilder("a", "            else");
        codeBuilder("w", "{");
        codeBuilder("a", "   ");
        Visit(node.ElseBlock);
        codeBuilder("w", "            }");

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
        //Console.WriteLine(node.ToString());
        codeBuilder("a", "\n\n");
        codeBuilder("a", "            for(");

        if (node.Initialization is AssignmentNode assignmentNode)
        {
            Visit(assignmentNode);
        }


        codeBuilder("a", node.Condition.ToString());
        codeBuilder("a", ";");

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
            }

            if (Child is ReturnNode returnNode)
            {
                Visit(returnNode);
            }

            if (Child is BlockNode blockNode)
            {
                Visit(blockNode);
            }
        }

        return node;
    }

    public override IASTNode? Visit(SeqBlockNode node)
    {
        bool hasAnimations = false;
        string groupIdentifier = "";
        string animationsIdentifier = "";
        Dictionary<string, bool> initializationStatusDict = new Dictionary<string, bool>();
        
        //get type
        foreach (var child in node.GetChildren())
        {
            if (child is AssignmentNode assignmentNode)
            {
                string identifier = assignmentNode.Identifier.ToString();

                foreach (var gchild in child.GetChildren())
                {
                    string points = "";
                    if (gchild is ShapeInitNode shapeInitNode)
                    {
                        codeBuilder("a",
                            $"         {shapeInitNode.ShapeType} {identifier} = new {shapeInitNode.ShapeType}");
                        codeBuilder("a", "{");


                        if (shapeInitNode.ShapeType.ToString() == "Circle")
                        {
                            int parameterCount = 0;
                            foreach (var arg in shapeInitNode.Arguments)
                            {
                                if (arg.Value is FunctionCallNode functionCallNode)
                                {
                                    if (parameterCount > 0)
                                    {
                                        codeBuilder("a", ",");
                                    }

                                    codeBuilder("a", $"{arg.Key} = Functions.{functionCallNode.FunctionIdentifier.ToString().ToLower()}(");
                                    parameterCount++;
                                    int Count = 0;
                                    foreach (var Farg in functionCallNode.Arguments)
                                    {
                                        if (Count > 0)
                                        {
                                            codeBuilder("a", ",");
                                        }

                                        codeBuilder("a", $"{Farg.ToString()}");
                                        Count++;
                                    }

                                    codeBuilder("a", ")");

                                    //Console.WriteLine(functionCallNode);
                                }

                                if (arg.Value is TupleNode tupleNode)
                                {
                                    if (parameterCount > 0)
                                    {
                                        codeBuilder("a", ",");
                                    }


                                    codeBuilder("a", $"{arg.Key} = ({arg.Value})");
                                    parameterCount++;
                                    //Console.WriteLine(tupleNode);
                                }

                                if (arg.Value is IntegerLiteralNode integerLiteralNode)
                                {
                                    if (parameterCount > 0)
                                    {
                                        codeBuilder("a", ",");
                                    }

                                    codeBuilder("a", $"{arg.Key} = {integerLiteralNode}");
                                    parameterCount++;
                                    //Console.WriteLine(integerLiteralNode);
                                }

                                if (arg.Value is FloatLiteralNode floatLiteralNode)
                                {
                                    if (parameterCount > 0)
                                    {
                                        codeBuilder("a", ",");
                                    }


                                    codeBuilder("a", $"{arg.Key} = {floatLiteralNode}");
                                    parameterCount++;
                                    //Console.WriteLine(floatLiteralNode);
                                }
                            }
                        }
                        
                        if (shapeInitNode.ShapeType.ToString() == "Polygon")
                        {
                            
                            foreach (var arg in shapeInitNode.Arguments)
                            {
                                int parameterCount = 0;

                                if (arg.Value is FunctionCallNode functionCallNode)
                                {
                                    if (parameterCount > 0)
                                    {
                                        codeBuilder("a", ",");
                                    }

                                    codeBuilder("a", $"{arg.Key} = Functions.{functionCallNode.FunctionIdentifier}(");
                                    parameterCount++;
                                    int Count = 0;
                                    foreach (var Farg in functionCallNode.Arguments)
                                    {
                                        if (Count > 0)
                                        {
                                            codeBuilder("a", ",");
                                        }

                                        codeBuilder("a", $"{Farg.ToString()}");
                                        Count++;
                                    }

                                    codeBuilder("a", ")");

                                    //Console.WriteLine(functionCallNode);
                                }

                                if (arg.Value is TupleNode tupleNode)
                                {
                                    if (parameterCount > 0)
                                    {
                                        codeBuilder("a", ",");
                                    }

                                    if (Regex.IsMatch(arg.Key.ToString(), @"point\d+"))
                                    {
                                        //Console.WriteLine(arg.Value);
                                        if (points != "")
                                        {
                                            points += ",";
                                        }
                                        
                                        points += $"new double[2] {{{arg.Value}}}";
                                    }
                                    else
                                    {
                                        codeBuilder("a", $"{arg.Key} = ({arg.Value})");
                                    }
                                    
                                    parameterCount++;
                                    //Console.WriteLine(tupleNode);
                                }

                                if (arg.Value is IntegerLiteralNode integerLiteralNode)
                                {
                                    if (parameterCount > 0)
                                    {
                                        codeBuilder("a", ",");
                                    }

                                    codeBuilder("a", $"{arg.Key} = {integerLiteralNode}");
                                    parameterCount++;
                                    //Console.WriteLine(integerLiteralNode);
                                }

                                if (arg.Value is FloatLiteralNode floatLiteralNode)
                                {
                                    if (parameterCount > 0)
                                    {
                                        codeBuilder("a", ",");
                                    }


                                    codeBuilder("a", $"{arg.Key} = {floatLiteralNode}");
                                    parameterCount++;
                                    //Console.WriteLine(floatLiteralNode);
                                }
                            }
                            
                            
                        }
                        
                        
                        
                        codeBuilder("a", "};");
                        codeBuilder("w", "");

                        if (shapeInitNode.ShapeType.ToString() == "Polygon")
                        {
                            codeBuilder("w", $"List<double[]> {identifier}Points = new List<double[]>");
                            codeBuilder("w", "{");
                            codeBuilder("w", points);
                            codeBuilder("w", "};");
                            
                            codeBuilder("w", $"{identifier}.points = {identifier}Points;");
                        }
                        
           

                    }
                }
            }
        }

        foreach (var child in node.GetChildren())
        {
            if (child is AnimationNode animationNode)
            {
                bool isItem1Initialized;
                if (initializationStatusDict.TryGetValue($"{child.ToString()}", out isItem1Initialized))
                {
                    Console.WriteLine($"{child.ToString()} is already initialized");
                    codeBuilder("w", $"{child.ToString()}Animations.Clear();");
                }
                else
                {
                    // The key does not exist in the dictionary
                    codeBuilder("w", $"List<Animation> {child.ToString()}Animations = new List<Animation>();");
                    initializationStatusDict[$"{child.ToString()}"] = true;
                }
                
               
                animationsIdentifier = $"{child.ToString()}Animations";
                groupIdentifier = $"{child.ToString()}";
                foreach (var transition in animationNode.Transitions)
                {
                    if (transition is TransitionNode transitionNode)
                    {
                        codeBuilder("w", $"{child.ToString()}Animations.Add(");
                        Visit(transition);
                        codeBuilder("a", ");");
                        hasAnimations = true;
                    }
                    
                    
                }
                
            }
            
            if (hasAnimations)
            {
                codeBuilder("w", $"framebuffer = Functions.animate({groupIdentifier}, {animationsIdentifier}, framebuffer, frameoffset);");
            }
            
            if (child is IdentifierGroupingNode identifierGroupingNode)
            {
                codeBuilder("a", $"group {identifierGroupingNode.Identifier} = new group();\n");

                foreach (var expressionNode in identifierGroupingNode.GroupingElements.Expressions)
                {
                    //Console.WriteLine(expressionNode);

                    codeBuilder("w",
                        $"{identifierGroupingNode.Identifier}.Add(\"{expressionNode}\", {expressionNode});");
                }

                // foreach (var VARIABLE in identifierGroupingNode.GroupingElements.KeyValuePairs)
                // {
                //     Console.WriteLine(VARIABLE.NodeType);
                // }
                //
                // foreach (var VARIABLE in identifierGroupingNode.GroupingElements.Identifiers)
                // {
                //     Console.WriteLine(VARIABLE.NodeType);
                // }
            }
        }

        
        
       
        
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
                switch (node.OperatorNode.OperatorSymbol)
                {
                    case "and":
                        codeBuilder("a", " && ");
                        break;
                    case "or":
                        codeBuilder("a", " || ");
                        break;
                    default:
                        codeBuilder("a", " " + operatorNode.OperatorSymbol + " ");
                        break;
                }
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
        string sequenceCallWithoutParams =
            node.SequenceCall.ToString().Substring(0, node.SequenceCall.ToString().IndexOf('(') + 1);
        string sequenceCallParams =
            node.SequenceCall.ToString().Substring(node.SequenceCall.ToString().IndexOf('(') + 1);
        
        if (sequenceCallParams.Substring(0, sequenceCallParams.IndexOf(')')) != "")
        {
            sequenceCallParams = $", {sequenceCallParams}";
        }

        codeBuilder("w", $"\t \t \t Sequences.{sequenceCallWithoutParams}{node.FrameTime}{sequenceCallParams};");

        foreach (var child in node.GetChildren())
        {
            if (child is SequenceCallNode sequenceCallNode)
            {
                Visit(sequenceCallNode);
            }
            
        }
        
        
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
        if (node.ToString() != "")
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
        foreach (var child in node.GetChildren())
        {
            //Console.WriteLine(child.GetType());
            if (child is SeqBlockNode seqBlockNode)
            {
                Visit(seqBlockNode);
            }

            if (child is IdentifierNode identifierNode)
            {
                // Console.WriteLine(child);
            }


            // if (child.GetType() == typeof(SeqBlockNode)) {
            //     foreach (var grandChild in child.GetChildren()) {
            //         
            //         if (grandChild.GetType() != typeof(AnimationNode)) {
            //             codeBuilder("a", $"\t\t{grandChild.ToString()} \n");
            //         }
            //         if (grandChild.GetType() == typeof(AnimationNode)) {
            //             codeBuilder("a", $"\t\tframeBuffer = renderAnimation({grandChild.ToString()}, frameOffset, frameBuffer); \n");
            //         }
            //         Console.WriteLine($"\t {grandChild.ToString()}");
            //
            //     }
            // }
            //Visit((SeqBlockPartNode)child);
        }

        codeBuilder("a", "\t\treturn framebuffer; \n \t} \n");

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
        List<string> arguements = new List<string>();
        
        foreach (var argument in node.GetChildren())
        {
            if (argument is ArgumentNode argumentNode)
            {
                if (argumentNode.Value is FunctionCallNode)
                {
                    string argKeyValue = $"{argumentNode.Name.Replace(":", "").ToLower()} = Functions.{argumentNode.Value} ";
                    arguements.Add(argKeyValue);
                }
                else
                {
                    string argKeyValue = $"{argumentNode.Name.Replace(":", "").ToLower()} = {argumentNode.Value} ";
                    arguements.Add(argKeyValue);
                }
                
                //Console.WriteLine(argument);
                
            }
        }

        codeBuilder("a", "new Animation {");
        
        for (int i = 0; i < arguements.Count; i++)
        {
            if (i != 0)
            {
                codeBuilder("a", ",");
            }
            
            string argOut = arguements[i].ToString();
            
            codeBuilder("a", $"{argOut}");
            
        }
        
        codeBuilder("a", "} ");

        
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