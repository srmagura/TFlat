using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser;

internal static class ExpressionParser
{
    public static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        var binaryOperation = ParseBinaryOperation(tokens, position);
        if (binaryOperation != null)
            return ParseResultUtil.Generic(binaryOperation);

        var unaryOperation = ParseUnaryOperation(tokens, position);
        if (unaryOperation != null)
            return ParseResultUtil.Generic(unaryOperation);

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

    private static ParseResult<UnaryOperationParseNode>? ParseUnaryOperation(Token[] tokens, int position)
    {
        var i = position;

        if (tokens[position].Type != TokenType.Minus) return null;
        i++;

        var operandResult = Parse(tokens, i);
        if (operandResult == null) return null;
        i += operandResult.ConsumedTokens;

        return new ParseResult<UnaryOperationParseNode>(
            new UnaryOperationParseNode(UnaryOperator.Negation, operandResult.Node),
            i - position
        );
    }

    private static readonly Dictionary<TokenType, BinaryOperator> TokenTypeToBinaryOperatorMap = new()
    {
        [TokenType.Plus] = BinaryOperator.Addition,
        [TokenType.Minus] = BinaryOperator.Subtraction,
        [TokenType.Asterisk] = BinaryOperator.Multiplication,
        [TokenType.DoubleAsterisk] = BinaryOperator.Exponentiation,
        [TokenType.Slash] = BinaryOperator.Division,
        [TokenType.DoubleBackslash] = BinaryOperator.IntegerDivision,
    };

    private static ParseResult<BinaryOperationParseNode>? ParseBinaryOperation(Token[] tokens, int position)
    {
        var i = position;

        // TODO Currently, left operator is required to be int literal to prevent infinite recursion
        var operand0Result = ParseIntLiteral(tokens, i);
        //var operand0Result = Parse(tokens, i);
        if(operand0Result == null) return null;
        i += operand0Result.ConsumedTokens;
        
        if (i >= tokens.Length) return null;

        if (!TokenTypeToBinaryOperatorMap.TryGetValue(tokens[i].Type, out var binaryOperator))
            return null;
        i++;

        if (i >= tokens.Length) return null;

        var operand1Result = Parse(tokens, i);
        if (operand1Result == null) return null;
        i += operand1Result.ConsumedTokens;

        return new ParseResult<BinaryOperationParseNode>(
            new BinaryOperationParseNode(binaryOperator, operand0Result.Node, operand1Result.Node),
            i - position
        );
    }
}
