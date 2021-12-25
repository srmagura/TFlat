using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace UnitTests.Parser;

[TestClass]
public class ModuleParserTests : ParserTest
{
    private static ModuleParseNode Parse(string code)
    {
        var tokens = TheLexer.Lex(code);
        return ModuleParser.Parse(tokens);
    }

    [TestMethod]
    public void ItParsesHelloWorld()
    {
        var actual = Parse(CodeFixtures.HelloWorld);

        var statement = new StatementParseNode(
            new FunctionCallParseNode(
                "print",
                new[]
                {
                    new StringLiteralParseNode("hello world")
                }
            )
        );

        var expected = new ModuleParseNode(
            new[]
            {
                new FunctionDeclarationParseNode(
                    "main",
                    Exported: false,
                    new []
                    {
                        statement
                    }
                )
            }
        );

        AssertParseTreesEqual(expected, actual);
    }
}
