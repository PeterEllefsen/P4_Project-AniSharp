namespace AnimationLanguage.ASTCommon;

//This struct is used to store the location of a node in the source code, which is useful for error reporting.
public struct SourceLocation
{
    public int Line { get; set; } //What line the node is on
    public int Column { get; set; } //What column the node is on
    public static SourceLocation None { get; } = new SourceLocation(0, 0);

    
    public SourceLocation(int line, int column)
    {
        Line = line;
        Column = column;
    }
    
    public override string ToString() //This is used to print the location of the node in the console when calling the ToString() method on the source location of a node.
    {
        return $"Line: {Line}, Column: {Column}";
    }
}