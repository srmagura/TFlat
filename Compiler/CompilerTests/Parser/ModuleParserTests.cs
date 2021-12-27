using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace CompilerTests.Parser;

[TestClass]
public class ModuleParserTests : ParserTestOLD
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

        var main = new FunctionDeclarationParseNode(
            "main",
            Exported: false,
            new[]
            {
                new FunctionCallStatementParseNode(
                    new FunctionCallParseNode(
                        "print",
                        new ArgumentListParseNode(
                            new[]
                            {
                                new StringLiteralParseNode("hello world")
                            }
                        )
                    )
                )
            }
        );

        var expected = new ModuleParseNode(
            new[]
            {
                main
            }
        );

        AssertParseTreesEqual(expected, actual);
    }
}
