using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser;

internal static class ExpressionParser
{
    internal static ParseResult<ParseNode> Generic<T>(ParseResult<T> result)
        where T : ParseNode
    {
        return new ParseResult<ParseNode>(result.Node, result.ConsumedTokens);
    }

    public static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        var intLiteral = ParseIntLiteral(tokens, position);
        if (intLiteral != null) 
            return Generic(intLiteral);

        var stringLiteral = ParseStringLiteral(tokens, position);
        if (stringLiteral != null)
            return Generic(stringLiteral);

        return null;
    }

    private static ParseResult<IntLiteralParseNode>? ParseIntLiteral(Token[] tokens, int position)
    {
        if (tokens[position].Type != TokenType.IntLiteral)
            return null;

        var value = int.Parse(tokens[position].Value);

        return new ParseResult<IntLiteralParseNode>(
            new IntLiteralParseNode(value),
            1
        );
    }

    private static ParseResult<StringLiteralParseNode>? ParseStringLiteral(Token[] tokens, int position)
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
