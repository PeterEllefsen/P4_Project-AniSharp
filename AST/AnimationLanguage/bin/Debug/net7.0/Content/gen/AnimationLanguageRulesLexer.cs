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

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.12.0")]
[System.CLSCompliant(false)]
public partial class AnimationLanguageRulesLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		SETUP=1, PROTOTYPE=2, TIMELINE=3, FOR=4, WHILE=5, IF=6, ELSE=7, SEQ=8, 
		GROUP=9, INT=10, FLOAT_TYPE=11, STRING_TYPE=12, BOOL=13, CIRCLE=14, POLYGON=15, 
		TRUE=16, FALSE=17, FRAME=18, FUNCTION=19, RETURN=20, PLUS=21, MINUS=22, 
		MULTIPLY=23, DIVIDE=24, MODULO=25, LT=26, GT=27, LE=28, GE=29, EQ=30, 
		NE=31, EQUAL=32, PLUSEQUAL=33, MINUSEQUAL=34, AND=35, OR=36, INC=37, DEC=38, 
		LPAREN=39, RPAREN=40, LBRACE=41, RBRACE=42, LBRACKET=43, RBRACKET=44, 
		COLON=45, SEMICOLON=46, COMMA=47, ARROW=48, MAP=49, QUOTE=50, DOT=51, 
		INTEGER=52, FLOAT=53, IDENTIFIER=54, STRING=55, WS=56, COMMENT=57;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"SETUP", "PROTOTYPE", "TIMELINE", "FOR", "WHILE", "IF", "ELSE", "SEQ", 
		"GROUP", "INT", "FLOAT_TYPE", "STRING_TYPE", "BOOL", "CIRCLE", "POLYGON", 
		"TRUE", "FALSE", "FRAME", "FUNCTION", "RETURN", "PLUS", "MINUS", "MULTIPLY", 
		"DIVIDE", "MODULO", "LT", "GT", "LE", "GE", "EQ", "NE", "EQUAL", "PLUSEQUAL", 
		"MINUSEQUAL", "AND", "OR", "INC", "DEC", "LPAREN", "RPAREN", "LBRACE", 
		"RBRACE", "LBRACKET", "RBRACKET", "COLON", "SEMICOLON", "COMMA", "ARROW", 
		"MAP", "QUOTE", "DOT", "DIGIT", "LETTER", "WS_CHAR", "SPECIAL_CHAR", "INTEGER", 
		"FLOAT", "IDENTIFIER", "STRING", "WS", "COMMENT"
	};


	public AnimationLanguageRulesLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public AnimationLanguageRulesLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'setup'", "'prototypes'", "'timeline'", "'for'", "'while'", "'if'", 
		"'else'", "'seq'", "'group'", "'int'", "'float'", "'string'", "'bool'", 
		"'Circle'", "'Polygon'", "'true'", "'false'", "'Frame'", "'function'", 
		"'return'", "'+'", "'-'", "'*'", "'/'", "'%'", "'<'", "'>'", "'<='", "'>='", 
		"'=='", "'!='", "'='", "'+='", "'-='", "'and'", "'or'", "'++'", "'--'", 
		"'('", "')'", "'{'", "'}'", "'['", "']'", "':'", "';'", "','", "'->'", 
		"'=>'", "'\"'", "'.'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "SETUP", "PROTOTYPE", "TIMELINE", "FOR", "WHILE", "IF", "ELSE", 
		"SEQ", "GROUP", "INT", "FLOAT_TYPE", "STRING_TYPE", "BOOL", "CIRCLE", 
		"POLYGON", "TRUE", "FALSE", "FRAME", "FUNCTION", "RETURN", "PLUS", "MINUS", 
		"MULTIPLY", "DIVIDE", "MODULO", "LT", "GT", "LE", "GE", "EQ", "NE", "EQUAL", 
		"PLUSEQUAL", "MINUSEQUAL", "AND", "OR", "INC", "DEC", "LPAREN", "RPAREN", 
		"LBRACE", "RBRACE", "LBRACKET", "RBRACKET", "COLON", "SEMICOLON", "COMMA", 
		"ARROW", "MAP", "QUOTE", "DOT", "INTEGER", "FLOAT", "IDENTIFIER", "STRING", 
		"WS", "COMMENT"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "AnimationLanguageRules.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static AnimationLanguageRulesLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,57,384,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,2,42,
		7,42,2,43,7,43,2,44,7,44,2,45,7,45,2,46,7,46,2,47,7,47,2,48,7,48,2,49,
		7,49,2,50,7,50,2,51,7,51,2,52,7,52,2,53,7,53,2,54,7,54,2,55,7,55,2,56,
		7,56,2,57,7,57,2,58,7,58,2,59,7,59,2,60,7,60,1,0,1,0,1,0,1,0,1,0,1,0,1,
		1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,2,1,2,1,2,1,2,1,2,1,2,
		1,2,1,2,1,3,1,3,1,3,1,3,1,4,1,4,1,4,1,4,1,4,1,4,1,5,1,5,1,5,1,6,1,6,1,
		6,1,6,1,6,1,7,1,7,1,7,1,7,1,8,1,8,1,8,1,8,1,8,1,8,1,9,1,9,1,9,1,9,1,10,
		1,10,1,10,1,10,1,10,1,10,1,11,1,11,1,11,1,11,1,11,1,11,1,11,1,12,1,12,
		1,12,1,12,1,12,1,13,1,13,1,13,1,13,1,13,1,13,1,13,1,14,1,14,1,14,1,14,
		1,14,1,14,1,14,1,14,1,15,1,15,1,15,1,15,1,15,1,16,1,16,1,16,1,16,1,16,
		1,16,1,17,1,17,1,17,1,17,1,17,1,17,1,18,1,18,1,18,1,18,1,18,1,18,1,18,
		1,18,1,18,1,19,1,19,1,19,1,19,1,19,1,19,1,19,1,20,1,20,1,21,1,21,1,22,
		1,22,1,23,1,23,1,24,1,24,1,25,1,25,1,26,1,26,1,27,1,27,1,27,1,28,1,28,
		1,28,1,29,1,29,1,29,1,30,1,30,1,30,1,31,1,31,1,32,1,32,1,32,1,33,1,33,
		1,33,1,34,1,34,1,34,1,34,1,35,1,35,1,35,1,36,1,36,1,36,1,37,1,37,1,37,
		1,38,1,38,1,39,1,39,1,40,1,40,1,41,1,41,1,42,1,42,1,43,1,43,1,44,1,44,
		1,45,1,45,1,46,1,46,1,47,1,47,1,47,1,48,1,48,1,48,1,49,1,49,1,50,1,50,
		1,51,1,51,1,52,1,52,1,53,1,53,1,54,1,54,1,55,4,55,332,8,55,11,55,12,55,
		333,1,56,4,56,337,8,56,11,56,12,56,338,1,56,1,56,4,56,343,8,56,11,56,12,
		56,344,1,57,1,57,1,57,5,57,350,8,57,10,57,12,57,353,9,57,1,58,1,58,1,58,
		1,58,1,58,5,58,360,8,58,10,58,12,58,363,9,58,1,58,1,58,1,59,4,59,368,8,
		59,11,59,12,59,369,1,59,1,59,1,60,1,60,1,60,1,60,5,60,378,8,60,10,60,12,
		60,381,9,60,1,60,1,60,0,0,61,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,19,
		10,21,11,23,12,25,13,27,14,29,15,31,16,33,17,35,18,37,19,39,20,41,21,43,
		22,45,23,47,24,49,25,51,26,53,27,55,28,57,29,59,30,61,31,63,32,65,33,67,
		34,69,35,71,36,73,37,75,38,77,39,79,40,81,41,83,42,85,43,87,44,89,45,91,
		46,93,47,95,48,97,49,99,50,101,51,103,0,105,0,107,0,109,0,111,52,113,53,
		115,54,117,55,119,56,121,57,1,0,5,1,0,48,57,2,0,65,90,97,122,3,0,9,10,
		13,13,32,32,6,0,9,10,13,13,32,32,42,47,58,62,92,92,2,0,10,10,13,13,390,
		0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,
		0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,
		1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,
		0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,
		1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,
		0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,0,0,67,
		1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,0,0,77,1,0,0,
		0,0,79,1,0,0,0,0,81,1,0,0,0,0,83,1,0,0,0,0,85,1,0,0,0,0,87,1,0,0,0,0,89,
		1,0,0,0,0,91,1,0,0,0,0,93,1,0,0,0,0,95,1,0,0,0,0,97,1,0,0,0,0,99,1,0,0,
		0,0,101,1,0,0,0,0,111,1,0,0,0,0,113,1,0,0,0,0,115,1,0,0,0,0,117,1,0,0,
		0,0,119,1,0,0,0,0,121,1,0,0,0,1,123,1,0,0,0,3,129,1,0,0,0,5,140,1,0,0,
		0,7,149,1,0,0,0,9,153,1,0,0,0,11,159,1,0,0,0,13,162,1,0,0,0,15,167,1,0,
		0,0,17,171,1,0,0,0,19,177,1,0,0,0,21,181,1,0,0,0,23,187,1,0,0,0,25,194,
		1,0,0,0,27,199,1,0,0,0,29,206,1,0,0,0,31,214,1,0,0,0,33,219,1,0,0,0,35,
		225,1,0,0,0,37,231,1,0,0,0,39,240,1,0,0,0,41,247,1,0,0,0,43,249,1,0,0,
		0,45,251,1,0,0,0,47,253,1,0,0,0,49,255,1,0,0,0,51,257,1,0,0,0,53,259,1,
		0,0,0,55,261,1,0,0,0,57,264,1,0,0,0,59,267,1,0,0,0,61,270,1,0,0,0,63,273,
		1,0,0,0,65,275,1,0,0,0,67,278,1,0,0,0,69,281,1,0,0,0,71,285,1,0,0,0,73,
		288,1,0,0,0,75,291,1,0,0,0,77,294,1,0,0,0,79,296,1,0,0,0,81,298,1,0,0,
		0,83,300,1,0,0,0,85,302,1,0,0,0,87,304,1,0,0,0,89,306,1,0,0,0,91,308,1,
		0,0,0,93,310,1,0,0,0,95,312,1,0,0,0,97,315,1,0,0,0,99,318,1,0,0,0,101,
		320,1,0,0,0,103,322,1,0,0,0,105,324,1,0,0,0,107,326,1,0,0,0,109,328,1,
		0,0,0,111,331,1,0,0,0,113,336,1,0,0,0,115,346,1,0,0,0,117,354,1,0,0,0,
		119,367,1,0,0,0,121,373,1,0,0,0,123,124,5,115,0,0,124,125,5,101,0,0,125,
		126,5,116,0,0,126,127,5,117,0,0,127,128,5,112,0,0,128,2,1,0,0,0,129,130,
		5,112,0,0,130,131,5,114,0,0,131,132,5,111,0,0,132,133,5,116,0,0,133,134,
		5,111,0,0,134,135,5,116,0,0,135,136,5,121,0,0,136,137,5,112,0,0,137,138,
		5,101,0,0,138,139,5,115,0,0,139,4,1,0,0,0,140,141,5,116,0,0,141,142,5,
		105,0,0,142,143,5,109,0,0,143,144,5,101,0,0,144,145,5,108,0,0,145,146,
		5,105,0,0,146,147,5,110,0,0,147,148,5,101,0,0,148,6,1,0,0,0,149,150,5,
		102,0,0,150,151,5,111,0,0,151,152,5,114,0,0,152,8,1,0,0,0,153,154,5,119,
		0,0,154,155,5,104,0,0,155,156,5,105,0,0,156,157,5,108,0,0,157,158,5,101,
		0,0,158,10,1,0,0,0,159,160,5,105,0,0,160,161,5,102,0,0,161,12,1,0,0,0,
		162,163,5,101,0,0,163,164,5,108,0,0,164,165,5,115,0,0,165,166,5,101,0,
		0,166,14,1,0,0,0,167,168,5,115,0,0,168,169,5,101,0,0,169,170,5,113,0,0,
		170,16,1,0,0,0,171,172,5,103,0,0,172,173,5,114,0,0,173,174,5,111,0,0,174,
		175,5,117,0,0,175,176,5,112,0,0,176,18,1,0,0,0,177,178,5,105,0,0,178,179,
		5,110,0,0,179,180,5,116,0,0,180,20,1,0,0,0,181,182,5,102,0,0,182,183,5,
		108,0,0,183,184,5,111,0,0,184,185,5,97,0,0,185,186,5,116,0,0,186,22,1,
		0,0,0,187,188,5,115,0,0,188,189,5,116,0,0,189,190,5,114,0,0,190,191,5,
		105,0,0,191,192,5,110,0,0,192,193,5,103,0,0,193,24,1,0,0,0,194,195,5,98,
		0,0,195,196,5,111,0,0,196,197,5,111,0,0,197,198,5,108,0,0,198,26,1,0,0,
		0,199,200,5,67,0,0,200,201,5,105,0,0,201,202,5,114,0,0,202,203,5,99,0,
		0,203,204,5,108,0,0,204,205,5,101,0,0,205,28,1,0,0,0,206,207,5,80,0,0,
		207,208,5,111,0,0,208,209,5,108,0,0,209,210,5,121,0,0,210,211,5,103,0,
		0,211,212,5,111,0,0,212,213,5,110,0,0,213,30,1,0,0,0,214,215,5,116,0,0,
		215,216,5,114,0,0,216,217,5,117,0,0,217,218,5,101,0,0,218,32,1,0,0,0,219,
		220,5,102,0,0,220,221,5,97,0,0,221,222,5,108,0,0,222,223,5,115,0,0,223,
		224,5,101,0,0,224,34,1,0,0,0,225,226,5,70,0,0,226,227,5,114,0,0,227,228,
		5,97,0,0,228,229,5,109,0,0,229,230,5,101,0,0,230,36,1,0,0,0,231,232,5,
		102,0,0,232,233,5,117,0,0,233,234,5,110,0,0,234,235,5,99,0,0,235,236,5,
		116,0,0,236,237,5,105,0,0,237,238,5,111,0,0,238,239,5,110,0,0,239,38,1,
		0,0,0,240,241,5,114,0,0,241,242,5,101,0,0,242,243,5,116,0,0,243,244,5,
		117,0,0,244,245,5,114,0,0,245,246,5,110,0,0,246,40,1,0,0,0,247,248,5,43,
		0,0,248,42,1,0,0,0,249,250,5,45,0,0,250,44,1,0,0,0,251,252,5,42,0,0,252,
		46,1,0,0,0,253,254,5,47,0,0,254,48,1,0,0,0,255,256,5,37,0,0,256,50,1,0,
		0,0,257,258,5,60,0,0,258,52,1,0,0,0,259,260,5,62,0,0,260,54,1,0,0,0,261,
		262,5,60,0,0,262,263,5,61,0,0,263,56,1,0,0,0,264,265,5,62,0,0,265,266,
		5,61,0,0,266,58,1,0,0,0,267,268,5,61,0,0,268,269,5,61,0,0,269,60,1,0,0,
		0,270,271,5,33,0,0,271,272,5,61,0,0,272,62,1,0,0,0,273,274,5,61,0,0,274,
		64,1,0,0,0,275,276,5,43,0,0,276,277,5,61,0,0,277,66,1,0,0,0,278,279,5,
		45,0,0,279,280,5,61,0,0,280,68,1,0,0,0,281,282,5,97,0,0,282,283,5,110,
		0,0,283,284,5,100,0,0,284,70,1,0,0,0,285,286,5,111,0,0,286,287,5,114,0,
		0,287,72,1,0,0,0,288,289,5,43,0,0,289,290,5,43,0,0,290,74,1,0,0,0,291,
		292,5,45,0,0,292,293,5,45,0,0,293,76,1,0,0,0,294,295,5,40,0,0,295,78,1,
		0,0,0,296,297,5,41,0,0,297,80,1,0,0,0,298,299,5,123,0,0,299,82,1,0,0,0,
		300,301,5,125,0,0,301,84,1,0,0,0,302,303,5,91,0,0,303,86,1,0,0,0,304,305,
		5,93,0,0,305,88,1,0,0,0,306,307,5,58,0,0,307,90,1,0,0,0,308,309,5,59,0,
		0,309,92,1,0,0,0,310,311,5,44,0,0,311,94,1,0,0,0,312,313,5,45,0,0,313,
		314,5,62,0,0,314,96,1,0,0,0,315,316,5,61,0,0,316,317,5,62,0,0,317,98,1,
		0,0,0,318,319,5,34,0,0,319,100,1,0,0,0,320,321,5,46,0,0,321,102,1,0,0,
		0,322,323,7,0,0,0,323,104,1,0,0,0,324,325,7,1,0,0,325,106,1,0,0,0,326,
		327,7,2,0,0,327,108,1,0,0,0,328,329,8,3,0,0,329,110,1,0,0,0,330,332,3,
		103,51,0,331,330,1,0,0,0,332,333,1,0,0,0,333,331,1,0,0,0,333,334,1,0,0,
		0,334,112,1,0,0,0,335,337,3,103,51,0,336,335,1,0,0,0,337,338,1,0,0,0,338,
		336,1,0,0,0,338,339,1,0,0,0,339,340,1,0,0,0,340,342,5,46,0,0,341,343,3,
		103,51,0,342,341,1,0,0,0,343,344,1,0,0,0,344,342,1,0,0,0,344,345,1,0,0,
		0,345,114,1,0,0,0,346,351,3,105,52,0,347,350,3,105,52,0,348,350,3,103,
		51,0,349,347,1,0,0,0,349,348,1,0,0,0,350,353,1,0,0,0,351,349,1,0,0,0,351,
		352,1,0,0,0,352,116,1,0,0,0,353,351,1,0,0,0,354,361,3,99,49,0,355,360,
		3,105,52,0,356,360,3,103,51,0,357,360,3,109,54,0,358,360,3,107,53,0,359,
		355,1,0,0,0,359,356,1,0,0,0,359,357,1,0,0,0,359,358,1,0,0,0,360,363,1,
		0,0,0,361,359,1,0,0,0,361,362,1,0,0,0,362,364,1,0,0,0,363,361,1,0,0,0,
		364,365,3,99,49,0,365,118,1,0,0,0,366,368,7,2,0,0,367,366,1,0,0,0,368,
		369,1,0,0,0,369,367,1,0,0,0,369,370,1,0,0,0,370,371,1,0,0,0,371,372,6,
		59,0,0,372,120,1,0,0,0,373,374,5,47,0,0,374,375,5,47,0,0,375,379,1,0,0,
		0,376,378,8,4,0,0,377,376,1,0,0,0,378,381,1,0,0,0,379,377,1,0,0,0,379,
		380,1,0,0,0,380,382,1,0,0,0,381,379,1,0,0,0,382,383,6,60,0,0,383,122,1,
		0,0,0,10,0,333,338,344,349,351,359,361,369,379,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
