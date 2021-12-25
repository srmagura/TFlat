using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace UnitTests.Parser;

[TestClass]
public class ExpressionParserTests
{
    [TestMethod]
    public void ItParsesStringLiteral()
    {
        var code = "\"foo\"";
        var tokens = TheLexer.Lex(code);

        var stringLiteralResult = ExpressionParser.ParseStringLiteral(tokens, 0);
        Assert.IsNotNull(stringLiteralResult);
        Assert.AreEqual("foo", stringLiteralResult.Node.Value);
        Assert.AreEqual(1, stringLiteralResult.ConsumedTokens);

        var result = ExpressionParser.ParseExpression(tokens, 0);
        Assert.IsNotNull(result);
        Assert.AreEqual(stringLiteralResult.Node, result.Node.Value);
        Assert.AreEqual(1, result.ConsumedTokens);
    }
}
