//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.12.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Users/pelle/Git/P4-Project/AST/AnimationLanguage/Content\AnimationLanguageRules.g4 by ANTLR 4.12.0

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="AnimationLanguageRulesParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.12.0")]
[System.CLSCompliant(false)]
public interface IAnimationLanguageRulesListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.s"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterS([NotNull] AnimationLanguageRulesParser.SContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.s"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitS([NotNull] AnimationLanguageRulesParser.SContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProgram([NotNull] AnimationLanguageRulesParser.ProgramContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProgram([NotNull] AnimationLanguageRulesParser.ProgramContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.setupBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSetupBlock([NotNull] AnimationLanguageRulesParser.SetupBlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.setupBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSetupBlock([NotNull] AnimationLanguageRulesParser.SetupBlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.grouping"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterGrouping([NotNull] AnimationLanguageRulesParser.GroupingContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.grouping"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitGrouping([NotNull] AnimationLanguageRulesParser.GroupingContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.groupingElements"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterGroupingElements([NotNull] AnimationLanguageRulesParser.GroupingElementsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.groupingElements"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitGroupingElements([NotNull] AnimationLanguageRulesParser.GroupingElementsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.keyValuePair"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterKeyValuePair([NotNull] AnimationLanguageRulesParser.KeyValuePairContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.keyValuePair"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitKeyValuePair([NotNull] AnimationLanguageRulesParser.KeyValuePairContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.assignments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignments([NotNull] AnimationLanguageRulesParser.AssignmentsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.assignments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignments([NotNull] AnimationLanguageRulesParser.AssignmentsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignment([NotNull] AnimationLanguageRulesParser.AssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignment([NotNull] AnimationLanguageRulesParser.AssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.unary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUnary([NotNull] AnimationLanguageRulesParser.UnaryContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.unary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUnary([NotNull] AnimationLanguageRulesParser.UnaryContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.unaryOperation"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUnaryOperation([NotNull] AnimationLanguageRulesParser.UnaryOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.unaryOperation"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUnaryOperation([NotNull] AnimationLanguageRulesParser.UnaryOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.assOps"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssOps([NotNull] AnimationLanguageRulesParser.AssOpsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.assOps"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssOps([NotNull] AnimationLanguageRulesParser.AssOpsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterType([NotNull] AnimationLanguageRulesParser.TypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitType([NotNull] AnimationLanguageRulesParser.TypeContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>binaryExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBinaryExpression([NotNull] AnimationLanguageRulesParser.BinaryExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>binaryExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBinaryExpression([NotNull] AnimationLanguageRulesParser.BinaryExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>stringExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStringExpression([NotNull] AnimationLanguageRulesParser.StringExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>stringExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStringExpression([NotNull] AnimationLanguageRulesParser.StringExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>identifierExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIdentifierExpression([NotNull] AnimationLanguageRulesParser.IdentifierExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>identifierExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIdentifierExpression([NotNull] AnimationLanguageRulesParser.IdentifierExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>functionCallExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionCallExpression([NotNull] AnimationLanguageRulesParser.FunctionCallExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>functionCallExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionCallExpression([NotNull] AnimationLanguageRulesParser.FunctionCallExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>shapeInitExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterShapeInitExpression([NotNull] AnimationLanguageRulesParser.ShapeInitExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>shapeInitExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitShapeInitExpression([NotNull] AnimationLanguageRulesParser.ShapeInitExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>booleanExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBooleanExpression([NotNull] AnimationLanguageRulesParser.BooleanExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>booleanExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBooleanExpression([NotNull] AnimationLanguageRulesParser.BooleanExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>parenthesizedExpressionExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParenthesizedExpressionExpression([NotNull] AnimationLanguageRulesParser.ParenthesizedExpressionExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>parenthesizedExpressionExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParenthesizedExpressionExpression([NotNull] AnimationLanguageRulesParser.ParenthesizedExpressionExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>integerExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIntegerExpression([NotNull] AnimationLanguageRulesParser.IntegerExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>integerExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIntegerExpression([NotNull] AnimationLanguageRulesParser.IntegerExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>floatExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFloatExpression([NotNull] AnimationLanguageRulesParser.FloatExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>floatExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFloatExpression([NotNull] AnimationLanguageRulesParser.FloatExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.parenthesizedExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParenthesizedExpression([NotNull] AnimationLanguageRulesParser.ParenthesizedExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.parenthesizedExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParenthesizedExpression([NotNull] AnimationLanguageRulesParser.ParenthesizedExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.boolean"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBoolean([NotNull] AnimationLanguageRulesParser.BooleanContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.boolean"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBoolean([NotNull] AnimationLanguageRulesParser.BooleanContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOperator([NotNull] AnimationLanguageRulesParser.OperatorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOperator([NotNull] AnimationLanguageRulesParser.OperatorContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.funcCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFuncCall([NotNull] AnimationLanguageRulesParser.FuncCallContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.funcCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFuncCall([NotNull] AnimationLanguageRulesParser.FuncCallContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.funcArgs"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFuncArgs([NotNull] AnimationLanguageRulesParser.FuncArgsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.funcArgs"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFuncArgs([NotNull] AnimationLanguageRulesParser.FuncArgsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.shapeinit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterShapeinit([NotNull] AnimationLanguageRulesParser.ShapeinitContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.shapeinit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitShapeinit([NotNull] AnimationLanguageRulesParser.ShapeinitContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.argName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArgName([NotNull] AnimationLanguageRulesParser.ArgNameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.argName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArgName([NotNull] AnimationLanguageRulesParser.ArgNameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.arg"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArg([NotNull] AnimationLanguageRulesParser.ArgContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.arg"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArg([NotNull] AnimationLanguageRulesParser.ArgContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.tuple"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTuple([NotNull] AnimationLanguageRulesParser.TupleContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.tuple"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTuple([NotNull] AnimationLanguageRulesParser.TupleContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.call_parameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCall_parameters([NotNull] AnimationLanguageRulesParser.Call_parametersContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.call_parameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCall_parameters([NotNull] AnimationLanguageRulesParser.Call_parametersContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.call_parameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCall_parameter([NotNull] AnimationLanguageRulesParser.Call_parameterContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.call_parameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCall_parameter([NotNull] AnimationLanguageRulesParser.Call_parameterContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.prototype"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrototype([NotNull] AnimationLanguageRulesParser.PrototypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.prototype"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrototype([NotNull] AnimationLanguageRulesParser.PrototypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.parameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParameters([NotNull] AnimationLanguageRulesParser.ParametersContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.parameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParameters([NotNull] AnimationLanguageRulesParser.ParametersContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.parameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParameter([NotNull] AnimationLanguageRulesParser.ParameterContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.parameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParameter([NotNull] AnimationLanguageRulesParser.ParameterContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.funcDecl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFuncDecl([NotNull] AnimationLanguageRulesParser.FuncDeclContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.funcDecl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFuncDecl([NotNull] AnimationLanguageRulesParser.FuncDeclContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlock([NotNull] AnimationLanguageRulesParser.BlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlock([NotNull] AnimationLanguageRulesParser.BlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.statements"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatements([NotNull] AnimationLanguageRulesParser.StatementsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.statements"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatements([NotNull] AnimationLanguageRulesParser.StatementsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatement([NotNull] AnimationLanguageRulesParser.StatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatement([NotNull] AnimationLanguageRulesParser.StatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.identifierGrouping"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIdentifierGrouping([NotNull] AnimationLanguageRulesParser.IdentifierGroupingContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.identifierGrouping"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIdentifierGrouping([NotNull] AnimationLanguageRulesParser.IdentifierGroupingContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.return"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterReturn([NotNull] AnimationLanguageRulesParser.ReturnContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.return"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitReturn([NotNull] AnimationLanguageRulesParser.ReturnContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.loop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLoop([NotNull] AnimationLanguageRulesParser.LoopContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.loop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLoop([NotNull] AnimationLanguageRulesParser.LoopContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.for_loop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFor_loop([NotNull] AnimationLanguageRulesParser.For_loopContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.for_loop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFor_loop([NotNull] AnimationLanguageRulesParser.For_loopContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.while_loop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterWhile_loop([NotNull] AnimationLanguageRulesParser.While_loopContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.while_loop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitWhile_loop([NotNull] AnimationLanguageRulesParser.While_loopContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.logicOpp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLogicOpp([NotNull] AnimationLanguageRulesParser.LogicOppContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.logicOpp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLogicOpp([NotNull] AnimationLanguageRulesParser.LogicOppContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.comparator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterComparator([NotNull] AnimationLanguageRulesParser.ComparatorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.comparator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitComparator([NotNull] AnimationLanguageRulesParser.ComparatorContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.conditional"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConditional([NotNull] AnimationLanguageRulesParser.ConditionalContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.conditional"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConditional([NotNull] AnimationLanguageRulesParser.ConditionalContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.elseif"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterElseif([NotNull] AnimationLanguageRulesParser.ElseifContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.elseif"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitElseif([NotNull] AnimationLanguageRulesParser.ElseifContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.else"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterElse([NotNull] AnimationLanguageRulesParser.ElseContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.else"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitElse([NotNull] AnimationLanguageRulesParser.ElseContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.sequences"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSequences([NotNull] AnimationLanguageRulesParser.SequencesContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.sequences"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSequences([NotNull] AnimationLanguageRulesParser.SequencesContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.sequence"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSequence([NotNull] AnimationLanguageRulesParser.SequenceContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.sequence"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSequence([NotNull] AnimationLanguageRulesParser.SequenceContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.sequenceCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSequenceCall([NotNull] AnimationLanguageRulesParser.SequenceCallContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.sequenceCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSequenceCall([NotNull] AnimationLanguageRulesParser.SequenceCallContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.seqBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSeqBlock([NotNull] AnimationLanguageRulesParser.SeqBlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.seqBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSeqBlock([NotNull] AnimationLanguageRulesParser.SeqBlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.seqBlockPart"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSeqBlockPart([NotNull] AnimationLanguageRulesParser.SeqBlockPartContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.seqBlockPart"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSeqBlockPart([NotNull] AnimationLanguageRulesParser.SeqBlockPartContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.animation"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAnimation([NotNull] AnimationLanguageRulesParser.AnimationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.animation"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAnimation([NotNull] AnimationLanguageRulesParser.AnimationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.animationPart"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAnimationPart([NotNull] AnimationLanguageRulesParser.AnimationPartContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.animationPart"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAnimationPart([NotNull] AnimationLanguageRulesParser.AnimationPartContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.transition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTransition([NotNull] AnimationLanguageRulesParser.TransitionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.transition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTransition([NotNull] AnimationLanguageRulesParser.TransitionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCommand([NotNull] AnimationLanguageRulesParser.CommandContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCommand([NotNull] AnimationLanguageRulesParser.CommandContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.timelineBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTimelineBlock([NotNull] AnimationLanguageRulesParser.TimelineBlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.timelineBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTimelineBlock([NotNull] AnimationLanguageRulesParser.TimelineBlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="AnimationLanguageRulesParser.frameDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFrameDef([NotNull] AnimationLanguageRulesParser.FrameDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="AnimationLanguageRulesParser.frameDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFrameDef([NotNull] AnimationLanguageRulesParser.FrameDefContext context);
}
