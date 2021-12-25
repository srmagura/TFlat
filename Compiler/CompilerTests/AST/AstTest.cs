using System.Text.Json;

namespace UnitTests.AST;

public abstract class AstTest
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

    protected static string SerializeAst(object? node)
    {
        return JsonSerializer.Serialize(node, JsonSerializerOptions);
    }

    protected static void AssertAstsEqual(object? expected, object? actual)
    {
        Assert.AreEqual(SerializeAst(expected), SerializeAst(actual));
    }
}
