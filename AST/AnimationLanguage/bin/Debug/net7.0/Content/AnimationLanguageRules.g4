grammar AnimationLanguageRules;

// Tokens
SETUP: 'setup';
PROTOTYPE: 'prototypes';
TIMELINE: 'timeline';
FOR: 'for';
WHILE: 'while';
IF: 'if';
SEQ: 'seq';
VOID: 'void';
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


// Rules
s: program EOF;

program: (SETUP setupBlock)? (PROTOTYPE LBRACE prototypes RBRACE funcDecls)? sequences? TIMELINE timelineBlock?;

setupBlock: grouping SEMICOLON;

grouping: LBRACKET groupingElements RBRACKET;

groupingElements: (keyValuePair | expression | IDENTIFIER) (COMMA groupingElements)?;


keyValuePair: IDENTIFIER EQUAL expression;

assignments: assignment (SEMICOLON assignments)?;

assignment: type? IDENTIFIER assOps (expression | IDENTIFIER) SEMICOLON?
            | IDENTIFIER unary SEMICOLON?
            | unary IDENTIFIER SEMICOLON?
            | IDENTIFIER grouping SEMICOLON
            ;
unary: (DEC | INC);

assOps: (EQUAL | PLUSEQUAL | MINUSEQUAL);

term: LPAREN (expression | IDENTIFIER | term)  RPAREN;

type: INT | FLOAT_TYPE | STRING_TYPE | BOOL | CIRCLE | POLYGON;

expression: MINUS? INTEGER
          | MINUS? FLOAT
          | STRING
          | boolean
          | expression operator expression
          | IDENTIFIER
          | funcCall
          | shapeinit
          | term
          ;


boolean: TRUE | FALSE;

operator: PLUS | MINUS | MULTIPLY | DIVIDE | MODULO;

funcCall: IDENTIFIER LPAREN call_parameters RPAREN;

shapeinit: (POLYGON | CIRCLE) LPAREN argName arg (COMMA argName arg)* RPAREN; 

argName: IDENTIFIER COLON;

arg: (tuple | expression | IDENTIFIER);

tuple: LPAREN argName arg COMMA argName arg (COMMA argName arg)* RPAREN;

call_parameters: call_parameters COMMA call_parameter 
               | call_parameter;
               
call_parameter: (argName arg | arg);

prototypes: prototype (COMMA prototypes)?;

prototype: (type | VOID | GROUP) FUNCTION IDENTIFIER LPAREN parameters RPAREN SEMICOLON;

parameters: parameter (COMMA parameters)?
           | parameter
           | /* epsilon */
           ;

parameter: type IDENTIFIER (COMMA parameters)?
         | type IDENTIFIER
         | /* epsilon */
         ;

funcDecls: funcDecl*;

funcDecl: (type | VOID | GROUP) FUNCTION IDENTIFIER LPAREN parameters RPAREN block;

block: LBRACE statements return? RBRACE;

statements: statement  statements
          | statement 
          | /* epsilon */
          ;

statement: assignment | GROUP IDENTIFIER grouping | loop | conditional;

return: RETURN IDENTIFIER SEMICOLON
        | RETURN grouping SEMICOLON
        ;

loop: for_loop | while_loop;

for_loop: FOR LPAREN assignment condition SEMICOLON assignment RPAREN block;

while_loop: WHILE LPAREN condition RPAREN block;

condition: expression comparator expression ;

comparator: LT | GT | LE | GE | EQ | NE;

conditional: IF LPAREN condition RPAREN block;

sequences: sequence sequences?;

sequence: SEQ IDENTIFIER LPAREN parameters RPAREN seqBlock;

seqBlock: LBRACE seqBlockParts RBRACE;

seqBlockParts: statement seqBlockParts
               | statement
               | animation seqBlockParts
               | animation
               ;

animation: IDENTIFIER transitions 
         | IDENTIFIER command transitions
         ;

transitions: transition transitions
           | transition SEMICOLON
           | command transitions
           ;
           
transition: ARROW LPAREN call_parameters RPAREN;

command: ARROW IDENTIFIER LPAREN call_parameters? RPAREN;

timelineBlock: LBRACE frameDefs RBRACE;

frameDefs: frameDef* ;

frameDef: FRAME INTEGER COLON IDENTIFIER LPAREN parameters RPAREN SEMICOLON ;