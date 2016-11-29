grammar Base;

/// Parser rules

// general

compileUnit : 
		 input EOF
;


input : 
		 '{%' cleanInput '%}'
;
 

cleanInput :
		  expression ('|' params)*
;
 

number :   ('+' | '-') INT
	     | ('+' | '-') INT '.' INT
; 


// right side


params : 
		  '(' params ')' 
	    | METHOD						
		| '(' METHOD ')' ARGUMENT // '(' METHOD ')' ARGUMENT;
;		  
 

// left side


logicalExpression :                                        // incomplete
		  expression LOGICAL_OPERATOR expression
;


mathematicalExpression :                                       // incomplete
		  expression MATHEMATICAL_OPERATOR expression                    
;


// if (<condition>) { <expression> } [else { <expression> }]
// <bool> ? <true_expr> : <false_expr>
conditionExpression :																				  // incomplete
		  'if' '(' logicalExpression ')' '{' expression '}'                                       // incomplete
		| 'if' '(' logicalExpression ')' '{' expression '}' 'else' '{' expression '}'                 // do I want to differentiate this?
		| logicalExpression '?' expression ':' expression
;


// for (<variable> = <initialvalue>; <condition>; <expression>) { <expression> } 
// while (<condition>) { <expression> } 
// foreach (<variable> in <enumerable>) { <expression> }
cycleExpression :																			   // incomplete, ADD ;;;;;;;;;;;;
		   'while' '(' logicalExpression ')' '{' expression '}'
		 | 'for' '(' VARIABLE '=' number ';' logicalExpression ';' expression ')' '{' expression '}' 
		 | 'foreach' '(' VARIABLE ' in ' expression ')' '{' expression '}' 
;


// (<variable1>, <variable2>, ..., <variablen>) => <expression>
lambdaExpression :
		   METHOD '(' VARIABLE '=>' expression ')'                                             // more variables, incomplete
;


expression : 
		 | logicalExpression
		 | mathematicalExpression
		 | conditionExpression
		 | cycleExpression
		 | INT
	     | BOOL
;

/// Lexer rules

BOOL : 'True' | 'False'| 'true'| 'false';
INT : [0-9]+;     

METHOD : [a-z]+;     
VARIABLE : [a-z]+;          
ARGUMENT : [a-z]+;

LOGICAL_OPERATOR: ('&&' | '||' | 'or' | 'and' | '<' | '>' | '==' | '<=' | '>=' | '!=' | '^');
MATHEMATICAL_OPERATOR: ('+' | '-' | '*' | '/' | '^');

WS  :   (' ' | '\t' | '\r' | '\n') -> skip;
