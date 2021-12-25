internal enum AstNodeType
{
    StringLiteral,
    Expression,
    FunctionCall,
    Statement,
    FunctionDeclaration,
    Module
}

internal record AstNode(AstNodeType Type);

internal record StringLiteralAstNode(string Value)
    : AstNode(AstNodeType.StringLiteral);

internal record ExpressionAstNode(StringLiteralAstNode Value)
    : AstNode(AstNodeType.Expression);

internal record FunctionCallAstNode(string Function, ExpressionAstNode[] Arguments)
    : AstNode(AstNodeType.FunctionCall);

internal record StatementAstNode(FunctionCallAstNode FunctionCall)
    : AstNode(AstNodeType.Statement);

internal record FunctionDeclarationAstNode(string Name, bool Exported, StatementAstNode[] Statements)
    : AstNode(AstNodeType.FunctionDeclaration);

internal record ModuleAstNode(FunctionDeclarationAstNode[] Functions)
    : AstNode(AstNodeType.Module);
