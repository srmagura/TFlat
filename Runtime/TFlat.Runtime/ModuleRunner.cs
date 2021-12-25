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
        // Main function does not have to be exported
        var mainFunction = ast.Functions.FirstOrDefault(f => f.Name == "main");
        if (mainFunction == null)
            throw new Exception("Module did not contain a function named \"main\".");

        RunDeclarationFunction(mainFunction);
    }

    private static void RunDeclarationFunction(FunctionDeclarationAstNode functionDeclaration)
    {
        foreach(var statement in functionDeclaration.Statements)
        {
            RunStatement(statement);
        }
    }

    private static void RunStatement(StatementAstNode statement)
    {
        RunFunctionCall(statement.FunctionCall);
    }

    private static void RunFunctionCall(FunctionCallAstNode functionCall)
    {
        var functionName = functionCall.Function;

        if (functionName != "print")
            throw new Exception("Unrecognized function.");

        Print(functionCall.Arguments);
    }

    private static void Print(AstNode[] arguments)
    {
        if (arguments.Length != 1)
            throw new Exception("print expected exactly 1 argument.");

        if (arguments[0] is not StringLiteralAstNode stringLiteral)
            throw new Exception("The argument to print must be a string.");

        _executionOptions.StandardOut.WriteLine(stringLiteral.Value);
    }
}
