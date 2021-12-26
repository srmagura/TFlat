using TFlat.Compiler.Parser;
using TFlat.Shared;

namespace TFlat.Compiler.AST;

internal static class ParseTreeToAst
{
    public static ModuleAstNode ConvertModule(ModuleParseNode parseNode)
    {
        var functionDeclarations = parseNode.FunctionDeclarations
            .Select(ConvertFunctionDeclaration)
            .ToArray();

        return new ModuleAstNode(functionDeclarations);
    }

    private static FunctionDeclarationAstNode ConvertFunctionDeclaration(FunctionDeclarationParseNode parseNode)
    {
        var statements = parseNode.Statements
            .Select(ConvertStatement)
            .ToArray();

        return new FunctionDeclarationAstNode(parseNode.Name, parseNode.Exported, statements);
    }

    private static AstNode ConvertStatement(ParseNode parseNode)
    {
        return parseNode switch
        {
            VariableDeclarationAndAssignmentStatementParseNode variableDeclarationAndAssignment => 
                ConvertVariableDeclarationAndAssignmentStatement(variableDeclarationAndAssignment),
            AssignmentStatementParseNode assignment => ConvertAssignmentStatement(assignment),
            FunctionCallStatementParseNode functionCall => ConvertFunctionCallStatement(functionCall),

            _ => throw new Exception($"{parseNode.GetType().Name} is not a statement.")
        };
    }

    private static VariableDeclarationAndAssignmentStatementAstNode 
        ConvertVariableDeclarationAndAssignmentStatement(VariableDeclarationAndAssignmentStatementParseNode parseNode)
    {
        return new VariableDeclarationAndAssignmentStatementAstNode(
            parseNode.Declaration.Identifier,
            parseNode.Declaration.Const,
            ConvertExpression(parseNode.Value)
        );
    }

    private static AssignmentStatementAstNode ConvertAssignmentStatement(AssignmentStatementParseNode parseNode)
    {
        return new AssignmentStatementAstNode(
            parseNode.Identifier,
            ConvertExpression(parseNode.Value)
        );
    }

    private static FunctionCallStatementAstNode ConvertFunctionCallStatement(FunctionCallStatementParseNode parseNode)
    {
        return new FunctionCallStatementAstNode(
            ConvertFunctionCall(parseNode.FunctionCall)
        );
    }

    private static FunctionCallAstNode ConvertFunctionCall(FunctionCallParseNode parseNode)
    {
        var arguments = parseNode.Arguments
            .Select(ConvertExpression)
            .ToArray();

        return new FunctionCallAstNode(parseNode.Function, arguments);
    }

    private static AstNode ConvertExpression(ParseNode parseNode)
    {
        return parseNode switch
        {
            IntLiteralParseNode intLiteral => ConvertIntLiteral(intLiteral),
            StringLiteralParseNode stringLiteral => ConvertStringLiteral(stringLiteral),
            UnaryOperationParseNode unaryOperation => ConvertUnaryOperation(unaryOperation),
            BinaryOperationParseNode binaryOperation => ConvertBinaryOperation(binaryOperation),

            IdentifierExpressionParseNode identifierExpression => ConvertIdentifierExpression(identifierExpression),

            _ => throw new Exception($"Could not convert to expression: {parseNode.GetType().Name}.")
        };
    }

    private static IntLiteralAstNode ConvertIntLiteral(IntLiteralParseNode parseNode)
    {
        return new IntLiteralAstNode(parseNode.Value);
    }

    private static StringLiteralAstNode ConvertStringLiteral(StringLiteralParseNode parseNode)
    {
        return new StringLiteralAstNode(parseNode.Value);
    }

    private static VariableReferenceAstNode ConvertIdentifierExpression(IdentifierExpressionParseNode parseNode)
    {
        return new VariableReferenceAstNode(parseNode.Identifier);
    }

    private static UnaryOperationAstNode ConvertUnaryOperation(UnaryOperationParseNode parseNode)
    {
        return new UnaryOperationAstNode(parseNode.Operator, ConvertExpression(parseNode.Operand));
    }

    private static BinaryOperationAstNode ConvertBinaryOperation(BinaryOperationParseNode parseNode)
    {
        return new BinaryOperationAstNode(
            parseNode.Operator,
            ConvertExpression(parseNode.Operand0),
            ConvertExpression(parseNode.Operand1)
        );
    }
}
