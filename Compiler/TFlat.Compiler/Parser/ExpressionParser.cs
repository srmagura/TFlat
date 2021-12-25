using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser;

internal static class ExpressionParser
{
    public static ParseResult<ExpressionParseNode>? ParseExpression(Token[] tokens, int position)
    {
        var stringLiteralResult = ParseStringLiteral(tokens, position);
        if (stringLiteralResult == null) return null;

        return new ParseResult<ExpressionParseNode>(
            new ExpressionParseNode(stringLiteralResult.Node),
            stringLiteralResult.ConsumedTokens
        );
    }

    public static ParseResult<StringLiteralParseNode>? ParseStringLiteral(Token[] tokens, int position)
    {
        if (tokens[position].Type != TokenType.StringLiteral)
            return null;

        var value = tokens[position].Value[1..^1];

        return new ParseResult<StringLiteralParseNode>(
            new StringLiteralParseNode(value),
            1
        );
    }
}
