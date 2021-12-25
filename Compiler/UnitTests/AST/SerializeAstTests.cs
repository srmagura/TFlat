using TFlat.Compiler.AST;
using TFlat.Shared;

namespace UnitTests.AST;

[TestClass]
public class SerializeAstTests
{
    [TestMethod]
    public async Task ItSerializesAndDeserializes()
    {
        var ast = new ModuleAstNode(Array.Empty<FunctionDeclarationAstNode>());

        using var memoryStream = new MemoryStream();
        await AstSerializer.SerializeAsync(ast, memoryStream);

        memoryStream.Position = 0;
        var ast2 = await AstDeserializer.DeserializeAsync(memoryStream);

        Assert.IsNotNull(ast2);
        Assert.AreEqual(0, ast2.Functions.Length);
    }
}
