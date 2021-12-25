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

    private static StatementAstNode ConvertStatement(StatementParseNode parseNode)
    {
        var functionCall = ConvertFunctionCall(parseNode.FunctionCall);

        return new StatementAstNode(functionCall);
    }

    private static FunctionCallAstNode ConvertFunctionCall(FunctionCallParseNode parseNode)
    {
        var arguments = parseNode.ArgumentList.Arguments
            .Select(ConvertExpression)
            .ToArray();

        return new FunctionCallAstNode(parseNode.Function, arguments);
    }

    private static ExpressionAstNode ConvertExpression(ExpressionParseNode parseNode)
    {
        var stringLiteral = ConvertStringLiteral(parseNode.Value);

        return new ExpressionAstNode(stringLiteral);
    }

    private static StringLiteralAstNode ConvertStringLiteral(StringLiteralParseNode parseNode)
    {
        return new StringLiteralAstNode(parseNode.Value);
    }
}
