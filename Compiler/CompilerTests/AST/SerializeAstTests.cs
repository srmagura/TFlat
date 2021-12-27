using TFlat.Compiler.AST;
using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser.Module;

namespace CompilerTests.AST;

[TestClass]
public class SerializeAstTests : AstTest
{
    [TestMethod]
    public async Task ItSerializesAndDeserializes()
    {
        var tokens = TheLexer.Lex(CodeFixtures.HelloWorld);
        var parseTree = ModuleParser.Parse(tokens);
        var expectedAst = ModuleToAst.Convert(parseTree);

        using var memoryStream = new MemoryStream();
        await AstSerializer.SerializeAsync(expectedAst, memoryStream);

        memoryStream.Position = 0;
        var actualAst = await AstDeserializer.DeserializeAsync(memoryStream);

        AssertAstsEqual(expectedAst, actualAst);
    }

    [TestMethod]
    public void AstNodeTypeMapIsComplete()
    {
        foreach (var t in Enum.GetValues<AstNodeType>())
        {
            Assert.IsInstanceOfType(AstNodeTypeMap.Map[t], typeof(Type), $"{t} is not in the map.");
        }
    }
}
