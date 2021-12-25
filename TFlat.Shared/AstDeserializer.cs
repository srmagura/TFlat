using System.IO.Compression;
using System.Text.Json;

namespace TFlat.Shared;

public static class AstDeserializer
{
    public static async Task<ModuleAstNode?> DeserializeAsync(Stream stream)
    {
        using var brotliStream = new BrotliStream(stream, CompressionMode.Decompress);

        return await JsonSerializer.DeserializeAsync<ModuleAstNode>(brotliStream);
    }
}
