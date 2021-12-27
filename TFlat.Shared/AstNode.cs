namespace TFlat.Shared;

public enum AstNodeType
{
    // Literals
    BoolLiteral,
    IntLiteral,
    StringLiteral,

    // Expressions
    VariableReference,
    FunctionCall,
    UnaryOperation,
    BinaryOperation,

    // Statements
    VariableDeclarationStatement,
    VariableDeclarationAndAssignmentStatement,
    AssignmentStatement,
    FunctionCallStatement,

    // Top-level
    FunctionDeclaration,
    Module
}

public abstract record AstNode(AstNodeType Type);

// Literals

public record BoolLiteralAstNode(bool Value)
    : AstNode(AstNodeType.BoolLiteral);

public record IntLiteralAstNode(int Value)
    : AstNode(AstNodeType.IntLiteral);

public record StringLiteralAstNode(string Value)
    : AstNode(AstNodeType.StringLiteral);

// Expressions

public record VariableReferenceAstNode(string Identifier)
    : AstNode(AstNodeType.VariableReference);

public record FunctionCallAstNode(string Function, AstNode[] Arguments)
    : AstNode(AstNodeType.FunctionCall);

public record UnaryOperationAstNode(UnaryOperator Operator, AstNode Operand)
    : AstNode(AstNodeType.UnaryOperation);

public record BinaryOperationAstNode(BinaryOperator Operator, AstNode Operand0, AstNode Operand1)
    : AstNode(AstNodeType.BinaryOperation);

// Statements

public record VariableDeclarationStatementAstNode(string Identifier)
    : AstNode(AstNodeType.VariableDeclarationStatement);

public record VariableDeclarationAndAssignmentStatementAstNode(string Identifier, bool Const, AstNode Value)
    : AstNode(AstNodeType.VariableDeclarationAndAssignmentStatement);

public record AssignmentStatementAstNode(string Identifier, AstNode Value)
    : AstNode(AstNodeType.AssignmentStatement);

public record FunctionCallStatementAstNode(FunctionCallAstNode FunctionCall)
    : AstNode(AstNodeType.FunctionCallStatement);

// Top-level

public record FunctionDeclarationAstNode(string Name, bool Exported, AstNode[] Statements)
    : AstNode(AstNodeType.FunctionDeclaration);

public record ModuleAstNode(FunctionDeclarationAstNode[] FunctionDeclarations)
    : AstNode(AstNodeType.Module);

public static class AstNodeTypeMap
{
    public static readonly Dictionary<AstNodeType, Type> Map = new()
    {
        // Literals
        [AstNodeType.BoolLiteral] = typeof(BoolLiteralAstNode),
        [AstNodeType.IntLiteral] = typeof(IntLiteralAstNode),
        [AstNodeType.StringLiteral] = typeof(StringLiteralAstNode),
        
        // Expressions
        [AstNodeType.VariableReference] = typeof(VariableReferenceAstNode),
        [AstNodeType.FunctionCall] = typeof(FunctionCallAstNode),
        [AstNodeType.UnaryOperation] = typeof(UnaryOperationAstNode),
        [AstNodeType.BinaryOperation] = typeof(BinaryOperationAstNode),

        // Statements
        [AstNodeType.VariableDeclarationStatement] = typeof(VariableDeclarationStatementAstNode),
        [AstNodeType.VariableDeclarationAndAssignmentStatement] = typeof(VariableDeclarationAndAssignmentStatementAstNode),
        [AstNodeType.AssignmentStatement] = typeof(AssignmentStatementAstNode),
        [AstNodeType.FunctionCallStatement] = typeof(FunctionCallStatementAstNode),

        // Top-level
        [AstNodeType.FunctionDeclaration] = typeof(FunctionDeclarationAstNode),
        [AstNodeType.Module] = typeof(ModuleAstNode)
    };
}
