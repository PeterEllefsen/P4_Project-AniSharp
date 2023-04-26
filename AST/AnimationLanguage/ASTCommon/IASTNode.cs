namespace AnimationLanguage.ASTCommon;

//General shared rules of all nodes in the AST.
public interface IASTNode
{
    SourceLocation SourceLocation { get; set; } //The location of the node in the source code
    NodeType NodeType { get; } //The type of node e.g. Assignment, Expression, FunctionCall, FunctionDeclaration, Identifier, IfStatement
    
    /*The yield return statement will be used in the implementations of the GetChildren method.
     The 'yield return' statement returns individual elements of a collection one by one,
    instead of building the entire collection and returning it.*/

    IEnumerable<IASTNode> GetChildren(); //Method used to return the children of the nodes, to print the AST.
}