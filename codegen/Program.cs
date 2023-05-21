using System;

namespace AnimationLanguage;

public class Circle{
      public (double, double) center { get; set; }
    public double radius { get; set; }
    public string color { get; set; }
    public double borderWidth { get; set; }

         public Circle()
        {
        center = (0,0); // Default center
        radius = 1; // Default radius
        color = "#000000"; // Default color
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
        color = "#000000"; // Default color
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

public class Animation
{
    public int endframe { get; set; }
    public int? x { get; set; }
    public int? y { get; set; }
    public int? radius { get; set; }
    public string? color { get; set; }
    public int? borderWidth { get; set; }
}
public class setup
{
    public int sceneWidth = 500;
    public int sceneHeight = 300;
    public int framerate = 60;
    public string backgroundColor = "#ffffff";
    }
public static class Sequences {
	public static List<List<string>>CarDrivingOnScreen(int frameoffset, int i) { 
		List<List<string>> framebuffer = new List<List<string>>(); 
         Circle circleHeadlight = new Circle{center = (327, 135),radius = 10,color = Functions.rgb(255,255,0)};
         Circle circle1wheel = new Circle{center = (187, 170),radius = 25,color = Functions.rgb(0,0,0)};
         Circle circle1wheelinner = new Circle{center = (187, 170),radius = 125,color = Functions.rgb(165,165,165)};
         Circle circle2wheel = new Circle{center = (276, 157),radius = 25,color = Functions.rgb(0,0,0)};
         Circle circle2wheelinner = new Circle{center = (276, 157),radius = 125,color = Functions.rgb(165,165,165)};
         Circle cirkel1 = new Circle{radius = 50,borderWidth = 3,center = (200, 50)};
group car = new group();
car.Add("circle1wheel", circle1wheel);
car.Add("circle1wheelinner", circle1wheelinner);
car.Add("circle2wheel", circle2wheel);
car.Add("circle2wheelinner", circle2wheelinner);
car.Add("circleHeadlight", circleHeadlight);
List<Animation> carAnimations = new List<Animation>();
carAnimations.Add(
new Animation {endframe = 180 ,x = 250 } );carAnimations.Add(
new Animation {endframe = 180 ,x = 250 } );carAnimations.Add(
new Animation {endframe = 180 ,x = 250 } );framebuffer = Functions.animate(car, carAnimations, framebuffer, frameoffset);
		return framebuffer; 
 	} 
	public static List<List<string>>CircleGoVroomInTriangle(int frameoffset, float j) { 
		List<List<string>> framebuffer = new List<List<string>>(); 
         Circle cirkel1 = new Circle{radius = 50,borderWidth = 3,center = (200, 50)};
List<Animation> cirkel1Animations = new List<Animation>();
cirkel1Animations.Add(
new Animation {endframe = 50 ,x = 50 ,color = Functions.ColorBasedOnNumber(4) } );cirkel1Animations.Add(
new Animation {endframe = 100 ,x = 50 ,y = -50 } );cirkel1Animations.Add(
new Animation {endframe = 150 ,y = -50 } );framebuffer = Functions.animate(cirkel1, cirkel1Animations, framebuffer, frameoffset);
		return framebuffer; 
 	} 
}  
public static class Functions
   {
    private static void codeBuilder(string p, string appendingString)
    {
        var path = "";
        path = "output.html";


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
    
public static string rgb(int red, int green, int blue)
{
    string hex = $"#{red:X2}{green:X2}{blue:X2}";
    return hex;
}
    public static void PrintFramebuffer(List<List<string>> framebuffer, setup setup)
    {
        string frames = "";
        
        for (int frameIndex = 0; frameIndex < framebuffer.Count; frameIndex++)
        {
            //Console.WriteLine($"Frame {frameIndex + 1}:");
            string frameContent = "";
            int contentCount = 0;
            foreach (string details in framebuffer[frameIndex])
            {
                if (contentCount > 0)
                {
                    frameContent += $", '{details}'";
                }
                else
                {
                    frameContent += $"'{details}'";
                }

                contentCount++;
            }

            frames +=  $"frames.push([{frameContent}]); \n";
        }
        
        
        codeBuilder("w", @$"<!DOCTYPE html>
<html>
<body>

<canvas id=""myCanvas"" width=""{setup.sceneWidth}"" height=""{setup.sceneHeight}"" style=""border:1px solid {setup.backgroundColor};"">
Your browser does not support the HTML canvas tag.</canvas>

<button onclick=""run(0)"">NEXT SLIDE</button>


<script>
    var frames = [];
    {frames}


    function sleep(ms) {{
        return new Promise(resolve => setTimeout(resolve, ms));
    }}

    function blankCanvas() {{
        ctx.clearRect(0, 0, c.width, c.height);
    }}

    function drawCircle(x, y, radius, borderwidth, color) {{
        ctx.beginPath();
        ctx.fillStyle = color;
        ctx.arc(x, y, radius, 0, 2*Math.PI);
        ctx.fill();
        ctx.lineWidth = borderwidth;
        ctx.strokeStyle = ""#000000"";
        ctx.stroke();
        ctx.closePath();

        ctx.fillStyle = ""#000000"";
    }}
    function drawPolygon(points, borderwidth, color) {{
        
        let polygon = new Path2D();
        for (var i = 0; i < (points.length / 2); i++) {{
            if (i == 0) {{
                polygon.moveTo(points[i*2], points[(2 * i)+1]);
                polygon.lineTo(points[(i+1)*2], points[(2 * (i+1))+1])
            }} else {{
                polygon.lineTo(points[i*2], points[(2 * i)+1]);
                
            }}
        }}
        polygon.closePath();
        ctx.lineWidth = borderwidth;
        ctx.fillStyle = color;
        ctx.strokeStyle = ""#000000"";
        ctx.fill(polygon);
        ctx.stroke(polygon);
        ctx.fillStyle = ""#000000"";
    }}

    function drawText (x, y, text, fontsize, color, font) {{
        ctx.font = fontsize + ""px "" + font;
        ctx.fillText(text,x,y);
    }}
    var c = document.getElementById(""myCanvas"");
    var ctx = c.getContext(""2d"");

    function run(frame) {{
        if (frame != frames.length) {{
            sleep({1000 / setup.framerate})
                .then(() => blankCanvas())
                .then(() => run(frame + 1))
            for (j = 0; j < frames[frame].length; j++) {{
                obj = frames[frame][j].split(""|"");
                console.log(obj)
                if (obj[0] == ""circle"") {{
                    drawCircle(Math.round(obj[1].replace(/\,/g,""."")), Math.round(obj[2].replace(/\,/g,""."")),  Math.round(obj[3].replace(/\,/g,""."")), Math.round(obj[4]), obj[5]);
                }} else if (obj[0].includes(""polygon"") ) {{
                    console.log(obj[0].substring(7));
                    var points = [];
                    var pointsAmount = Number(obj[0].substring(7)) + 1;
                    for (var i = 1; i < pointsAmount; i++) {{
                        points.push(obj[(i*2)-1]);
                        points.push(obj[i*2]);
                        
                    }}

                    drawPolygon(points, obj[obj.length-2], obj[obj.length - 1]);
                }}
            }}

        }} else {{
            blankCanvas();
        }}
    }}

</script> 

</body>
</html>"); 
}
 public static (int, int, int) HexToRgb(string hex)
        {
            // Remove the '#' symbol if present
            hex = hex.TrimStart('#');

            // Convert the hex code to RGB values
            int r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            return (r, g, b);
        }
    public static List<List<string>> mergeFramebuffer(List<List<string>> framebuffer1, List<List<string>> framebuffer2)
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

    }
 public static List<List<string>> animate(object item, List<Animation> animations, List<List<string>> framebuffer,
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
                        $"circle|{circle.center.Item1}|{circle.center.Item2}|{circle.radius}|{circle.borderWidth}|{circle.color}");
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
                    
                    string polygonString = "";
                    polygonString += $"polygon{polygon.points.Count}|";
                    foreach (var point in polygon.points)
                    {
                        polygonString += $"{point[0]}|{point[1]}|";
                    }

                    polygonString += $"{polygon.borderWidth}|{polygon.color}";
                    //Add the frame to the framebuffer
                    framebuffer[frameOffset + (i == 0 ? 0 : animations[i - 1].endframe) + frame].Add(polygonString);
                }
            }
        }

