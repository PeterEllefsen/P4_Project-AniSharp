grammar AnimationLanguageRules;

// Tokens
SETUP: 'setup';
PROTOTYPE: 'prototypes';
TIMELINE: 'timeline';
FOR: 'for';
WHILE: 'while';
IF: 'if';
ELSE: 'else';
SEQ: 'seq';
GROUP: 'group';
INT: 'int';
FLOAT_TYPE: 'float';
STRING_TYPE: 'string';
BOOL: 'bool';
CIRCLE: 'Circle';
POLYGON: 'Polygon';
TRUE: 'true';
FALSE: 'false';
FRAME: 'Frame';
FUNCTION: 'function';
RETURN: 'return';

// Operators
PLUS: '+';
MINUS: '-';
MULTIPLY: '*';
DIVIDE: '/';
MODULO: '%';
LT: '<';
GT: '>';
LE: '<=';
GE: '>=';
EQ: '==';
NE: '!=';

//Ass ops
EQUAL: '=';
PLUSEQUAL: '+=';
MINUSEQUAL: '-=';

// Logical Opps
AND: 'and';
OR: 'or';

// Unary ops
INC: '++';
DEC: '--';

// Delimiters
LPAREN: '(';
RPAREN: ')';
LBRACE: '{';
RBRACE: '}';
LBRACKET: '[';
RBRACKET: ']';
COLON: ':';
SEMICOLON: ';';
COMMA: ',';

ARROW: '->';
MAP: '=>';
QUOTE: '"';
DOT: '.';

// Literals
fragment DIGIT: [0-9];
fragment LETTER: [a-zA-Z];
fragment WS_CHAR: [\t\n\r ];
fragment SPECIAL_CHAR: ~[\t\n\r ,.:=;<>*/\\+-];
//\[\]\{\}\(\)

INTEGER: DIGIT+;
FLOAT: DIGIT+ '.' DIGIT+;
IDENTIFIER: LETTER (LETTER | DIGIT)*;
STRING: QUOTE (LETTER | DIGIT | SPECIAL_CHAR | WS_CHAR)* QUOTE;

// Whitespace and comments
WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;

//-------------------------------------------------------------------------------------------------------------------//
// Rules
s: program EOF;

program: (PROTOTYPE LBRACE prototypes RBRACE)? (SETUP setupBlock)? funcDecl* sequences? TIMELINE timelineBlock?;


setupBlock: grouping SEMICOLON;

grouping: LBRACKET groupingElements RBRACKET;

groupingElements: (keyValuePair | expression | IDENTIFIER) (COMMA (keyValuePair | expression | IDENTIFIER))*;

keyValuePair: IDENTIFIER EQUAL expression;

assignments: assignment (SEMICOLON assignments)?;

assignment: type? IDENTIFIER assOps (expression | IDENTIFIER) SEMICOLON?
            | unaryOperation SEMICOLON
            ;
            
unary: DEC | INC;

unaryOperation: IDENTIFIER unary
              | unary IDENTIFIER
              ;

assOps: (EQUAL | PLUSEQUAL | MINUSEQUAL);

term: LPAREN (expression | IDENTIFIER | term)  RPAREN;

type: INT | FLOAT_TYPE | STRING_TYPE | BOOL | CIRCLE | POLYGON | GROUP;

expression: MINUS? INTEGER                          #integerExpression
          | MINUS? FLOAT                            #floatExpression
          | STRING                                  #stringExpression
          | boolean                                 #booleanExpression
          | expression operator expression          #binaryExpression
          | IDENTIFIER                              #identifierExpression
          | funcCall                                #functionCallExpression
          | shapeinit                               #shapeInitExpression
          | term                                    #termExpression
          ;


boolean: TRUE | FALSE;

operator: PLUS | MINUS | MULTIPLY | DIVIDE | MODULO;

funcCall: IDENTIFIER LPAREN funcArgs? RPAREN;

funcArgs: expression (COMMA expression)*;

shapeinit: (POLYGON | CIRCLE) LPAREN argName arg (COMMA argName arg)* RPAREN; 

argName: IDENTIFIER COLON;

arg: (argName)? (tuple | expression | IDENTIFIER);

tuple: LPAREN argName arg COMMA argName arg (COMMA argName arg)* RPAREN;

call_parameters: call_parameters COMMA call_parameter 
               | call_parameter;
               
call_parameter: (argName arg | arg);

prototypes: prototype (COMMA prototypes)?;

prototype: (type | GROUP) FUNCTION IDENTIFIER LPAREN parameters? RPAREN SEMICOLON;

parameters: parameter (COMMA parameter)*;

parameter: type IDENTIFIER
         | funcCall
         ;

funcDecl: (type | GROUP) FUNCTION IDENTIFIER LPAREN (parameters)? RPAREN block;

block: LBRACE statements return? RBRACE;

statements: statement*
          ;

statement: assignment
          | identifierGrouping
          | loop
          | conditional
          ;
          
identifierGrouping: IDENTIFIER LBRACKET groupingElements RBRACKET SEMICOLON;

return: RETURN expression SEMICOLON
        | RETURN grouping SEMICOLON
        ;

loop: for_loop | while_loop;

for_loop: FOR LPAREN assignment SEMICOLON condition SEMICOLON (assignment | unaryOperation) RPAREN block;

while_loop: WHILE LPAREN condition RPAREN block;

condition: expression (comparator | logicOpp) expression ((comparator | logicOpp) expression)*  ;

logicOpp: AND | OR;

comparator: LT | GT | LE | GE | EQ | NE;

conditional: IF LPAREN condition RPAREN block (elseif)* (else)?;

elseif: ELSE IF LPAREN condition RPAREN block;

else: ELSE block;

sequences: sequence sequences?;

sequence: SEQ IDENTIFIER LPAREN parameters? RPAREN seqBlock;

sequenceCall: IDENTIFIER LPAREN call_parameters? RPAREN;

seqBlock: LBRACE seqBlockPart* RBRACE;

seqBlockPart: statement
             | animation
             ;

animation: IDENTIFIER animationPart+ SEMICOLON; 

animationPart: command
             | transition
             ;
           
transition: ARROW LPAREN call_parameter (COMMA call_parameter)* RPAREN;

command: ARROW IDENTIFIER LPAREN call_parameters? RPAREN;

timelineBlock: LBRACE frameDef* RBRACE;

frameDef: FRAME INTEGER COLON sequenceCall SEMICOLON;