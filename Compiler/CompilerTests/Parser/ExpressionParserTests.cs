using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace UnitTests.Parser;

[TestClass]
public class ExpressionParserTests
{
    [TestMethod]
    public void ItParsesIntLiteral()
    {
        var code = "2";
        var tokens = TheLexer.Lex(code);

        var result = ExpressionParser.Parse(tokens, 0);
        Assert.IsNotNull(result);
        Assert.AreEqual(2, ((IntLiteralParseNode)result.Node).Value);
        Assert.AreEqual(1, result.ConsumedTokens);
    }

    [TestMethod]
    public void ItParsesStringLiteral()
    {
        var code = "\"foo\"";
        var tokens = TheLexer.Lex(code);

        var result = ExpressionParser.Parse(tokens, 0);
        Assert.IsNotNull(result);
        Assert.AreEqual("foo", ((StringLiteralParseNode)result.Node).Value);
        Assert.AreEqual(1, result.ConsumedTokens);
    }
}
