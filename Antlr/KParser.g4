parser grammar KParser;

options { tokenVocab=KLexer; }
 
//! OK
argument_list
	: argument (COMMA argument)*
	; 

	//! OK
argument 
	: (identifier COLON)? expression
	;

	//! OK
expression 
	: assignment
	| non_assignment_expression 
	;

	//! OK
non_assignment_expression 
	: lambda_expression 
	| conditional_expression
	;

	//! OK
assignment 
	: unary_expression assignment_operator expression
	;

	//! OK
assignment_operator
	: ASSIGN 
	| ADD_ASSIGN 
	| SUBSTRACT_ASSIGN 
	| MULTIPLY_ASSIGN 
	| DIVIDE_ASSIGN 
	| MOD_ASSIGN 
	| AND_ASSIGNMENT
	| OR_ASSIGNMENT
	| XOR_ASSIGNMENT
	| LEFT_SHIFT
	| LEFT_SHIFT_ASSIGNMENT
	| right_shift_assignment
	;

	//! OK
conditional_expression
	: null_coalescing_expression (QUESTION_M expression COLON expression)?
	;

	//! OK
null_coalescing_expression
	: conditional_or_expression (DOUBLE_QUESTION_M null_coalescing_expression)?
	;

	//! OK
conditional_or_expression
	: conditional_and_expression (OR conditional_and_expression)*
	;

	//! OK
conditional_and_expression
	: inclusive_or_expression (AND inclusive_or_expression)*
	;

	//! OK
inclusive_or_expression
	: exclusive_or_expression (PIPE exclusive_or_expression)*
	;

		//! OK
exclusive_or_expression
	: and_expression (CARET and_expression)*
	;

		//! OK
and_expression
	: equality_expression (AMPERSAND equality_expression)*
	;

		//! OK
equality_expression
	: relational_expression ((EQUAL | NOT_EQUAL)  relational_expression)*
	;

	//! OK
relational_expression
	: shift_expression ((LT | RT | LOWER_EQUAL | HIGHER_EQUAL) shift_expression)*
	;

		//! OK
shift_expression
	: additive_expression (('<<' | right_shift)  additive_expression)*
	;

	//! OK
additive_expression
	: multiplicative_expression ((PLUS | MINUS)  multiplicative_expression)*
	;

	//! OK
multiplicative_expression
	: unary_expression ((MUL | DIV | MOD)  unary_expression)*
	;

//! OK https://msdn.microsoft.com/library/6a71f45d(v=vs.110).aspx
unary_expression
	: primary_expression 
	| PLUS unary_expression
	| MINUS unary_expression
	| BANG unary_expression
	| WAVE_DASH unary_expression
	| INC unary_expression
	| DEC unary_expression
	| AMPERSAND unary_expression
	| MUL unary_expression
	;

primary_expression  //! IN PROGRESS Null-conditional operators C# 6: https://msdn.microsoft.com/en-us/library/dn986595.aspx
	: pe=primary_expression_start bracket_expression*
	  ((member_access | method_invocation | INC | DEC | POINTER identifier) bracket_expression*)*
	;

primary_expression_start
	: literal                                   #literalExpression
	| identifier           #simpleNameExpression
	| OPEN_PARENS expression CLOSE_PARENS       #parenthesisExpressions
	| LITERAL_ACCESS                            #literalAccessExpression
	;

member_access
	: QUESTION_M? PERIOD identifier
	;

bracket_expression
	: QUESTION_M? OPEN_BRACKET indexer_argument ( COMMA indexer_argument)* CLOSE_BRACKET
	;

indexer_argument
	: (identifier COLON)? expression
	;

expression_list
	: expression (COMMA expression)*
	;

object_or_collection_initializer
	: object_initializer
	| collection_initializer
	;

object_initializer
	: OPEN_BRACE (member_initializer_list COMMA?)? CLOSE_BRACE
	;

member_initializer_list
	: member_initializer (COMMA member_initializer)*
	;

member_initializer
	: (identifier | OPEN_BRACKET expression CLOSE_BRACKET) ASSIGN initializer_value // C# 6
	;

initializer_value
	: expression
	| object_or_collection_initializer
	;

collection_initializer
	: OPEN_BRACE element_initializer (COMMA element_initializer)* COMMA? CLOSE_BRACE
	;

element_initializer
	: non_assignment_expression
	| OPEN_BRACE expression_list CLOSE_BRACE
	;

anonymous_object_initializer
	: OPEN_BRACE (member_declarator_list COMMA?)? CLOSE_BRACE
	;

member_declarator_list
	: member_declarator ( COMMA member_declarator)*
	;

member_declarator
	: primary_expression
	| identifier ASSIGN expression
	;

unbound_type_name
	: identifier ( generic_dimension_specifier? | DOUBLE_COLON identifier generic_dimension_specifier?)
	  (PERIOD identifier generic_dimension_specifier?)*
	;

