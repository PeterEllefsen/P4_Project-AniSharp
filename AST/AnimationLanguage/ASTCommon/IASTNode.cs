namespace AnimationLanguage.ASTCommon;

//General shared rules of all nodes in the AST.
public interface IASTNode
{
    SourceLocation SourceLocation { get; set; } //The location of the node in the source code
    NodeType NodeType { get; } //The type of node e.g. Assignment, Expression, FunctionCall, FunctionDeclaration, Identifier, IfStatement
    
    IEnumerable<IASTNode> GetChildren(); //Method used to return the children of the nodes, to print the AST.
    
    T? Accept<T>(ASTVisitor<T> visitor) where T : IASTNode; //Method used to accept a visitor, to traverse the AST.
}