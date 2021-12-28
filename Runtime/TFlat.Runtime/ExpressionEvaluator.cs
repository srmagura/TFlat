using TFlat.Runtime.Instances;

namespace TFlat.Runtime;

internal static class ExpressionEvaluator
{
    internal static TfInstance Evaluate(AstNode ast, ScopeStack scopeStack)
    {
        return ast switch
        {
            IntAstNode intLiteral => new TfInt(intLiteral.Value),
            StringAstNode stringLiteral => new TfString(stringLiteral.Value),
            BoolAstNode boolLiteral => new TfBool(boolLiteral.Value),

            UnaryOperationAstNode unaryOperation => EvaluateUnaryOperation(unaryOperation, scopeStack),
            BinaryOperationAstNode binaryOperation => EvaluateBinaryOperation(binaryOperation, scopeStack),

            VariableReferenceAstNode variableReference => scopeStack.ResolveVariable(variableReference.Identifier),

            _ => throw new Exception($"{ast.GetType().Name} is not an expression.")
        };
    }

    private static TfInstance EvaluateUnaryOperation(UnaryOperationAstNode ast, ScopeStack scopeStack)
    {
        var operand = Evaluate(ast.Operand, scopeStack);

        return operand.ApplyUnaryOperator(ast.Operator);
    }

    private static TfInstance EvaluateBinaryOperation(BinaryOperationAstNode ast, ScopeStack scopeStack)
    {
        var operand0 = Evaluate(ast.Operand0, scopeStack);
        var operand1 = Evaluate(ast.Operand1, scopeStack);

        var x = operand0.ApplyBinaryOperator(ast.Operator, operand1);
        Console.WriteLine($"{operand0} {ast.Operator} {operand1} = {x}");
        return x;
    }
}