        return framebuffer;
    }
           public static string ColorBasedOnNumber(int f)
           {
            int i = 1;
            int a = 1 + 5;
            int j = 1;
            bool b = true;
            i = j;

            if (i < 2){
               i = 1;
            }
            else if (b == true){
               i = 3;
            }
            int hej = 5;


            for(            i = 1;
(i < 3);i--){            i = 5;
}
            return "#FFFFFF";
           }

   }
public static class Program
{
private static void CreateFilesForCompilation()
    {
        //if file exists delete it
        if (File.Exists("output.html")) File.Delete("output.html");
        //files to create
        //Program.cs for main
        //function class for all functions
        //Sequence class containing methods that create sequences
        using (var fs = File.Create("output.html", 1024))
        {
        }
    }
   public static void Main()
   {
setup setup = new setup();
CreateFilesForCompilation();
List<List<string>> framebuffer = new List<List<string>>();
setup.sceneWidth =
500;setup.sceneHeight =
300;setup.framerate =
60;setup.backgroundColor =
Functions.rgb(255, 255, 255);	 	 	framebuffer = Functions.mergeFramebuffer(framebuffer, Sequences.CircleGoVroomInTriangle(1, 75));
	 	 	framebuffer = Functions.mergeFramebuffer(framebuffer, Sequences.CarDrivingOnScreen(10, 4));
Functions.PrintFramebuffer(framebuffer, setup);
   }
}
