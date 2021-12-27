using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser;

internal static class AdditionParser
{
    /*
     * BEFORE:
     * 
     *    E => E + I | I
     *    
     * AFTER:
     * 
     *    E => I E'
     *    E' => + I E' | empty
     */

    public static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        var preExpression = ParsePreExpression(tokens, position);
        if (preExpression != null)
            return ParseResultUtil.Generic(preExpression);

        var intLiteral = ParseIntLiteral(tokens, position);
        if (intLiteral != null)
            return ParseResultUtil.Generic(intLiteral);

        return null;
    }

    private static ParseResult<PreExpressionParseNode>? ParsePreExpression(Token[] tokens, int position)
    {
        var i = position;

        var intLiteralResult = ParseIntLiteral(tokens, i);
        if (intLiteralResult == null) return null;
        i += intLiteralResult.ConsumedTokens;

        var ePrimeResult = ParsePostExpression(tokens, i);
        if (ePrimeResult == null) return null;
        i += ePrimeResult.ConsumedTokens;

        return new ParseResult<PreExpressionParseNode>(
            new PreExpressionParseNode(intLiteralResult.Node, ePrimeResult.Node),
            i - position
        );
    }

    private static ParseResult<ParseNode>? ParsePostExpression(Token[] tokens, int position)
    {
        var result = ParsePostExpressionCore(tokens, position);
        if (result != null)
            return ParseResultUtil.Generic(result);

        return ParseResultUtil.Empty;
    }

    private static ParseResult<PostExpressionParseNode>? ParsePostExpressionCore(Token[] tokens, int position)
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

        var intLiteralResult = ParseIntLiteral(tokens, i);
        if (intLiteralResult == null) return null;
        i += intLiteralResult.ConsumedTokens;

        var postExpressionResult = ParsePostExpression(tokens, i);
        if (postExpressionResult == null) return null;
        i += postExpressionResult.ConsumedTokens;

        return new ParseResult<PostExpressionParseNode>(
            new PostExpressionParseNode(binaryOperator.Value, intLiteralResult.Node, postExpressionResult.Node),
            i - position
        );
    }

    private static ParseResult<IntLiteralParseNode>? ParseIntLiteral(Token[] tokens, int position)
    {
        if(position >= tokens.Length) return null;

        if (tokens[position].Type != TokenType.IntLiteral)
            return null;

        var value = int.Parse(tokens[position].Value);

        return new ParseResult<IntLiteralParseNode>(
            new IntLiteralParseNode(value),
            1
        );
    }
}
