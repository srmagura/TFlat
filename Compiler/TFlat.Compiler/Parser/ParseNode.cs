namespace TFlat.Compiler.Parser;

internal record StringLiteralParseNode(string Value);

internal record ExpressionParseNode(StringLiteralParseNode Value);

internal record ArgumentListParseNode(ExpressionParseNode[] Arguments);

internal record FunctionCallParseNode(string Function, ArgumentListParseNode ArgumentList);

internal record StatementParseNode(FunctionCallParseNode FunctionCall);

internal record FunctionDeclarationParseNode(string Name, bool Exported, StatementParseNode[] Statements);

internal record ModuleParseNode(FunctionDeclarationParseNode[] Functions);
