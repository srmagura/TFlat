using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser;

internal static class MultiplicationParser
{
    internal static ParseResult<PreMultiplicationParseNode>? Parse(Token[] tokens, int position)
    {
        var i = position;

        var operand0Result = LiteralParser.Parse(tokens, i);
        if (operand0Result == null) return null;
        i += operand0Result.ConsumedTokens;

        var postResult = ParsePostMultiplication(tokens, i);
        if (postResult == null) return null;
        i += postResult.ConsumedTokens;

        return new ParseResult<PreMultiplicationParseNode>(
            new PreMultiplicationParseNode(operand0Result.Node, postResult.Node),
            i - position
        );
    }

    private static ParseResult<ParseNode>? ParsePostMultiplication(Token[] tokens, int position)
    {
        var result = ParsePostMultiplicationCore(tokens, position);
        if (result != null)
            return ParseResultUtil.Generic(result);

        return ParseResultUtil.Empty;
    }

    private static ParseResult<PostMultiplicationParseNode>? ParsePostMultiplicationCore(Token[] tokens, int position)
    {
        if(position >= tokens.Length) return null;

        var i = position;

        BinaryOperator? binaryOperator = tokens[i].Type switch
        {
            TokenType.Asterisk => BinaryOperator.Multiplication,
            TokenType.Slash => BinaryOperator.Division,
            TokenType.DoubleBackslash => BinaryOperator.IntegerDivision,
            _ => null
        };

        if (binaryOperator == null) return null;
        i++;

        var literalResult = LiteralParser.Parse(tokens, i);
        if (literalResult == null) return null;
        i += literalResult.ConsumedTokens;

        var postExpressionResult = ParsePostMultiplication(tokens, i);
        if (postExpressionResult == null) return null;
        i += postExpressionResult.ConsumedTokens;

        return new ParseResult<PostMultiplicationParseNode>(
            new PostMultiplicationParseNode(binaryOperator.Value, literalResult.Node, postExpressionResult.Node),
            i - position
        );
    }
}
