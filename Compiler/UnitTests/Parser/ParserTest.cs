using System.Text.Json;

namespace UnitTests.Parser;

public abstract class ParserTest
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

    protected static string SerializeParseTree(object? node)
    {
        if (node == null) return "null";

        return JsonSerializer.Serialize(
            new { Type = node.GetType().Name, Node = node },
            JsonSerializerOptions
        );
    }

    protected static void AssertParseTreesEqual(object? expected, object? actual)
    {
        Assert.AreEqual(SerializeParseTree(expected), SerializeParseTree(actual));
    }
}
