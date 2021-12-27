using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser.Expression;

internal static class ParenthesizedExpressionParser
{
    internal static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        var parenthesizedExpression = ParseParenthesized(tokens, position);
        if(parenthesizedExpression != null) 
            return ParseResultUtil.Generic(parenthesizedExpression);

        var terminal = TerminalParser.Parse(tokens, position);
        if (terminal != null)
            return ParseResultUtil.Generic(terminal);

        return null;
    }

    private static ParseResult<AdditionParseNode>? ParseParenthesized(Token[] tokens, int position)
    {
        if(position >= tokens.Length) return null;

        var i = position;
     
        if (tokens[i].Type != TokenType.OpenParenthesis) return null;
        i++;

        var expressionResult = AdditionParser.Parse(tokens, i); // TODO will change from AdditionParser in the future
        if (expressionResult == null) return null;
        i += expressionResult.ConsumedTokens;

        if (tokens[i].Type != TokenType.CloseParenthesis) return null;
        i++;

        return new ParseResult<AdditionParseNode>(
            expressionResult.Node,
            i - position
        );
    }
}
