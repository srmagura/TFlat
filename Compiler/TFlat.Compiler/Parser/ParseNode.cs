namespace TFlat.Compiler.Parser;

internal enum ParseNodeType
{
    // Literals
    BoolLiteral,
    IntLiteral,
    StringLiteral,

    // Expressions
    IdentifierExpression,
    FunctionCall,

    // Type annotations
    Type,

    // Statement parts
    VariableDeclaration,

    // Statements
    FunctionCallStatement,
    VariableDeclarationAndAssignmentStatement,

    // Top-level
    FunctionDeclaration,
    Module
}

internal abstract record ParseNode(ParseNodeType Type);

// Literals

internal record BoolLiteralParseNode(bool Value)
    : ParseNode(ParseNodeType.BoolLiteral);

internal record IntLiteralParseNode(int Value)
    : ParseNode(ParseNodeType.IntLiteral);

internal record StringLiteralParseNode(string Value) 
    : ParseNode(ParseNodeType.StringLiteral);

// Expressions

internal record IdentifierExpressionParseNode(string Identifier)
    : ParseNode(ParseNodeType.IdentifierExpression);

internal record FunctionCallParseNode(string Function, ParseNode[] Arguments)
    : ParseNode(ParseNodeType.FunctionCall);

// Type annotations

internal record TypeParseNode(string TheType)
    : ParseNode(ParseNodeType.Type);

// Statement parts

internal record VariableDeclarationParseNode(string Variable, bool Const, TypeParseNode TypeAnnotation)
    : ParseNode(ParseNodeType.VariableDeclaration);

// Statements

internal record FunctionCallStatementParseNode(FunctionCallParseNode FunctionCall)
    : ParseNode(ParseNodeType.FunctionCallStatement);

internal record VariableDeclarationAndAssignmentStatementParseNode(VariableDeclarationParseNode Declaration, ParseNode Value)
    : ParseNode(ParseNodeType.VariableDeclarationAndAssignmentStatement);

// Top-level

internal record FunctionDeclarationParseNode(string Name, bool Exported, ParseNode[] Statements)
    : ParseNode(ParseNodeType.FunctionDeclaration);

internal record ModuleParseNode(FunctionDeclarationParseNode[] FunctionDeclarations)
    : ParseNode(ParseNodeType.Module);
