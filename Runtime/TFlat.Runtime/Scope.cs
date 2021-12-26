using TFlat.Shared;

namespace TFlat.Runtime;

internal class Scope
{
    internal Dictionary<string, FunctionDeclarationAstNode> Variables { get; set; } = new();
}

internal class ScopeStack
{
    private readonly List<Scope> _stack;
    private readonly Scope _moduleScope;

    internal ScopeStack(Scope moduleScope)
    {
        _stack = new() { moduleScope };
        _moduleScope = moduleScope;
    }

    internal void Push(Scope scope)
    {
        _stack.Add(scope);
    }

    internal void Pop()
    {
        _stack.RemoveAt(_stack.Count - 1);
    }

    internal ScopeStack NewForTopLevelFunction()
    {
        return new ScopeStack(_moduleScope);
    }

    internal FunctionDeclarationAstNode ResolveVariable(string name)
    {
        for(var i = _stack.Count - 1; i >= 0; i--)
        {
            var scope = _stack[i];
            if (scope.Variables.TryGetValue(name, out var variable))
                return variable;
        }

        throw new Exception($"Variable `{name}` is not defined.");
    }
}
