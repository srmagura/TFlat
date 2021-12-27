using TFlat.Runtime.Instances;

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
            moduleScope.Variables[f.Name] = new TfFunction(f.Name, f);

        var scopeStack = new ScopeStack(moduleScope);

        // Main function does not have to be exported
        var main = scopeStack.ResolveVariable<TfFunction>("main");

        RunFunction(main, scopeStack);
    }

    private static void RunFunction(TfFunction function, ScopeStack scopeStack)
    {
        scopeStack.PushNew();

        foreach (var statement in function.Ast.Statements)
        {
            RunStatement(statement, scopeStack);
        }
    }

    private static void RunStatement(AstNode statement, ScopeStack scopeStack)
    {
        switch (statement)
        {
            case VariableDeclarationAndAssignmentStatementAstNode variableDeclarationAndAssignment:
                RunVariableDeclarationAndAssignmentStatement(variableDeclarationAndAssignment, scopeStack);
                break;
            case AssignmentStatementAstNode assignment:
                RunAssignmentStatement(assignment, scopeStack);
                break;
            case FunctionCallStatementAstNode functionCall:
                RunFunctionCallStatement(functionCall, scopeStack);
                break;
            default:
                throw new Exception($"{statement.GetType().Name} is not a statement.");
        }
    }

    private static void RunVariableDeclarationAndAssignmentStatement(
        VariableDeclarationAndAssignmentStatementAstNode variableDeclarationAndAssignmentStatement,
        ScopeStack scopeStack
    )
    {
        scopeStack.Current.Variables[variableDeclarationAndAssignmentStatement.Identifier] =
            ExpressionEvaluator.Evaluate(variableDeclarationAndAssignmentStatement.Value, scopeStack);
    }

    private static void RunAssignmentStatement(
        AssignmentStatementAstNode assignmentStatement,
        ScopeStack scopeStack
    )
    {
        scopeStack.Current.Variables[assignmentStatement.Identifier] =
            ExpressionEvaluator.Evaluate(assignmentStatement.Value, scopeStack);
    }

    private static void RunFunctionCallStatement(FunctionCallStatementAstNode functionCallStatement, ScopeStack scopeStack)
    {
        RunFunctionCall(functionCallStatement.FunctionCall, scopeStack);
    }

    private static void RunFunctionCall(FunctionCallAstNode functionCall, ScopeStack scopeStack)
    {
        var functionName = functionCall.Function;

        if (functionName == "print")
        {
            Print(functionCall.Arguments, scopeStack);
            return;
        }

        var function = scopeStack.ResolveVariable<TfFunction>(functionName);
        RunFunction(function, scopeStack.NewForTopLevelFunction());
    }

    private static void Print(AstNode[] arguments, ScopeStack scopeStack)
    {
        if (arguments.Length != 1)
            throw new Exception("print expected exactly 1 argument.");

        var argument = ExpressionEvaluator.Evaluate(arguments[0], scopeStack);
        _executionOptions.StandardOut.WriteLine(argument.ToString());
    }
}
