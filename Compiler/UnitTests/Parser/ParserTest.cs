using System.Text.Json;

namespace UnitTests.Parser;

public abstract class ParserTest
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

    protected static string SerializeParseTree(object? node)
    {
        return JsonSerializer.Serialize(node, JsonSerializerOptions);
    }

    protected static void AssertParseTreesEqual(object? expected, object? actual)
    {
        Assert.AreEqual(SerializeParseTree(expected), SerializeParseTree(actual));
    }
}
