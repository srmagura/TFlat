using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace UnitTests.Parser;

[TestClass]
public class ModuleParserTests : ParserTest
{
    [TestMethod]
    public void ItParsesHelloWorld()
    {
        var tokens = TheLexer.Lex(CodeFixtures.HelloWorld);
        
        var result = ModuleParser.Parse(tokens);

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

        var moduleParseTree = new ModuleParseNode(
            new []
            {
                new FunctionDeclarationParseNode(
                    "main",
                    Exported: true,
                    new []
                    {
                        statementParseTree
                    }
                )
            }
        );

        AssertParseTreesEqual(moduleParseTree, result?.Node);
    }
}
