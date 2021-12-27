using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser.Expression;

internal static class ExpressionParser
{
    public static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        var addition = AdditionParser.Parse(tokens, position);
        if(addition != null)
            return ParseResultUtil.Generic(addition);

        

        return null;
    }

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
            var argumentList = new ArgumentListParseNode(Array.Empty<ParseNode>());

            return new ParseResult<FunctionCallParseNode>(
                new FunctionCallParseNode(tokens[position].Value, argumentList),
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

    private static ParseResult<ArgumentListParseNode>? ParseArgumentList(Token[] tokens, int position)
    {
        // TODO support 0 or 2+ arguments
        var arg0Result = Parse(tokens, position);
        if (arg0Result == null) return null;

        return new ParseResult<ArgumentListParseNode>(
            new ArgumentListParseNode(new[] { arg0Result.Node }),
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

}
