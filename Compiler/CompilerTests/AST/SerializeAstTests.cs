using TFlat.Compiler.AST;
using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace CompilerTests.AST;

[TestClass]
public class SerializeAstTests : AstTest
{
    [TestMethod]
    public async Task ItSerializesAndDeserializes()
    {
        var tokens = TheLexer.Lex(CodeFixtures.HelloWorld);
        var parseTree = ModuleParser.Parse(tokens);
        var expectedAst = ModuleToAst.ConvertModule(parseTree);

        using var memoryStream = new MemoryStream();
        await AstSerializer.SerializeAsync(expectedAst, memoryStream);

        memoryStream.Position = 0;
        var actualAst = await AstDeserializer.DeserializeAsync(memoryStream);

        AssertAstsEqual(expectedAst, actualAst);
    }

    [TestMethod]
    public void AstNodeTypeMapIsComplete()
    {
        foreach (var x in Enum.GetValues<AstNodeType>())
        {
            Assert.IsInstanceOfType(AstNodeTypeMap.Map[x], typeof(Type), $"{x} is not in the map.");
        }
    }
}
