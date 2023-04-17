namespace AnimationLanguage.ASTNodes;
using ASTCommon;

//This class represents a statement node in the AST. A statement can be any of the following:
// - An assignment statement (e.g. circle1 = 5)
// - A function call statement (e.g. print("Cool animation bro!"))
// - An if statement (e.g. if (car.headlight.radius == 5) { print("headlight is 5px"); })
// - A for statement (e.g. for (int i = 0; i < 7; i++) { print(i); })
// - A while statement (e.g. while (x <= 10) { x = x + 1; })
// - A return statement (e.g. return (x + 5))
public class StatementNode : IASTNode
{
    public SourceLocation SourceLocation { get; set; }
    public NodeType NodeType => NodeType.Statement;
    public IList<IASTNode> Children { get; } = new List<IASTNode>();

    // Different types of statements that can be part of a StatementNode
    public AssignmentNode? Assignment { get; set; } // Represents an assignment statement.
    public FunctionCallNode? FunctionCall { get; set; } // Represents a function call statement.
    public IfStatementNode? IfStatement { get; set; } // Represents an if statement.
    public ForLoopNode? ForStatement { get; set; } // Represents a for statement.
    public WhileLoopNode? WhileStatement { get; set; } // Represents a while statement.
    public ReturnNode? ReturnStatement { get; set; } // Represents a return statement.

    public StatementNode(
        AssignmentNode? assignment,
        FunctionCallNode? functionCall,
        IfStatementNode? ifStatement,
        ForLoopNode? forStatement,
        WhileLoopNode? whileStatement,
        ReturnNode? returnStatement,
        SourceLocation sourceLocation)
    {
        Assignment = assignment;
        FunctionCall = functionCall;
        IfStatement = ifStatement;
        ForStatement = forStatement;
        WhileStatement = whileStatement;
        ReturnStatement = returnStatement;
        SourceLocation = sourceLocation;

        if (assignment != null) Children.Add(assignment); // Add the statement as a child if it is an assignment statement.
        if (functionCall != null) Children.Add(functionCall); // Add the statement as a child if it is a function call statement.
        if (ifStatement != null) Children.Add(ifStatement); // Add the statement as a child if it is an if statement.
        if (forStatement != null) Children.Add(forStatement); // Add the statement as a child if it is a for statement.
        if (whileStatement != null) Children.Add(whileStatement); // Add the statement as a child if it is a while statement.
        if (returnStatement != null) Children.Add(returnStatement); // Add the statement as a child if it is a return statement.
    }
}