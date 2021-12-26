using TFlat.Shared;

namespace TFlat.Runtime;

internal static class ModuleRunner
{
    private static ExecutionOptions _executionOptions = new();

    internal static void SetExecutionOptions(ExecutionOptions executionOptions)
    {
        _executionOptions = executionOptions;
    }

    internal static void Run(ModuleAstNode ast)
    {
        var moduleScope = new Scope();

        foreach (var f in ast.FunctionDeclarations)
            moduleScope.Variables[f.Name] = f;

        // Main function does not have to be exported
        if (!moduleScope.Variables.TryGetValue("main", out var mainFunction))
            throw new Exception("Module did not contain a function named \"main\".");

        var scopeStack = new ScopeStack(moduleScope);

        RunFunction(mainFunction, scopeStack);
    }

    private static void RunFunction(FunctionDeclarationAstNode functionDeclaration, ScopeStack scopeStack)
    {
        foreach (var statement in functionDeclaration.Statements)
        {
            RunStatement(statement, scopeStack);
        }
    }

    private static void RunStatement(StatementAstNode statement, ScopeStack scopeStack)
    {
        RunFunctionCall(statement.FunctionCall, scopeStack);
    }

    private static void RunFunctionCall(FunctionCallAstNode functionCall, ScopeStack scopeStack)
    {
        var functionName = functionCall.Function;

        if (functionName == "print")
        {
            Print(functionCall.Arguments);
            return;
        }

        var function = scopeStack.ResolveVariable(functionName);
        RunFunction(function, scopeStack.NewForTopLevelFunction());
    }

    private static void Print(AstNode[] arguments)
    {
        if (arguments.Length != 1)
            throw new Exception("print expected exactly 1 argument.");

        switch (arguments[0])
        {
            case IntLiteralAstNode intLiteral:
                _executionOptions.StandardOut.WriteLine(intLiteral.Value);
                break;
            case StringLiteralAstNode stringLiteral:
                _executionOptions.StandardOut.WriteLine(stringLiteral.Value);
                break;
            default:
                throw new Exception("print was called with an invalid argument.");
        }
    }
}
