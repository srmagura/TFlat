namespace TFlat.Compiler.Parser;

internal enum ParseNodeType
{
    StringLiteral,
    Expression,
    ArgumentList,
    FunctionCall,
    Statement,
    FunctionDeclaration,
    Module
}

internal record ParseNode(ParseNodeType Type);

internal record StringLiteralParseNode(string Value) 
    : ParseNode(ParseNodeType.StringLiteral);

internal record ExpressionParseNode(StringLiteralParseNode Value) 
    : ParseNode(ParseNodeType.Expression);

internal record ArgumentListParseNode(ExpressionParseNode[] Arguments) 
    : ParseNode(ParseNodeType.ArgumentList);

internal record FunctionCallParseNode(string Function, ArgumentListParseNode ArgumentList)
    : ParseNode(ParseNodeType.FunctionCall);

internal record StatementParseNode(FunctionCallParseNode FunctionCall)
    : ParseNode(ParseNodeType.Statement);

internal record FunctionDeclarationParseNode(string Name, bool Exported, StatementParseNode[] Statements)
    : ParseNode(ParseNodeType.FunctionDeclaration);

internal record ModuleParseNode(FunctionDeclarationParseNode[] FunctionDeclarations)
    : ParseNode(ParseNodeType.Module);
