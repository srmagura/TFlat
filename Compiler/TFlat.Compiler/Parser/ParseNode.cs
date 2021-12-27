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

internal record PostExpressionParseNode(BinaryOperator Operator, IntLiteralParseNode IntLiteral, ParseNode? PostExpression)
    : ParseNode();

internal record PreExpressionParseNode(IntLiteralParseNode IntLiteral, ParseNode EPrime)
    : ParseNode();

internal record IdentifierExpressionParseNode(string Identifier)
    : ParseNode();

internal record ArgumentListParseNode(ParseNode[] Arguments)
    : ParseNode();

internal record FunctionCallParseNode(string Function, ArgumentListParseNode ArgumentList)
    : ParseNode();

internal record UnaryOperationParseNode(UnaryOperator Operator, ParseNode Operand)
    : ParseNode();

internal record BinaryOperationParseNode(BinaryOperator Operator, ParseNode Operand0, ParseNode Operand1)
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
