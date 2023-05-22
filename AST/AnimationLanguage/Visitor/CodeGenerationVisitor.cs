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
        using (var fs = File.Create("../../codegen/Program.cs", 1024))
        {
        }
    }

    private void codeBuilder(string p, string appendingString)
    {
        var path = "";
        path = "../../codegen/Program.cs";


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
    public double borderWidth { get; set; }

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
        
        
        codeBuilder("w",@"public class setup
{
    public int sceneWidth = 500;
    public int sceneHeight = 300;
    public int framerate = 60;
    public string backgroundColor = ""#ffffff"";
    }");
    }

    private void insertFuncBoilerplate()
    {
        codeBuilder("w", @"    private static void codeBuilder(string p, string appendingString)
    {
        var path = """";
        path = ""output.html"";


        if (p == ""a"")
        {
            using (var sw = File.AppendText(path))
            {
                sw.Write(appendingString);
            }
        }
        else if (p == ""w"")
        {
            using (var sw = File.AppendText(path))
            {
                sw.WriteLine(appendingString);
            }
        }
    }
    ");
        
        codeBuilder("w", @"public static string rgb(int red, int green, int blue)
{
    string hex = $""#{red:X2}{green:X2}{blue:X2}"";
    return hex;
}");

        codeBuilder("w", @"    public static void PrintFramebuffer(List<List<string>> framebuffer, setup setup)
    {
        string frames = """";
        
        for (int frameIndex = 0; frameIndex < framebuffer.Count; frameIndex++)
        {
            //Console.WriteLine($""Frame {frameIndex + 1}:"");
            string frameContent = """";
            int contentCount = 0;
            foreach (string details in framebuffer[frameIndex])
            {
                if (contentCount > 0)
                {
                    frameContent += $"", '{details}'"";
                }
                else
                {
                    frameContent += $""'{details}'"";
                }

                contentCount++;
            }

            frames +=  $""frames.push([{frameContent}]); \n"";
        }
        
        
        codeBuilder(""w"", @$""<!DOCTYPE html>
<html>
<head>
    <title>My Canvas</title>
    <style>
        body {{
            margin: 0;
            padding: 0;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            height: 100vh;
            background: linear-gradient(135deg, #6C63FF, #2D91FF);
        }}

        #myCanvas.active {{
            transform: scale(1);
        }}

        #myCanvas {{
            background-color: {setup.backgroundColor};
            border-radius: 8px;
            max-width: 100%;
            max-height: 80vh;
            transform: scale(0);
            transition: transform 0.3s ease;
        }}

        #nextSlideBtn {{
            margin-top: 20px;
            padding: 12px 24px;
            font-size: 16px;
            background-color: #4CAF50;
            color: #FFFFFF;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.3);
            transition: background-color 0.3s ease, transform 0.3s ease;
        }}

        #nextSlideBtn:hover {{
            background-color: #45A049;
        }}

        #nextSlideBtn:active {{
            transform: scale(0.95);
        }}

        #nextSlideBtn.disabled {{
            cursor: not-allowed;
            background-color: gray;
        }}

        #frameInfo {{
            margin-top: 20px;
            font-size: 14px;
            color: #FFFFFF;
        }}

        #progressBar {{
            width: 80%;
            height: 14px;
            background-color: #E5E5E5;
            border-radius: 4px;
            margin-top: 20px;
            overflow: hidden;
        }}

        #progressBar .progress {{
            height: 100%;
            background-color: #4CAF50;
            transition: width 0.3s ease;
        }}
    </style>
</head>
<body>
<div id=""""frameInfo""""></div>
<canvas id=""""myCanvas"""" width=""""{setup.sceneWidth}"""" height=""""{setup.sceneHeight}"""">Your browser does not support the HTML canvas tag.</canvas>
<div id=""""progressBar"""">
    <div class=""""progress""""></div>
</div>

<button id=""""nextSlideBtn"""" onclick=""""playAnimation()"""">PLAY</button>


<script>
    var currentFrame = 0;
    var frames = [];
    {frames}

    var canvas = document.getElementById('myCanvas');
    var ctx = canvas.getContext('2d');
    var frameInfo = document.getElementById('frameInfo');
    var button = document.getElementById('nextSlideBtn');
    var progressBar = document.querySelector('#progressBar .progress');

    function updateFrameInfo() {{
        frameInfo.textContent = '{setup.framerate} - Frame: ' + (currentFrame + 1) + ' / ' + frames.length;
    }}

    function updateProgressBar() {{
        var progress = (currentFrame / (frames.length - 1)) * 100;
        progressBar.style.width = progress + '%';
    }}

    function playAnimation() {{
        canvas.classList.add('active');
        button.disabled = true;
        button.classList.add('disabled');

        updateFrameInfo();
        updateProgressBar();
        run(0);
    }}

    function sleep(ms) {{
        return new Promise(resolve => setTimeout(resolve, ms));
    }}

    function blankCanvas() {{
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    }}

    function drawCircle(x, y, radius, borderWidth, color) {{
        ctx.beginPath();
        ctx.fillStyle = color;
        ctx.arc(x, y, radius, 0, 2 * Math.PI);
        ctx.fill();
        ctx.lineWidth = borderWidth;
        ctx.strokeStyle = """"#000000"""";
        ctx.stroke();
        ctx.closePath();
        ctx.fillStyle = """"#000000"""";
    }}

    function drawPolygon(points, borderWidth, color) {{
        let polygon = new Path2D();
        for (var i = 0; i < points.length / 2; i++) {{
            if (i === 0) {{
                polygon.moveTo(points[i * 2], points[2 * i + 1]);
                polygon.lineTo(points[(i + 1) * 2], points[2 * (i + 1) + 1]);
            }} else {{
                polygon.lineTo(points[i * 2], points[2 * i + 1]);
            }}
        }}
        polygon.closePath();
        ctx.lineWidth = borderWidth;
        ctx.fillStyle = color;
        ctx.strokeStyle = """"#000000"""";
        ctx.fill(polygon);
        ctx.stroke(polygon);
        ctx.fillStyle = """"#000000"""";
    }}

    function drawText(x, y, text, fontsize, color, font) {{
        ctx.font = fontsize + """"px """" + font;
        ctx.fillText(text, x, y);
    }}

    function run(frame) {{
        if (frame === frames.length) {{
            blankCanvas();
            button.disabled = false;
            button.className = """"active"""";
            currentFrame = 0;
            updateFrameInfo();
            updateProgressBar();
            return;
        }}

        sleep({1000 / setup.framerate})
            .then(() => {{
                blankCanvas();
                for (var j = 0; j < frames[frame].length; j++) {{
                    var obj = frames[frame][j].split(""""|"""");
                    if (obj[0] === """"circle"""") {{
                        drawCircle(parseFloat(obj[1]), parseFloat(obj[2]), parseFloat(obj[3]), parseFloat(obj[4]), obj[5]);
                    }} else if (obj[0].includes(""""polygon"""")) {{
                        var points = [];
                        var pointsAmount = parseInt(obj[0].substring(7)) + 1;
                        for (var i = 1; i < pointsAmount; i++) {{
                            points.push(parseFloat(obj[i * 2 - 1]));
                            points.push(parseFloat(obj[i * 2]));
                        }}
                        drawPolygon(points, parseFloat(obj[obj.length - 2]), obj[obj.length - 1]);
                    }}
                }}
                currentFrame = frame;
                updateFrameInfo();
                updateProgressBar();
                run(frame + 1);
            }});
    }}
</script>
</body>
</html>
"");}");
        
        
        codeBuilder("w", @"    public static List<List<string>> mergeFramebuffer(List<List<string>> framebuffer1, List<List<string>> framebuffer2)
    {
        int maxframes = framebuffer1.Count >= framebuffer2.Count ? framebuffer2.Count : framebuffer1.Count;

        if (framebuffer1.Count != 0)
        {
            if (framebuffer1.Count > framebuffer2.Count)
            {
                for (int frameIndex = 0; frameIndex < framebuffer2.Count; frameIndex++)
                {
                    framebuffer1[frameIndex].AddRange(framebuffer2[frameIndex]);
                }

                return framebuffer1;
            }
            else if (framebuffer2.Count > framebuffer1.Count || framebuffer1.Count == framebuffer2.Count)
            {
                for (int frameIndex = 0; frameIndex < framebuffer1.Count; frameIndex++)
                {
                    framebuffer2[frameIndex].AddRange(framebuffer1[frameIndex]);
                }

                return framebuffer2;
            }
        }else
        {
            return framebuffer2;
        }

        return framebuffer2;

    }");
        
        codeBuilder("w", @" public static List<List<string>> animate(object item, List<Animation> animations, List<List<string>> framebuffer,
        int frameOffset)
    {
        int totalFrames = 0;
        double dx = 0;
        double dy = 0;
        double drad = 0;
        double dbw = 0;
        int dr = 0;
        int dg = 0;
        int db = 0;

        foreach (var animation in animations)
        {
            if (animation.endframe > totalFrames)
            {
                totalFrames = animation.endframe + frameOffset;
            }
        }
        
        //wait until start frame
        for (int frame = framebuffer.Count; frame < totalFrames; frame++)
        {
            framebuffer.Add(new List<string>()); //wait
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


                if (animations[i].x != null)
                {
                    dx = (double)animations[i].x / (animationFrameCount - 1);
                }

                if (animations[i].y != null)
                {
                    dy = (double)animations[i].y / (animationFrameCount - 1);
                }

                if (animations[i].radius != null)
                {
                    double startRadius = circle.radius;
                    double endRadius = (double)animations[i].radius;

                    drad = (endRadius - startRadius) / (animationFrameCount - 1);
                }

                if (animations[i].borderWidth != null)
                {
                    double startBorderWidth = circle.borderWidth;
                    double endBorderWidth = (double)animations[i].borderWidth;

                    dbw = (endBorderWidth - startBorderWidth) / (animationFrameCount - 1);
                }

                if (animations[i].color != null)
                {
                    (int r, int g, int b) startColor = HexToRgb(circle.color);
                    (int r, int g, int b) endColor = HexToRgb(animations[i].color);

                    // Calculate the color change per frame for each RGB component
                    dr = (endColor.r - startColor.r) / (animationFrameCount - 1);
                    dg = (endColor.g - startColor.g) / (animationFrameCount - 1);
                    db = (endColor.b - startColor.b) / (animationFrameCount - 1);
                }


                for (int frame = 0; frame < animationFrameCount; frame++)
                {
                    if (frame != 0)
                    {
                        if (animations[i].color != null)
                        {
                            (int r, int g, int b) currentColorHex = HexToRgb(circle.color);
                            // Animate the color for each frame
                            int currentR = currentColorHex.r + dr;
                            int currentG = currentColorHex.g + dg;
                            int currentB = currentColorHex.b + db;

                            // Convert the RGB values back to a hex color code
                            string currentColor = rgb(currentR, currentG, currentB);

                            // Animate the color property
                            circle.color = currentColor;
                        }

                        if (animations[i].x != null)
                        {
                            // Animate the x-coordinate for each frame
                            double currentX = circle.center.Item1 + dx;
                            circle.center = (currentX, circle.center.Item2);
                        }

                        if (animations[i].y != null)
                        {
                            // Animate the y-coordinate for each frame
                            double currentY = circle.center.Item2 + dy;
                            circle.center = (circle.center.Item1, currentY);
                        }

                        if (animations[i].radius != null)
                        {
                            // Animate the radius for each frame
                            double currentRadius = circle.radius + drad;
                            circle.radius = currentRadius;
                        }

                        if (animations[i].borderWidth != null)
                        {
                            // Animate the borderWidth for each frame
                            double currentBorderWidth = circle.borderWidth + dbw;
                            circle.borderWidth = currentBorderWidth;
                        }
                    }


                    int frameIndex = frameOffset + (i == 0 ? 0 : animations[i - 1].endframe) + frame;
                    


                    //Add the frame to the framebuffer
                    framebuffer[frameOffset + (i == 0 ? 0 : animations[i - 1].endframe) + frame].Add(
                        $""circle|{circle.center.Item1}|{circle.center.Item2}|{circle.radius}|{circle.borderWidth}|{circle.color}"");
                }
            }
        }
        else if (item is Polygon polygon)
        {
            for (int i = 0; i < animations.Count; i++)
            {
                int animationFrameCount =
                    i == 0 ? animations[i].endframe : animations[i].endframe - animations[i - 1].endframe;

                // Create a new list for the frame in the framebuffer
                List<string> frameList = new List<string>();


                if (animations[i].x != null)
                {
                    dx = (double)animations[i].x / (animationFrameCount - 1);
                }

                if (animations[i].y != null)
                {
                    dy = (double)animations[i].y / (animationFrameCount - 1);
                }

                if (animations[i].borderWidth != null)
                {
                    double startBorderWidth = polygon.borderWidth;
                    double endBorderWidth = (double)animations[i].borderWidth;

                    dbw = (endBorderWidth - startBorderWidth) / (animationFrameCount - 1);
                }

                if (animations[i].color != null)
                {
                    (int r, int g, int b) startColor = HexToRgb(polygon.color);
                    (int r, int g, int b) endColor = HexToRgb(animations[i].color);

                    // Calculate the color change per frame for each RGB component
                    dr = (endColor.r - startColor.r) / (animationFrameCount - 1);
                    dg = (endColor.g - startColor.g) / (animationFrameCount - 1);
                    db = (endColor.b - startColor.b) / (animationFrameCount - 1);
                }


                for (int frame = 0; frame < animationFrameCount; frame++)
                {
                    if (frame != 0)
                    {
                        if (animations[i].color != null)
                        {
                            (int r, int g, int b) currentColorHex = HexToRgb(polygon.color);
                            // Animate the color for each frame
                            int currentR = currentColorHex.r + dr;
                            int currentG = currentColorHex.g + dg;
                            int currentB = currentColorHex.b + db;

                            // Convert the RGB values back to a hex color code
                            string currentColor = rgb(currentR, currentG, currentB);

                            // Animate the color property
                            polygon.color = currentColor;
                        }

                        if (animations[i].x != null)
                        {
                            // Animate the x-coordinate for each frame
                            foreach (var point in polygon.points)
                            {
                                point[0] += dx;
                            }
                        }

                        if (animations[i].y != null)
                        {
                            // Animate the y-coordinate for each frame
                            foreach (var point in polygon.points)
                            {
                                point[1] += dy;
                            }
                        }

                        if (animations[i].borderWidth != null)
                        {
                            // Animate the borderWidth for each frame
                            double currentBorderWidth = polygon.borderWidth + dbw;
                            polygon.borderWidth = currentBorderWidth;
                        }
                    }
                    
                    string polygonString = """";
                    polygonString += $""polygon{polygon.points.Count}|"";
                    foreach (var point in polygon.points)
                    {
                        polygonString += $""{point[0]}|{point[1]}|"";
                    }

                    polygonString += $""{polygon.borderWidth}|{polygon.color}"";
                    //Add the frame to the framebuffer
                    framebuffer[frameOffset + (i == 0 ? 0 : animations[i - 1].endframe) + frame].Add(polygonString);
                }
            }
        }

        return framebuffer;
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
        
        codeBuilder("w", @"private static void CreateFilesForCompilation()
    {
        //if file exists delete it
        if (File.Exists(""output.html"")) File.Delete(""output.html"");
        //files to create
        //Program.cs for main
        //function class for all functions
        //Sequence class containing methods that create sequences
        using (var fs = File.Create(""output.html"", 1024))
        {
        }
    }");
        
        codeBuilder("w", "   public static void Main()");
        codeBuilder("w", "   {");
        codeBuilder("w", "setup setup = new setup();");
        codeBuilder("w", "CreateFilesForCompilation();");

        codeBuilder("w", "List<List<string>> framebuffer = new List<List<string>>();");

        foreach (var child in node.GetChildren())
        {
            if (child is SetupNode setupNode)
            {
                Visit(setupNode);
            }
            
            if (child.GetType() == typeof(TimelineBlockNode))
            {
                foreach (var timelineChild in child.GetChildren())
                {
                    Visit((FrameDefNode)timelineChild);
                }
            }
        }
        
        codeBuilder("w", "Functions.PrintFramebuffer(framebuffer, setup);");

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
           Console.WriteLine(Child);

           if (Child is IdentifierNode identifierNode)
           {
               codeBuilder("w", $"setup.{identifierNode.ToString()} =");
           }

           if (Child is IntegerLiteralNode integerLiteralNode)
           {
               codeBuilder("a", $"{integerLiteralNode.ToString()};");

           }

           if (Child is StringLiteralNode stringLiteralNode)
           {
               codeBuilder("a", $"{stringLiteralNode.ToString()};");
           }

           if (Child is FunctionCallNode functionCallNode)
           {
               codeBuilder("a", $"Functions.{functionCallNode.ToString().ToLower()};");
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
        codeBuilder("a", $"           public static {node.ReturnType.ToString().ToLower()} {node.Identifier}(");
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
                                    /*
                                    if (parameterCount > 0)
                                    {
                                        codeBuilder("a", ",");
                                    }
                                    */

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
                                    
                                    //parameterCount++;
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
                   // Console.WriteLine($"{child.ToString()} is already initialized");
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

        codeBuilder("w", $"\t \t \tframebuffer = Functions.mergeFramebuffer(framebuffer, Sequences.{sequenceCallWithoutParams}{node.FrameTime}{sequenceCallParams});");

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
        
        //Console.WriteLine(node);
        
        foreach (var argument in node.GetChildren())
        {
            //Console.WriteLine(argument);
            
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