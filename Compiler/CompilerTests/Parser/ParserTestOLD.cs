using System.Text.Json;
using System.Text.Json.Serialization;
using TFlat.Compiler.Parser;

namespace CompilerTests.Parser;

// TODO:SAM remove
public abstract class ParserTestOLD
{
    internal class ParseNodeWriteConverter : JsonConverter<ParseNode>
    {
        public override ParseNode? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }

        public override void Write(Utf8JsonWriter writer, ParseNode value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Write the runtime type name to the JSON
            writer.WriteString("Type", value.GetType().Name);
            writer.WritePropertyName("Node");

            // typeof(object) makes it serialize the properties of the runtime type,
            // not the declared type
            JsonSerializer.Serialize(writer, value, typeof(object), options);

            writer.WriteEndObject();
        }
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = { new ParseNodeWriteConverter() },
        WriteIndented = true
    };

    internal static string SerializeParseTree(ParseNode node)
    {
        return JsonSerializer.Serialize(node, JsonSerializerOptions);
    }

    internal static void AssertParseTreesEqual(ParseNode expected, ParseNode actual)
    {
        Assert.AreEqual(SerializeParseTree(expected), SerializeParseTree(actual));
    }
}
