
grammar K;


@header {
using System;
}

// general

compileUnit : 
		 expression+;

expression :
   multiplyingExpression ((PLUS | MINUS) multiplyingExpression)*
;

multiplyingExpression
   : //powExpression ((TIMES | DIV) powExpression)*
DIGIT ((TIMES | DIV) DIGIT)*   
;
	
powExpression
   : atom (POW atom)*
   ;

atom
   : variable
   | LPAREN expression RPAREN
   ;

variable
   : MINUS? LETTER (LETTER | DIGIT)*
   ;



/// Lexer rules

DIGIT
   : ('0' .. '9')
   ;
    
WS  :   (' ' | '\t' | '\r' | '\n') -> skip;

LPAREN
   : '('
   ;


RPAREN
   : ')'
   ;


PLUS
   : '+'
   ;


MINUS
   : '-' 
   ;


TIMES
   : '*'
   ;


DIV
   : '/'
   ;

POINT
   : '.'
   ;

POW
   : '^'
   ;
