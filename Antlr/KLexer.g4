lexer grammar KLexer;

LITERAL_ACCESS:      [0-9]+ IntegerTypeSuffix? '.' '@'? IdentifierOrKeyword;
	ORDERBY:       'orderby';
	PARAMS:        'params';
	
REAL_LITERAL:        [0-9]* '.' [0-9]+ ExponentPart? [FfDdMm]? | [0-9]+ ([FfDdMm] | ExponentPart [FfDdMm]?);
INTEGER_LITERAL:     [0-9]+ IntegerTypeSuffix?;

REGULAR_STRING:                      '"'  (~["\\\r\n\u0085\u2028\u2029] | CommonCharacter)* '"';
INTERPOLATED_REGULAR_STRING_START:   '$"';
DOUBLE_QUOTE_INSIDE:           '"' { interpolatedStringLevel--; interpolatedVerbatiums.Pop();
    verbatium = (interpolatedVerbatiums.Count > 0 ? interpolatedVerbatiums.Peek() : false); } -> popMode;
DOUBLE_CURLY_INSIDE:           '{{';
REGULAR_CHAR_INSIDE:           { !verbatium }? SimpleEscapeSequence;
REGULAR_STRING_INSIDE:         { !verbatium }? ~('{' | '\\' | '"')+;
FORMAT_STRING:                  ~'}'+;

	 // OK
BREAK:         'break';
CONTINUE:      'continue';
ELSE:          'else';
EQUALS:        'equals'; 
FALSE:         'false';
FOR:           'for';
IF:            'if'; 
DO: 'do';
WHERE:         'where'; 
RETURN:        'return';
WHILE:         'while';
TRUE: 'true';
NULL: 'null';

HEX_INTEGER_LITERAL: [0-9];
CHARACTER_LITERAL: [0-9] | [A-F] | [a-f];



COMMA: ',';
COLON: ':';
DOUBLE_COLON: '::';
PERIOD: '.';
QUESTION_M:  '?'; 
DOUBLE_QUESTION_M:   '??'; 
SEMICOLON: ';';

CARET:                    '^';
BANG:                     '!';
LT:                       '<';
RT:                       '>';
POINTER:                       '->';

LOWER_EQUAL : '<=';
HIGHER_EQUAL : '>=';
EQUAL: '==';
NOT_EQUAL: '!=';
 
ASSIGN: '=';
ADD_ASSIGN: '+=';
SUBSTRACT_ASSIGN: '-=';
MULTIPLY_ASSIGN: '*=';
DIVIDE_ASSIGN: '/=';
MOD_ASSIGN: '%=';
AND_ASSIGNMENT:        '&=';
OR_ASSIGNMENT:         '|=';
XOR_ASSIGNMENT:        '^=';
LEFT_SHIFT:            '<<';
LEFT_SHIFT_ASSIGNMENT: '<<=';

AND: '&&';
OR: '||';

INC: '++';
DEC: '--';
MUL: '*';
DIV: '/';
MOD: '%';
PLUS: '+';
MINUS: '-';

PIPE: '|';
WAVE_DASH: '~';
AMPERSAND: '&';

OPEN_BRACKET:             '[';
CLOSE_BRACKET:            ']';
OPEN_PARENS:              '(';
CLOSE_PARENS:             ')';
	OPEN_BRACE:               '{'
	{
	if (interpolatedStringLevel > 0)
	{
		curlyLevels.Push(curlyLevels.Pop() + 1);
	}};
	CLOSE_BRACE:              '}'
	{
	if (interpolatedStringLevel > 0)
	{
		curlyLevels.Push(curlyLevels.Pop() - 1);
		if (curlyLevels.Peek() == 0)
		{
			curlyLevels.Pop();
			Skip();
			PopMode();
		}
	}
	};

//Stop
WS
	:	' ' -> channel(HIDDEN)
	;
