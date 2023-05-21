﻿namespace AnimationLanguage.ASTCommon;

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
    Parameter,
    Grouping,
    GroupingElements,
    Setup,
    KeyValuePair,
    Type,
    ShapeInit,
    Argument,
    Block,
    Statement,
    ElseIf,
    Else,
    ForLoop,
    WhileLoop,
    Return,
    Sequence,
    TimelineBlock,
    Animation,
    Command,
    Transition,
    Tuple,
    List,
    IdentifierGrouping,
    Condition,
    SeqBlock,
    SeqBlockPart,
    FrameDef,
    SequenceCall,
    CallParameter,
}