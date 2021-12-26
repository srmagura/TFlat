using TFlat.Runtime.Instances;
using TFlat.Shared;

namespace TFlat.Runtime;

internal class Scope
{
    internal Dictionary<string, TfInstance> Variables { get; set; } = new();
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

    internal void PushNew()
    {
        _stack.Add(new Scope());
    }

    internal void Pop()
    {
        _stack.RemoveAt(_stack.Count - 1);
    }

    internal Scope Current => _stack[^1];

    internal ScopeStack NewForTopLevelFunction()
    {
        return new ScopeStack(_moduleScope);
    }

    internal TfInstance ResolveVariable(string name)
    {
        for (var i = _stack.Count - 1; i >= 0; i--)
        {
            var scope = _stack[i];
            if (scope.Variables.TryGetValue(name, out var variable))
                return variable;
        }

        throw new Exception($"Variable `{name}` is not defined.");
    }

    internal T ResolveVariable<T>(string name)
        where T : TfInstance
    {
        var instance = ResolveVariable(name);
        if (instance is T tInstance) return tInstance;

        throw new Exception(
            $"Variable `{name}` is defined but had type {instance.GetType().Name} " + 
            $"when {typeof(T).Name} was expected."
        );
    }
}
