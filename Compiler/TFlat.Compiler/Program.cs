using CommandLine;
using System.Text;
using TFlat.Compiler.AST;
using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

internal static class CompilerProgram
{
    public class Options
    {
        [Value(0, Required = true)]
        public string? ModulePath { get; set; }
    }

    private static async Task RunAsync(Options options)
    {
        if (options.ModulePath == null) 
            throw new Exception("ModulePath is null.");

        string code;

        using(var codeStream = File.OpenRead(options.ModulePath))
        {
            using var streamReader = new StreamReader(codeStream, Encoding.UTF8);
            code = await streamReader.ReadToEndAsync();
        }

        var tokens = TheLexer.Lex(code);
        var parseResult = ModuleParser.Parse(tokens);

        if (parseResult == null)
            throw new Exception("Failed to parse module.");

        var ast = ParseTreeToAst.ConvertModule(parseResult.Node);

        var dir = Path.GetDirectoryName(options.ModulePath);
        var dllPath = Path.Combine(dir, "Program.dll");
        
        using var dllStream = File.OpenWrite(dllPath);
        await AstSerializer.SerializeAsync(ast, dllStream);
    }

    public static async Task Main(string[] args)
    {
        await Parser.Default.ParseArguments<Options>(args)
            .WithParsedAsync(RunAsync);
    }
}
