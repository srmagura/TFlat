using TFlat.Runtime.Instances;

namespace TFlat.Runtime;

internal static class ExpressionEvaluator
{
    internal static TfInstance Evaluate(AstNode ast, ScopeStack scopeStack)
    {
        return ast switch
        {
            IntLiteralAstNode intLiteral => new TfInt(intLiteral.Value),
            StringLiteralAstNode stringLiteral => new TfString(stringLiteral.Value),

            VariableReferenceAstNode variableReference => scopeStack.ResolveVariable(variableReference.Identifier),

            _ => throw new Exception($"{ast.GetType().Name} is not an expression.")
        };
    }
}
