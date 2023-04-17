namespace AnimationLanguage.ASTCommon;

public enum NodeType //This enum is used to determine what type of node is being dealt with.
{
    Assignment,
    Expression,
    FunctionCall,
    FunctionDeclaration,
    Identifier,
    IfStatement,
    Operator,
    IntegerLiteral,
    FloatLiteral,
    StringLiteral,
    BooleanLiteral,
    Circle,
    Polygon,
    Program,
    Prototype,
    Parameter
}