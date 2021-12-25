using CommandLine;
using System.Text.Json;
using System.Text.Json.Serialization;
using TFlat.Shared;

namespace TFlat.Disassembler;

internal class DisassemblerProgram
{
    public class Options
    {
        [Value(0, Required = true)]
        public string? DllPath { get; set; }
    }

    private static async Task RunAsync(Options options)
    {
        if (options.DllPath == null)
            throw new Exception("DllPath is null.");

        using var dllStream = File.OpenRead(options.DllPath);

        var ast = await AstDeserializer.DeserializeAsync(dllStream);
        if (ast == null)
            throw new Exception("Failed to load DLL.");

        var json = JsonSerializer.Serialize(ast, new JsonSerializerOptions {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() },
        });
        Console.WriteLine(json);
    }

    internal static async Task Main(string[] args)
    {
        await Parser.Default.ParseArguments<Options>(args)
            .WithParsedAsync(RunAsync);
    }
}
