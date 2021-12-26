namespace TFlat.Shared;

public enum AstNodeType
{
    IntLiteral,
    StringLiteral,

    FunctionCall,
    Statement,
    FunctionDeclaration,
    Module
}

public abstract record AstNode(AstNodeType Type);

public record IntLiteralAstNode(int Value)
    : AstNode(AstNodeType.IntLiteral);

public record StringLiteralAstNode(string Value)
    : AstNode(AstNodeType.StringLiteral);

public record FunctionCallAstNode(string Function, AstNode[] Arguments)
    : AstNode(AstNodeType.FunctionCall);

public record StatementAstNode(FunctionCallAstNode FunctionCall)
    : AstNode(AstNodeType.Statement);

public record FunctionDeclarationAstNode(string Name, bool Exported, StatementAstNode[] Statements)
    : AstNode(AstNodeType.FunctionDeclaration);

public record ModuleAstNode(FunctionDeclarationAstNode[] FunctionDeclarations)
    : AstNode(AstNodeType.Module);

public static class AstNodeTypeMap
{
    public static readonly Dictionary<AstNodeType, Type> Map = new()
    {
        [AstNodeType.IntLiteral] = typeof(IntLiteralAstNode),
        [AstNodeType.StringLiteral] = typeof(StringLiteralAstNode),

        [AstNodeType.FunctionCall] = typeof(FunctionCallAstNode),
        [AstNodeType.Statement] = typeof(StatementAstNode),
        [AstNodeType.FunctionDeclaration] = typeof(FunctionDeclarationAstNode),
        [AstNodeType.Module] = typeof(ModuleAstNode)
    };
}
