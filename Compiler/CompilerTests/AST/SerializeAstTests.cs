using System.Text.Json;
using TFlat.Compiler.AST;
using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;
using TFlat.Shared;

namespace UnitTests.AST;

[TestClass]
public class SerializeAstTests : AstTest
{
    [TestMethod]
    public async Task ItSerializesAndDeserializes()
    {
        var tokens = TheLexer.Lex(CodeFixtures.HelloWorld);
        var parseResult = ModuleParser.Parse(tokens);
        Assert.IsNotNull(parseResult);

        var expectedAst = ParseTreeToAst.ConvertModule(parseResult.Node);

        using var memoryStream = new MemoryStream();
        await AstSerializer.SerializeAsync(expectedAst, memoryStream);

        memoryStream.Position = 0;
        var actualAst = await AstDeserializer.DeserializeAsync(memoryStream);

        AssertAstsEqual(expectedAst, actualAst);
    }
}
