namespace TFlat.Compiler.Parser;

internal enum ParseNodeType
{
    IntLiteral,
    StringLiteral,

    FunctionCall,
    Statement,
    FunctionDeclaration,
    Module
}

internal abstract record ParseNode(ParseNodeType Type);

internal record IntLiteralParseNode(int Value)
    : ParseNode(ParseNodeType.IntLiteral);

internal record StringLiteralParseNode(string Value) 
    : ParseNode(ParseNodeType.StringLiteral);

internal record FunctionCallParseNode(string Function, ParseNode[] Arguments)
    : ParseNode(ParseNodeType.FunctionCall);

internal record StatementParseNode(FunctionCallParseNode FunctionCall)
    : ParseNode(ParseNodeType.Statement);

internal record FunctionDeclarationParseNode(string Name, bool Exported, StatementParseNode[] Statements)
    : ParseNode(ParseNodeType.FunctionDeclaration);

internal record ModuleParseNode(FunctionDeclarationParseNode[] FunctionDeclarations)
    : ParseNode(ParseNodeType.Module);
