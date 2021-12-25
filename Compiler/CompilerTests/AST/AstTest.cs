using System.Text.Json;
using TFlat.Compiler.AST;
using TFlat.Shared;
namespace UnitTests.AST;

public abstract class AstTest
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { 
        Converters = { new AstSerializer.AstNodeWriteConverter() },
        WriteIndented = true
    };

    protected static string SerializeAst(ModuleAstNode ast)
    {
        return JsonSerializer.Serialize(ast, JsonSerializerOptions);
    }

    protected static void AssertAstsEqual(ModuleAstNode expected, ModuleAstNode? actual)
    {
        Assert.IsNotNull(actual);
        Assert.AreEqual(SerializeAst(expected), SerializeAst(actual));
    }
}
