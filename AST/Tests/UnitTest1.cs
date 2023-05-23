using AnimationLanguage.ASTNodes;

using AnimationLanguage.ASTCommon;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using System.Xml;
using AnimationLanguage.Visitor;
using Antlr4.Runtime;

namespace Tests
{
    public class ASTNodes
    {
        [Fact]
        public void IdentifierNode_CreateNode_CheckProperties()
        {
            // Arrange
            string identifierName = "myIdentifier";
            SourceLocation sourceLocation = new SourceLocation(1, 1);

            // Act
            IdentifierNode identifierNode = new IdentifierNode(identifierName, sourceLocation);

            // Assert
            Assert.Equal(ExpressionNodeType.Identifier, identifierNode.ExpressionType);
            Assert.Equal(identifierName, identifierNode.Name);
            Assert.Equal(sourceLocation, identifierNode.SourceLocation);
        }

        [Fact]
        public void IdentifierNode_GetChildren_CheckEmptyList()
        {
            // Arrange
            string identifierName = "myIdentifier";
            SourceLocation sourceLocation = new SourceLocation(1, 1);
            IdentifierNode identifierNode = new IdentifierNode(identifierName, sourceLocation);

            // Act
            var children = identifierNode.GetChildren();

            // Assert
            Assert.False(children.Any());
        }

        [Fact]
        public void IdentifierNode_ToString_CheckFormat()
        {
            // Arrange
            string identifierName = "myIdentifier";
            SourceLocation sourceLocation = new SourceLocation(1, 1);
            IdentifierNode identifierNode = new IdentifierNode(identifierName, sourceLocation);

            // Act
            string identifierNodeString = identifierNode.ToString();

            // Assert
            Assert.Equal($"myIdentifier", identifierNodeString);
        }
        
        [Fact]
        public void SequenceNode_CreateNode_CheckProperties()
        {
            // Arrange
            IdentifierNode sequenceName = new IdentifierNode("mySequence", new SourceLocation(1, 1));
            ParameterNode parameter1 = new ParameterNode(DataType.Int, "param1", new SourceLocation(1, 2));
            ParameterNode parameter2 = new ParameterNode(DataType.Float, "param2", new SourceLocation(1, 3));
            List<ParameterNode> parameters = new List<ParameterNode> { parameter1, parameter2 };

            // Create a sample statement
            AssignmentNode assignment = new AssignmentNode(
                new IdentifierNode("x", new SourceLocation(1, 4)),
                AssignmentOperator.Assign,
                new IntegerLiteralNode(5, new SourceLocation(1, 5)),
                VariableType.Int,
                new SourceLocation(1, 4));
            StatementNode statement = new StatementNode(assignment, null, null, null, null, null, new SourceLocation(1, 4));
            List<StatementNode> statements = new List<StatementNode> { statement };

            // Create a sample animation
            IdentifierNode animationIdentifier = new IdentifierNode("myAnimation", new SourceLocation(1, 6));
            CommandNode command = new CommandNode(animationIdentifier, null, new SourceLocation(1, 7));

            // Create a sample transition
            IdentifierNode transitionIdentifier = new IdentifierNode("property", new SourceLocation(1, 11));
            IntegerLiteralNode value1 = new IntegerLiteralNode(100, new SourceLocation(1, 12));
            FloatLiteralNode value2 = new FloatLiteralNode(0.5f, new SourceLocation(1, 13));
            List<IASTNode> transitionParameters = new List<IASTNode> { transitionIdentifier, value1, value2 };
            TransitionNode transition = new TransitionNode(transitionParameters, new SourceLocation(1, 14));

            List<TransitionNode> transitions = new List<TransitionNode> { transition }; // Add the sample transition to the list
            AnimationNode animation = new AnimationNode(animationIdentifier, command, transitions, new SourceLocation(1, 8));
            List<AnimationNode> animations = new List<AnimationNode> { animation };

            SeqBlockNode seqBlock = new SeqBlockNode(statements, animations, new SourceLocation(1, 9));
            SourceLocation sourceLocation = new SourceLocation(1, 10);

            // Act
            SequenceNode sequenceNode = new SequenceNode(sequenceName, parameters, seqBlock, sourceLocation);

            // Assert
            Assert.Equal(NodeType.Sequence, sequenceNode.NodeType);
            Assert.Equal(sequenceName, sequenceNode.Name);
            Assert.Equal(parameters, sequenceNode.Parameters);
            Assert.Equal(seqBlock, sequenceNode.Block);
            Assert.Equal(sourceLocation, sequenceNode.SourceLocation);
        }
    }


