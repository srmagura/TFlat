using TFlat.Compiler.Parser;

namespace TFlat.Compiler.AST;

internal static class ExpressionToAst
{
    internal static AstNode Convert(ParseNode parseNode)
    {
        return parseNode switch
        {
            IntLiteralParseNode intLiteral => ConvertIntLiteral(intLiteral),
            StringLiteralParseNode stringLiteral => ConvertStringLiteral(stringLiteral),
            BoolLiteralParseNode boolLiteral => ConvertBoolLiteral(boolLiteral),

            // UnaryOperationParseNode unaryOperation => ConvertUnaryOperation(unaryOperation),

            AdditionParseNode preAddition => ConvertPreAddition(preAddition),
            MultiplicationParseNode preMultiplication => ConvertPreMultiplication(preMultiplication),

            IdentifierExpressionParseNode identifierExpression => ConvertIdentifierExpression(identifierExpression),

            _ => throw new Exception($"Could not convert to expression: {parseNode.GetType().Name}.")
        };
    }

    internal static FunctionCallAstNode ConvertFunctionCall(FunctionCallParseNode parseNode)
    {
        var arguments = parseNode.ArgumentList.Arguments
            .Select(Convert)
            .ToArray();

        return new FunctionCallAstNode(parseNode.Function, arguments);
    }

    private static AstNode ConvertPreAddition(AdditionParseNode parseNode)
    {
        var operand0 = Convert(parseNode.Operand);

        if (parseNode.Post is PostAdditionParseNode postAddition)
        {
            return ConvertPostAddition(postAddition, operand0);
        }
        else if (parseNode.Post is EmptyParseNode)
        {
            return operand0;
        }
        else
        {
            var property = $"{nameof(AdditionParseNode)}.{nameof(AdditionParseNode.Post)}";
            throw new Exception($"{property} was ${parseNode.Post.GetType().Name} which is not allowed.");
        }
    }

    private static AstNode ConvertPostAddition(PostAdditionParseNode parseNode, AstNode operand0)
    {
        var operand1 = Convert(parseNode.Operand);
        var binaryOperation = new BinaryOperationAstNode(parseNode.Operator, operand0, operand1);

        if (parseNode.Post is PostAdditionParseNode postAddition)
        {
            return ConvertPostAddition(postAddition, binaryOperation);
        }
        else if (parseNode.Post is EmptyParseNode)
        {
            return binaryOperation;
        }
        else
        {
            var property = $"{nameof(PostAdditionParseNode)}.{nameof(PostAdditionParseNode.Post)}";
            throw new Exception($"{property} was {parseNode.Post.GetType().Name} which is not allowed.");
        }
    }

    private static AstNode ConvertPreMultiplication(MultiplicationParseNode parseNode)
    {
        var operand0 = Convert(parseNode.Operand);

        if (parseNode.Post is PostMultiplicationParseNode postMultiplication)
        {
            return ConvertPostMultiplication(postMultiplication, operand0);
        }
        else if (parseNode.Post is EmptyParseNode)
        {
            return operand0;
        }
        else
        {
            var property = $"{nameof(MultiplicationParseNode)}.{nameof(MultiplicationParseNode.Post)}";
            throw new Exception($"{property} was ${parseNode.Post.GetType().Name} which is not allowed.");
        }
    }

    private static AstNode ConvertPostMultiplication(PostMultiplicationParseNode parseNode, AstNode operand0)
    {
        var operand1 = Convert(parseNode.Operand);
        var binaryOperation = new BinaryOperationAstNode(parseNode.Operator, operand0, operand1);

        if (parseNode.Post is PostMultiplicationParseNode postMultiplication)
        {
            return ConvertPostMultiplication(postMultiplication, binaryOperation);
        }
        else if (parseNode.Post is EmptyParseNode)
        {
            return binaryOperation;
        }
        else
        {
            var property = $"{nameof(PostMultiplicationParseNode)}.{nameof(PostMultiplicationParseNode.Post)}";
            throw new Exception($"{property} was {parseNode.Post.GetType().Name} which is not allowed.");
        }
    }

    private static IntLiteralAstNode ConvertIntLiteral(IntLiteralParseNode parseNode)
    {
        return new IntLiteralAstNode(parseNode.Value);
    }

    private static StringLiteralAstNode ConvertStringLiteral(StringLiteralParseNode parseNode)
    {
        return new StringLiteralAstNode(parseNode.Value);
    }

    private static BoolLiteralAstNode ConvertBoolLiteral(BoolLiteralParseNode parseNode)
    {
        return new BoolLiteralAstNode(parseNode.Value);
    }

    private static VariableReferenceAstNode ConvertIdentifierExpression(IdentifierExpressionParseNode parseNode)
    {
        return new VariableReferenceAstNode(parseNode.Identifier);
    }

    private static UnaryOperationAstNode ConvertUnaryOperation(UnaryOperationParseNode parseNode)
    {
        return new UnaryOperationAstNode(parseNode.Operator, Convert(parseNode.Operand));
    }
}
