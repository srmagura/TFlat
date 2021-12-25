using CommandLine;
using TFlat.Shared;

namespace TFlat.Runtime;

public static class RuntimeProgram
{
    public class Options
    {
        [Value(0, Required = true)]
        public string? DllPath { get; set; }
    }

    public static async Task<ModuleAstNode> LoadAssemblyAsync(Stream dllStream)
    {
        var ast = await AstDeserializer.DeserializeAsync(dllStream);
        if (ast == null)
            throw new Exception("Failed to load DLL.");

        return ast;
    }

    public static void RunAssembly(ModuleAstNode ast, ExecutionOptions? executionOptions)
    {
        if(executionOptions != null)
            ModuleRunner.SetExecutionOptions(executionOptions);

        ModuleRunner.Run(ast);
    }

    private static async Task RunAsync(Options options)
    {
        if (options.DllPath == null)
            throw new Exception("DllPath is null.");

        ModuleAstNode ast;

        using (var dllStream = File.OpenRead(options.DllPath))
        {
            ast = await LoadAssemblyAsync(dllStream);
        }

        RunAssembly(ast, executionOptions: null);
    }

    public static async Task Main(string[] args)
    {
        await Parser.Default.ParseArguments<Options>(args)
            .WithParsedAsync(RunAsync);
    }
}
