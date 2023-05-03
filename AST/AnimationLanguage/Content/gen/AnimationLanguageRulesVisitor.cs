//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.12.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Users/pelle/OneDrive/Skrivebord/Kode/GitHub/P4-Project/P4-Project/AST/AnimationLanguage/Content\AnimationLanguageRules.g4 by ANTLR 4.12.0

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="AnimationLanguageRulesParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.12.0")]
[System.CLSCompliant(false)]
public interface IAnimationLanguageRulesVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.s"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitS([NotNull] AnimationLanguageRulesParser.SContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProgram([NotNull] AnimationLanguageRulesParser.ProgramContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.setupBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSetupBlock([NotNull] AnimationLanguageRulesParser.SetupBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.grouping"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGrouping([NotNull] AnimationLanguageRulesParser.GroupingContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.groupingElements"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGroupingElements([NotNull] AnimationLanguageRulesParser.GroupingElementsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.keyValuePair"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitKeyValuePair([NotNull] AnimationLanguageRulesParser.KeyValuePairContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.assignments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignments([NotNull] AnimationLanguageRulesParser.AssignmentsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignment([NotNull] AnimationLanguageRulesParser.AssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.unary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnary([NotNull] AnimationLanguageRulesParser.UnaryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.assOps"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssOps([NotNull] AnimationLanguageRulesParser.AssOpsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTerm([NotNull] AnimationLanguageRulesParser.TermContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitType([NotNull] AnimationLanguageRulesParser.TypeContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>binaryExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinaryExpression([NotNull] AnimationLanguageRulesParser.BinaryExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>stringExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStringExpression([NotNull] AnimationLanguageRulesParser.StringExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>identifierExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIdentifierExpression([NotNull] AnimationLanguageRulesParser.IdentifierExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>functionCallExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionCallExpression([NotNull] AnimationLanguageRulesParser.FunctionCallExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>shapeInitExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitShapeInitExpression([NotNull] AnimationLanguageRulesParser.ShapeInitExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>booleanExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBooleanExpression([NotNull] AnimationLanguageRulesParser.BooleanExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>integerExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIntegerExpression([NotNull] AnimationLanguageRulesParser.IntegerExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>termExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTermExpression([NotNull] AnimationLanguageRulesParser.TermExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>floatExpression</c>
	/// labeled alternative in <see cref="AnimationLanguageRulesParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFloatExpression([NotNull] AnimationLanguageRulesParser.FloatExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.boolean"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBoolean([NotNull] AnimationLanguageRulesParser.BooleanContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperator([NotNull] AnimationLanguageRulesParser.OperatorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.funcCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFuncCall([NotNull] AnimationLanguageRulesParser.FuncCallContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.funcArgs"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFuncArgs([NotNull] AnimationLanguageRulesParser.FuncArgsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.shapeinit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitShapeinit([NotNull] AnimationLanguageRulesParser.ShapeinitContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.argName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArgName([NotNull] AnimationLanguageRulesParser.ArgNameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.arg"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArg([NotNull] AnimationLanguageRulesParser.ArgContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.tuple"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTuple([NotNull] AnimationLanguageRulesParser.TupleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.call_parameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCall_parameters([NotNull] AnimationLanguageRulesParser.Call_parametersContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.call_parameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCall_parameter([NotNull] AnimationLanguageRulesParser.Call_parameterContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.prototypes"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrototypes([NotNull] AnimationLanguageRulesParser.PrototypesContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.prototype"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrototype([NotNull] AnimationLanguageRulesParser.PrototypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.parameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParameters([NotNull] AnimationLanguageRulesParser.ParametersContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.parameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParameter([NotNull] AnimationLanguageRulesParser.ParameterContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.funcDecl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFuncDecl([NotNull] AnimationLanguageRulesParser.FuncDeclContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlock([NotNull] AnimationLanguageRulesParser.BlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.statements"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatements([NotNull] AnimationLanguageRulesParser.StatementsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement([NotNull] AnimationLanguageRulesParser.StatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.identifierGrouping"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIdentifierGrouping([NotNull] AnimationLanguageRulesParser.IdentifierGroupingContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.return"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitReturn([NotNull] AnimationLanguageRulesParser.ReturnContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.loop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLoop([NotNull] AnimationLanguageRulesParser.LoopContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.for_loop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFor_loop([NotNull] AnimationLanguageRulesParser.For_loopContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.while_loop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhile_loop([NotNull] AnimationLanguageRulesParser.While_loopContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.condition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCondition([NotNull] AnimationLanguageRulesParser.ConditionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.logicOpp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLogicOpp([NotNull] AnimationLanguageRulesParser.LogicOppContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.comparator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparator([NotNull] AnimationLanguageRulesParser.ComparatorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.conditional"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditional([NotNull] AnimationLanguageRulesParser.ConditionalContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.elseif"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElseif([NotNull] AnimationLanguageRulesParser.ElseifContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.else"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElse([NotNull] AnimationLanguageRulesParser.ElseContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.sequences"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSequences([NotNull] AnimationLanguageRulesParser.SequencesContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.sequence"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSequence([NotNull] AnimationLanguageRulesParser.SequenceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.sequenceCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSequenceCall([NotNull] AnimationLanguageRulesParser.SequenceCallContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.seqBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSeqBlock([NotNull] AnimationLanguageRulesParser.SeqBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.seqBlockPart"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSeqBlockPart([NotNull] AnimationLanguageRulesParser.SeqBlockPartContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.animation"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAnimation([NotNull] AnimationLanguageRulesParser.AnimationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.animationPart"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAnimationPart([NotNull] AnimationLanguageRulesParser.AnimationPartContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.transition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTransition([NotNull] AnimationLanguageRulesParser.TransitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCommand([NotNull] AnimationLanguageRulesParser.CommandContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.timelineBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTimelineBlock([NotNull] AnimationLanguageRulesParser.TimelineBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="AnimationLanguageRulesParser.frameDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFrameDef([NotNull] AnimationLanguageRulesParser.FrameDefContext context);
}
