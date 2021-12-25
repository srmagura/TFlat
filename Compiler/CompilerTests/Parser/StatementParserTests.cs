using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace UnitTests.Parser;

[TestClass]
public class StatementParserTests : ParserTest
{
    [TestMethod]
    public void ItParsesPrint()
    {
        var code = @"print(""hello world"");";
        var tokens = TheLexer.Lex(code);
        
        var result = StatementParser.Parse(tokens, 0);

        var expectedParseTree = new StatementParseNode(
            new FunctionCallParseNode(
                "print",
                new[]
                {
                    new StringLiteralParseNode("hello world")
                }
            )
        );

        AssertParseTreesEqual(expectedParseTree, result?.Node);
        Assert.AreEqual(tokens.Length, result?.ConsumedTokens);
    }
}
