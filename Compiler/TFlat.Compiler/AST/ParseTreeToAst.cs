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
            //VariableDeclarationStatementParseNode variableDeclaration => 
            //    ConvertVariableDeclaration(variableDeclaration),
            VariableDeclarationAndAssignmentStatementParseNode variableDeclarationAndAssignmentStatement => 
                ConvertVariableDeclarationAndAssignmentStatement(variableDeclarationAndAssignmentStatement),
            FunctionCallStatementParseNode functionCallStatement => ConvertFunctionCallStatement(functionCallStatement),

            _ => throw new Exception($"{parseNode.GetType().Name} is not a statement.")
        };
    }

    private static VariableDeclarationAndAssignmentStatementAstNode 
        ConvertVariableDeclarationAndAssignmentStatement(VariableDeclarationAndAssignmentStatementParseNode parseNode)
    {
        return new VariableDeclarationAndAssignmentStatementAstNode(
            parseNode.Declaration.Variable,
            parseNode.Declaration.Const,
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
}