    public class AnimationLanguageVisitorTest
    {
        [Theory]
        [InlineData("2", "2")]
        public void Integer_Addition(string input, string expected)
        {
            // Arrange
            ICharStream charStream = CharStreams.fromString(input);
            AnimationLanguageRulesLexer lexer = new AnimationLanguageRulesLexer(charStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            AnimationLanguageRulesParser parser = new AnimationLanguageRulesParser(tokens);
            ParserRuleContext parseTreeRoot = parser.expression();

            AnimationLanguageVisitor visitor = new AnimationLanguageVisitor();

            // Act
            IASTNode? astRoot = visitor.Visit(parseTreeRoot);

            // Assert
            Assert.Equal(expected, astRoot.ToString());
        }
        [Theory]
        [InlineData("int x = 2", "AssignmentNode: x Assign 2 (IsDeclaration? True) (VariableType: Int)")]
        [InlineData("int i = 2", "AssignmentNode: i Assign 2 (IsDeclaration? True) (VariableType: Int)")]
        [InlineData("i = 2", "AssignmentNode: i Assign 2 (IsDeclaration? False) (VariableType: Int)")]
        [InlineData("float t = 2.1", "AssignmentNode: t Assign 21 (IsDeclaration? True) (VariableType: Float)")]
        [InlineData("bool b = true", "AssignmentNode: b Assign true (IsDeclaration? True) (VariableType: Bool)")]
        [InlineData("string s = \"Hello World\"", "AssignmentNode: s Assign \"Hello World\" (IsDeclaration? True) (VariableType: String)")]
        public void Int_Assignment_test(string input, string expected)
        {
            // Arrange
            ICharStream charStream = CharStreams.fromString(input);
            AnimationLanguageRulesLexer lexer = new AnimationLanguageRulesLexer(charStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            AnimationLanguageRulesParser parser = new AnimationLanguageRulesParser(tokens);
            ParserRuleContext parseTreeRoot = parser.assignment();

            AnimationLanguageVisitor visitor = new AnimationLanguageVisitor();

            // Act
            IASTNode? astRoot = visitor.Visit(parseTreeRoot);

            // Assert
            Assert.Equal(expected, astRoot.ToString());
        }
        //[Theory]
        
        public void Sequence_test(string input, string expected)
        {
            
        }
    }

    public class TypeCheckingVisitorTests
    {
        [Theory]
        [InlineData("int x = \"Hello World\"")]
        public void Assignment_TypeMismatch_Int_And_String(string input)
        {
            ICharStream charStream = CharStreams.fromString(input);
            AnimationLanguageRulesLexer lexer = new AnimationLanguageRulesLexer(charStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            AnimationLanguageRulesParser parser = new AnimationLanguageRulesParser(tokens);
            ParserRuleContext parseTreeRoot = parser.assignment();
            AnimationLanguageVisitor visitor = new AnimationLanguageVisitor();
            IASTNode? astRoot = visitor.Visit(parseTreeRoot);
            
            //Instantiate the ScopedSymbolTable.
            ScopedSymbolTable scopedSymbolTable = new ScopedSymbolTable();
            //Instantiate the TypeCheckingVisitor to perform type checking on the AST.
            TypeCheckingVisitor typeCheckingVisitor = new TypeCheckingVisitor(scopedSymbolTable);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => typeCheckingVisitor.Visit((AssignmentNode)astRoot));
            
            Assert.Equal($"Type mismatch in assignment. Cannot assign a value of type 'String' to variable of type 'Int' at Line: 1, Column: 0.", exception.Message);
            
        }
        [Theory]
        [InlineData("bool x = \"Hello World\"")]
        public void Assignment_TypeMismatch_Bool_And_String(string input)
        {
            ICharStream charStream = CharStreams.fromString(input);
            AnimationLanguageRulesLexer lexer = new AnimationLanguageRulesLexer(charStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            AnimationLanguageRulesParser parser = new AnimationLanguageRulesParser(tokens);
            ParserRuleContext parseTreeRoot = parser.assignment();
            AnimationLanguageVisitor visitor = new AnimationLanguageVisitor();
            IASTNode? astRoot = visitor.Visit(parseTreeRoot);
            
            //Instantiate the ScopedSymbolTable.
            ScopedSymbolTable scopedSymbolTable = new ScopedSymbolTable();
            //Instantiate the TypeCheckingVisitor to perform type checking on the AST.
            TypeCheckingVisitor typeCheckingVisitor = new TypeCheckingVisitor(scopedSymbolTable);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => typeCheckingVisitor.Visit((AssignmentNode)astRoot));
            
            Assert.Equal($"Type mismatch in assignment. Cannot assign a value of type 'String' to variable of type 'Bool' at Line: 1, Column: 0.", exception.Message);
            
        }
        
        [Theory]
        [InlineData("int x = 2 and 3;")]
        [InlineData("int x = 2 or 3;")]
        public void Assignment_VaraibleTypeMismatch_Int_And_String(string input)
        {
            ICharStream charStream = CharStreams.fromString(input);
            AnimationLanguageRulesLexer lexer = new AnimationLanguageRulesLexer(charStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            AnimationLanguageRulesParser parser = new AnimationLanguageRulesParser(tokens);
            ParserRuleContext parseTreeRoot = parser.assignment();
            AnimationLanguageVisitor visitor = new AnimationLanguageVisitor();
            IASTNode? astRoot = visitor.Visit(parseTreeRoot);
            
            //Instantiate the ScopedSymbolTable.
            ScopedSymbolTable scopedSymbolTable = new ScopedSymbolTable();
            //Instantiate the TypeCheckingVisitor to perform type checking on the AST.
            TypeCheckingVisitor typeCheckingVisitor = new TypeCheckingVisitor(scopedSymbolTable);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => typeCheckingVisitor.Visit((AssignmentNode)astRoot));
            
            Assert.Equal($"2 and 3 must be of type boolean.", exception.Message);
            
        }
    }

    public class CodeGenerationTests
    {
        [Theory]
        [InlineData("sceneWidth = 2000", "setup.sceneWidth =2000;")]
        [InlineData("framerate = 60", "setup.framerate =60;")]
        [InlineData("sceneHeight = 1000", "setup.sceneHeight =1000;")]
        public void KeyValuePair_Test(string input, string expected)
        {
            Helper helper = new Helper();
            
            ICharStream charStream = CharStreams.fromString(input);
            AnimationLanguageRulesLexer lexer = new AnimationLanguageRulesLexer(charStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            AnimationLanguageRulesParser parser = new AnimationLanguageRulesParser(tokens);
            ParserRuleContext parseTreeRoot = parser.keyValuePair();
            AnimationLanguageVisitor visitor = new AnimationLanguageVisitor();
            IASTNode? astRoot = visitor.Visit(parseTreeRoot);

            //Instantiate the ScopedSymbolTable.
            ScopedSymbolTable scopedSymbolTable = new ScopedSymbolTable();
            //Instantiate the TypeCheckingVisitor to perform type checking on the AST.
            TypeCheckingVisitor typeCheckingVisitor = new TypeCheckingVisitor(scopedSymbolTable);
            IASTNode? decoratedAstRoot = typeCheckingVisitor.Visit((KeyValuePairNode)astRoot);
            
            String output = helper.Visit((KeyValuePairNode) astRoot);
            
            
            Assert.Equal(expected, output);
        }
        
        [Theory]
        [InlineData("int x = 2", "int x = 2;")]
        [InlineData("string y = \"Hello world\"", "string y = \"Hello world\";")]
        [InlineData("bool b = true", "bool b = true;")]
        [InlineData("x = 2", "x = 2;")]
        public void Assignment_Test(string input, string expected)
        {
            Helper helper = new Helper();
            
            ICharStream charStream = CharStreams.fromString(input);
            AnimationLanguageRulesLexer lexer = new AnimationLanguageRulesLexer(charStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            AnimationLanguageRulesParser parser = new AnimationLanguageRulesParser(tokens);
            ParserRuleContext parseTreeRoot = parser.assignment();
            AnimationLanguageVisitor visitor = new AnimationLanguageVisitor();
            IASTNode? astRoot = visitor.Visit(parseTreeRoot);

            //Instantiate the ScopedSymbolTable.
            ScopedSymbolTable scopedSymbolTable = new ScopedSymbolTable();
            //Instantiate the TypeCheckingVisitor to perform type checking on the AST.
            TypeCheckingVisitor typeCheckingVisitor = new TypeCheckingVisitor(scopedSymbolTable);
            IASTNode? decoratedAstRoot = typeCheckingVisitor.Visit((AssignmentNode)astRoot);
            
            String output = helper.Visit((AssignmentNode) astRoot);
            
            
            Assert.Equal(expected, output);
        }
        
    }

    public class IntegrationTest
    {
        public void Assignment_Integration_Test()
        {
            
        }
    }
}