generic_dimension_specifier
	: LT COMMA* RT
	;

lambda_expression
	: anonymous_function_signature right_arrow anonymous_function_body
	;

anonymous_function_signature //! OK
	: OPEN_PARENS CLOSE_PARENS
	| OPEN_PARENS explicit_anonymous_function_parameter_list CLOSE_PARENS
	| OPEN_PARENS implicit_anonymous_function_parameter_list CLOSE_PARENS
	| identifier
	;

	//! OK
explicit_anonymous_function_parameter_list
	: explicit_anonymous_function_parameter ( COMMA explicit_anonymous_function_parameter)*
	;

	//! OK
explicit_anonymous_function_parameter
	: identifier
	;

	//! OK
implicit_anonymous_function_parameter_list
	: identifier (COMMA identifier)*
	;

	//! OK
anonymous_function_body
	: expression
	| block
	;

query_body
	: orderby_clause* 
	;

// TODO
	

orderby_clause
	: ORDERBY ordering (COMMA  ordering)*
	;

ordering
	: expression;
//B.2.5 Statements
statement
	: identifier COLON statement                                       #labeledStatement
	| (local_variable_declaration) SEMICOLON  #declarationStatement
	| embedded_statement                                             #embeddedStatement
	;

embedded_statement
	: block
	| simple_embedded_statement
	;

simple_embedded_statement
	: SEMICOLON                                                         #emptyStatement
	| expression SEMICOLON                                              #expressionStatement

	// selection statements
	| IF OPEN_PARENS expression CLOSE_PARENS if_body (ELSE if_body)?    #ifStatement
    
    // iteration statements
	| WHILE OPEN_PARENS expression CLOSE_PARENS embedded_statement                                        #whileStatement
	| DO embedded_statement WHILE OPEN_PARENS expression CLOSE_PARENS SEMICOLON                                 #doStatement
	| FOR OPEN_PARENS for_initializer? SEMICOLON expression? SEMICOLON for_iterator? CLOSE_PARENS embedded_statement  #forStatement

    // jump statements
	| BREAK SEMICOLON                                                   #breakStatement
	| CONTINUE SEMICOLON													 #continueStatement
	| RETURN expression? SEMICOLON									  #returnStatement
	;

	//! OK
block
	: OPEN_BRACE statement_list? CLOSE_BRACE
	;

local_variable_declaration
	: local_variable_declarator ( COMMA  local_variable_declarator)*
	;

local_variable_declarator
	: identifier (ASSIGN local_variable_initializer)?
	;

local_variable_initializer
	: expression
	;

if_body
	: block
	| simple_embedded_statement
	;

	//! OK
statement_list
	: statement+
	;

for_initializer
	: local_variable_declaration
	| expression (COMMA  expression)*
	;

for_iterator
	: expression (COMMA  expression)*
	;

	//! OK
right_arrow
	: first=ASSIGN second=RT {$first.index + 1 == $second.index}? // Nothing between the tokens?
	;

	//! OK
right_shift
	: first=RT second=RT {$first.index + 1 == $second.index}? // Nothing between the tokens?
	;

	//! OK
right_shift_assignment
	: first=RT second=HIGHER_EQUAL {$first.index + 1 == $second.index}? // Nothing between the tokens?
	;

literal
	: boolean_literal
	| string_literal
	| INTEGER_LITERAL
	| HEX_INTEGER_LITERAL
	| REAL_LITERAL
	| CHARACTER_LITERAL
	| NULL
	;

boolean_literal
	: TRUE
	| FALSE
	;

string_literal
	: interpolated_regular_string
	| REGULAR_STRING
	;

interpolated_regular_string
	: INTERPOLATED_REGULAR_STRING_START interpolated_regular_string_part* DOUBLE_QUOTE_INSIDE
	;

interpolated_regular_string_part
	: interpolated_string_expression
	| DOUBLE_CURLY_INSIDE
	| REGULAR_CHAR_INSIDE
	| REGULAR_STRING_INSIDE
	;

interpolated_string_expression
	: expression (COMMA expression)* (COLON FORMAT_STRING+)?
	;

//B.1.7 Keywords
keyword
	: BREAK
	| CONTINUE
	| ELSE
	| FALSE
	| FOR
	| FOREACH
	| IF
	| IN //?
	| NULL
	| RETURN	
	| TRUE
	| WHILE
	;

// -------------------- extra rules for modularization --------------------------------
method_member_name
	: (identifier | identifier DOUBLE_COLON identifier) (PERIOD identifier)*
	;

arg_declaration
	: identifier (ASSIGN expression)?
	;

method_invocation
	: OPEN_PARENS argument_list? CLOSE_PARENS
	;

object_creation_expression
	: OPEN_PARENS argument_list? CLOSE_PARENS object_or_collection_initializer?
	;

identifier
	: IDENTIFIER	
	| EQUALS
	| ORDERBY
	| WHERE
	;