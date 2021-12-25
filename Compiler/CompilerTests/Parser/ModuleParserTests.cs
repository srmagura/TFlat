using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace UnitTests.Parser;

[TestClass]
public class ModuleParserTests : ParserTest
{
    [TestMethod]
    public void ItParsesHelloWorld()
    {
        var actual = Parse(CodeFixtures.HelloWorld);

        var statementParseTree = new StatementParseNode(
            new FunctionCallParseNode(
                "print",
                new ArgumentListParseNode(
                    new[]
                    {
                        new ExpressionParseNode(
                            new StringLiteralParseNode("hello world")
                        )
                    }
                )
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
                        statementParseTree
                    }
                )
            }
        );

        AssertParseTreesEqual(expected, actual);
    }
}
