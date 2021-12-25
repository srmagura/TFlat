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
        var arguments = parseNode.Arguments
            .Select(ConvertExpression)
            .ToArray();

        return new FunctionCallAstNode(parseNode.Function, arguments);
    }

    private static AstNode ConvertExpression(ParseNode parseNode)
    {
        if (parseNode is IntLiteralParseNode intLiteral)
            return ConvertIntLiteral(intLiteral);

        if (parseNode is StringLiteralParseNode stringLiteral)
            return ConvertStringLiteral(stringLiteral);

        throw new Exception($"Could not convert to expression: {parseNode.GetType().Name}.");
    }

    private static IntLiteralAstNode ConvertIntLiteral(IntLiteralParseNode parseNode)
    {
        return new IntLiteralAstNode(parseNode.Value);
    }

    private static StringLiteralAstNode ConvertStringLiteral(StringLiteralParseNode parseNode)
    {
        return new StringLiteralAstNode(parseNode.Value);
    }
}
