namespace TFlat.Runtime.Instances;

internal class TfFunction : TfObject
{
    public TfFunction(string name, FunctionDeclarationAstNode ast)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Ast = ast ?? throw new ArgumentNullException(nameof(ast));
    }

    public string Name { get; private init; }
    public FunctionDeclarationAstNode Ast { get; private init; }

    public override String ToString()
    {
        return $"Function:${Name}";
    }
}
