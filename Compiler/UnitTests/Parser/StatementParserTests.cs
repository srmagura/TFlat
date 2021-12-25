using System.Text.Json;
using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace UnitTests.Parser;

[TestClass]
public class StatementParserTests
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

    private static string SerializeParseTree(object? node)
    {
        if (node == null) return "null";

        return JsonSerializer.Serialize(
            new { Type = node.GetType().Name, Node = node },
            JsonSerializerOptions
        );
    }

    private static void AssertParseTreesEqual(object? expected, object? actual)
    {
        Assert.AreEqual(SerializeParseTree(expected), SerializeParseTree(actual));
    }

    [TestMethod]
    public void ItParsesPrint()
    {
        var code = @"print(""hello world"");";
        var tokens = TheLexer.Lex(code);

        var result = StatementParser.Parse(tokens, 0);

        var expectedParseTree = new StatementParseNode(
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
        AssertParseTreesEqual(expectedParseTree, result?.Node);
    }
}
