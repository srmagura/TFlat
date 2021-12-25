using System.IO.Compression;
using System.Text;
using System.Text.Json;
using TFlat.Shared;

namespace TFlat.Compiler.AST;

internal class AstSerializer
{
    public static async Task SerializeAsync(ModuleAstNode ast, Stream outStream)
    {
        using var brotliStream = new BrotliStream(outStream, CompressionLevel.Fastest, leaveOpen: true);
        
        await JsonSerializer.SerializeAsync(brotliStream, ast);
        await brotliStream.FlushAsync();
    }
}
