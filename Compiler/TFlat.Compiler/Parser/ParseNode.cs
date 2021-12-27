namespace TFlat.Compiler.Parser;

internal abstract record ParseNode();

internal record EmptyParseNode()
    : ParseNode();

// Literals

internal record BoolLiteralParseNode(bool Value)
    : ParseNode();

internal record IntLiteralParseNode(int Value)
    : ParseNode();

internal record StringLiteralParseNode(string Value) 
    : ParseNode();

// Expressions

internal record UnaryOperationParseNode(UnaryOperator Operator, ParseNode Operand)
    : ParseNode();

/*
 * 
 * The production rules for operations like addition and multiplication have to be 
 * transformed to remove left recursion. That's why they are split up into "pre"
 * and "post" nodes. See https://www.csd.uwo.ca/~mmorenom/CS447/Lectures/Syntax.html/node8.html.
 *
 * Example
 * -------
 *
 * BEFORE:
 * 
 *    Expression => Expression + Int | Int
 *    
 * AFTER:
 * 
 *    Expression => Int Expression'
 *    Expression' => + Int Expression' | empty
 */

internal record PostMultiplicationParseNode(BinaryOperator Operator, ParseNode Operand, ParseNode Post)
    : ParseNode();

internal record PreMultiplicationParseNode(ParseNode Operand, ParseNode Post)
    : ParseNode();

internal record PostAdditionParseNode(BinaryOperator Operator, ParseNode Operand, ParseNode Post)
    : ParseNode();

internal record PreAdditionParseNode(ParseNode Operand, ParseNode Post)
    : ParseNode();

internal record IdentifierExpressionParseNode(string Identifier)
    : ParseNode();

internal record ArgumentListParseNode(ParseNode[] Arguments)
    : ParseNode();

internal record FunctionCallParseNode(string Function, ArgumentListParseNode ArgumentList)
    : ParseNode();


// Type annotations

internal record TypeParseNode(string TheType)
    : ParseNode();

// Statement parts

internal record VariableDeclarationParseNode(string Identifier, bool Const, TypeParseNode TypeAnnotation)
    : ParseNode();

// Statements

internal record FunctionCallStatementParseNode(FunctionCallParseNode FunctionCall)
    : ParseNode();

internal record VariableDeclarationAndAssignmentStatementParseNode(VariableDeclarationParseNode Declaration, ParseNode Value)
    : ParseNode();

internal record AssignmentStatementParseNode(string Identifier, ParseNode Value)
    : ParseNode();

// Top-level

internal record FunctionDeclarationParseNode(string Name, bool Exported, ParseNode[] Statements)
    : ParseNode();

internal record ModuleParseNode(FunctionDeclarationParseNode[] FunctionDeclarations)
    : ParseNode();
