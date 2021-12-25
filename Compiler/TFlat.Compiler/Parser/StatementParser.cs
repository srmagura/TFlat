using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser;

internal static class StatementParser
{
    public static ParseResult<StatementParseNode>? Parse(Token[] tokens, int position)
    {
        var result = ParseFunctionCall(tokens, position);
        if (result == null) return null;

        var i = position + result.ConsumedTokens;
        if(tokens[i].Type != TokenType.Semicolon) return null;

        return new ParseResult<StatementParseNode>(
            new StatementParseNode(result.Node),
            result.ConsumedTokens + 1
        );
    }

    private static ParseResult<FunctionCallParseNode>? ParseFunctionCall(Token[] tokens, int position)
    {
        if (tokens[position].Type != TokenType.Identifier)
            return null;

        if (tokens[position + 1].Type != TokenType.OpenParen)
            return null;

        if (tokens[position + 2].Type == TokenType.CloseParen)
        {
            // Function call with no arguments
            return new ParseResult<FunctionCallParseNode>(
                new FunctionCallParseNode(tokens[position].Value, Array.Empty<ParseNode>()),
                3
            );
        }

        var argumentListResult = ParseArgumentList(tokens, position + 2);
        if (argumentListResult == null) return null;

        if (tokens[position + argumentListResult.ConsumedTokens + 2].Type != TokenType.CloseParen)
            return null;

        return new ParseResult<FunctionCallParseNode>(
            new FunctionCallParseNode(tokens[position].Value, argumentListResult.Node),
            3 + argumentListResult.ConsumedTokens
        );
    }

    private static ParseResult<ParseNode[]>? ParseArgumentList(Token[] tokens, int position)
    {
        // TODO support 0 or 2+ arguments
        var arg0Result = ExpressionParser.Parse(tokens, position);
        if(arg0Result == null) return null;

        return new ParseResult<ParseNode[]>(
            new[] { arg0Result.Node },
            arg0Result.ConsumedTokens
        );
    }
}
