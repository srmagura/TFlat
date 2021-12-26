using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser;

internal static class ExpressionParser
{
    public static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        var intLiteral = ParseIntLiteral(tokens, position);
        if (intLiteral != null) 
            return ParseResultUtil.Generic(intLiteral);

        var stringLiteral = ParseStringLiteral(tokens, position);
        if (stringLiteral != null)
            return ParseResultUtil.Generic(stringLiteral);

        var identifierExpression = ParseIdentifierExpression(tokens, position);
        if (identifierExpression != null)
            return ParseResultUtil.Generic(identifierExpression);

        return null;
    }

    // TODO don't use directly from StatementParser, change to private
    internal static ParseResult<FunctionCallParseNode>? ParseFunctionCall(Token[] tokens, int position)
    {
        var i = position;

        if (tokens[i].Type != TokenType.Identifier) return null;
        i++;

        if (tokens[i].Type != TokenType.OpenParen) return null;
        i++;
      
        if (tokens[i].Type == TokenType.CloseParen)
        {
            // Function call with no arguments
            return new ParseResult<FunctionCallParseNode>(
                new FunctionCallParseNode(tokens[position].Value, Array.Empty<ParseNode>()),
                3
            );
        }

        var argumentListResult = ParseArgumentList(tokens, i);
        if (argumentListResult == null) return null;
        i += argumentListResult.ConsumedTokens;

        if (tokens[i].Type != TokenType.CloseParen) return null;
        i++;

        return new ParseResult<FunctionCallParseNode>(
            new FunctionCallParseNode(tokens[position].Value, argumentListResult.Node),
            i - position
        );
    }

    private static ParseResult<ParseNode[]>? ParseArgumentList(Token[] tokens, int position)
    {
        // TODO support 0 or 2+ arguments
        var arg0Result = Parse(tokens, position);
        if (arg0Result == null) return null;

        return new ParseResult<ParseNode[]>(
            new[] { arg0Result.Node },
            arg0Result.ConsumedTokens
        );
    }

    private static ParseResult<IdentifierExpressionParseNode>? ParseIdentifierExpression(Token[] tokens, int position)
    {
        if (tokens[position].Type != TokenType.Identifier)
            return null;

        return new ParseResult<IdentifierExpressionParseNode>(
            new IdentifierExpressionParseNode(tokens[position].Value),
            1
        );
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
