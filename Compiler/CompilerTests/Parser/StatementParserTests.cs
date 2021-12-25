using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace UnitTests.Parser;

[TestClass]
public class StatementParserTests : ParserTest
{
    [TestMethod]
    public void FunctionCallWithNoArguments()
    {
        var code = @"f();";
        var tokens = TheLexer.Lex(code);
        var result = StatementParser.Parse(tokens, 0);

        var expectedParseTree = new StatementParseNode(
            new FunctionCallParseNode(
                "f",
                Array.Empty<ParseNode>()
            )
        );

        AssertParseTreesEqual(expectedParseTree, result?.Node);
        Assert.AreEqual(tokens.Length, result?.ConsumedTokens);
    }

    [TestMethod]
    public void PrintIntLiteral()
    {
        var code = @"print(3);";
        var tokens = TheLexer.Lex(code);
        var result = StatementParser.Parse(tokens, 0);

        var expectedParseTree = new StatementParseNode(
            new FunctionCallParseNode(
                "print",
                new[]
                {
                    new IntLiteralParseNode(3)
                }
            )
        );

        AssertParseTreesEqual(expectedParseTree, result?.Node);
        Assert.AreEqual(tokens.Length, result?.ConsumedTokens);
    }

    [TestMethod]
    public void PrintStringLiteral()
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
