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
        var parseTree = ModuleParser.Parse(tokens);
        var expectedAst = ParseTreeToAst.ConvertModule(parseTree);

        using var memoryStream = new MemoryStream();
        await AstSerializer.SerializeAsync(expectedAst, memoryStream);

        memoryStream.Position = 0;
        var actualAst = await AstDeserializer.DeserializeAsync(memoryStream);

        AssertAstsEqual(expectedAst, actualAst);
    }
}
