using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TFlat.Shared;

public static class AstDeserializer
{
    private class AstNodeReadConverter : JsonConverter<AstNode>
    {
        public override AstNode? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var astNodeType = (AstNodeType)jsonDocument.RootElement.GetProperty("Type").GetInt32();
            var type = AstNodeTypeMap.Map[astNodeType];

            return (AstNode?)JsonSerializer.Deserialize(jsonDocument, type, options);
        }

        public override void Write(Utf8JsonWriter writer, AstNode value, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = { new AstNodeReadConverter() }
    };

    public static async Task<ModuleAstNode?> DeserializeAsync(Stream stream)
    {
        using var brotliStream = new BrotliStream(stream, CompressionMode.Decompress);

        return await JsonSerializer.DeserializeAsync<ModuleAstNode>(brotliStream, JsonSerializerOptions);
    }
}
