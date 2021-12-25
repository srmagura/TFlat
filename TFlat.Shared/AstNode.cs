namespace TFlat.Shared;

public enum AstNodeType
{
    StringLiteral,
    Expression,
    FunctionCall,
    Statement,
    FunctionDeclaration,
    Module
}

public record AstNode(AstNodeType Type);

public record StringLiteralAstNode(string Value)
    : AstNode(AstNodeType.StringLiteral);

public record ExpressionAstNode(StringLiteralAstNode Value)
    : AstNode(AstNodeType.Expression);

public record FunctionCallAstNode(string Function, ExpressionAstNode[] Arguments)
    : AstNode(AstNodeType.FunctionCall);

public record StatementAstNode(FunctionCallAstNode FunctionCall)
    : AstNode(AstNodeType.Statement);

public record FunctionDeclarationAstNode(string Name, bool Exported, StatementAstNode[] Statements)
    : AstNode(AstNodeType.FunctionDeclaration);

public record ModuleAstNode(FunctionDeclarationAstNode[] Functions)
    : AstNode(AstNodeType.Module);
