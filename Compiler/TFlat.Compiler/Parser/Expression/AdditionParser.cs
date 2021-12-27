using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser;

internal static class AdditionParser
{
    internal static ParseResult<PreAdditionParseNode>? Parse(Token[] tokens, int position)
    {
        var i = position;

        var operandResult = MultiplicationParser.Parse(tokens, i);
        if (operandResult == null) return null;
        i += operandResult.ConsumedTokens;

        var postResult = ParsePostAddition(tokens, i);
        if (postResult == null) return null;
        i += postResult.ConsumedTokens;

        return new ParseResult<PreAdditionParseNode>(
            new PreAdditionParseNode(operandResult.Node, postResult.Node),
            i - position
        );
    }

    private static ParseResult<ParseNode>? ParsePostAddition(Token[] tokens, int position)
    {
        var result = ParsePostAdditionCore(tokens, position);
        if (result != null)
            return ParseResultUtil.Generic(result);

        return ParseResultUtil.Empty;
    }

    private static ParseResult<PostAdditionParseNode>? ParsePostAdditionCore(Token[] tokens, int position)
    {
        if(position >= tokens.Length) return null;

        var i = position;

        BinaryOperator? binaryOperator = tokens[i].Type switch
        {
            TokenType.Plus => BinaryOperator.Addition,
            TokenType.Minus => BinaryOperator.Subtraction,
            _ => null
        };

        if (binaryOperator == null) return null;
        i++;

        var operandResult = MultiplicationParser.Parse(tokens, i);
        if (operandResult == null) return null;
        i += operandResult.ConsumedTokens;

        var postExpressionResult = ParsePostAddition(tokens, i);
        if (postExpressionResult == null) return null;
        i += postExpressionResult.ConsumedTokens;

        return new ParseResult<PostAdditionParseNode>(
            new PostAdditionParseNode(binaryOperator.Value, operandResult.Node, postExpressionResult.Node),
            i - position
        );
    }
}
