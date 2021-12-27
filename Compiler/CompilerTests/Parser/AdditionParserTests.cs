using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace UnitTests.Parser;

[TestClass]
public class AdditionParserTests : ParserTest
{
    private static void TestParse(string code, ParseNode expected)
    {
        var tokens = TheLexer.Lex(code);
        var result = AdditionParser.Parse(tokens, 0);

        Assert.IsNotNull(result);
        AssertParseTreesEqual(expected, result.Node);
        Assert.AreEqual(tokens.Length, result.ConsumedTokens);
    }

    [TestMethod]
    public void IntLiteral()
    {
        TestParse("2", new PreExpressionParseNode(new IntLiteralParseNode(2), new EmptyParseNode()));
    }

    [TestMethod]
    public void OneAddition()
    {
        var post = new PostExpressionParseNode(BinaryOperator.Addition, new IntLiteralParseNode(2), new EmptyParseNode());
        var expected = new PreExpressionParseNode(new IntLiteralParseNode(1), post);

        TestParse("1+2", expected);
    }

    [TestMethod]
    public void OneSubtraction()
    {
        var post = new PostExpressionParseNode(BinaryOperator.Subtraction, new IntLiteralParseNode(2), new EmptyParseNode());
        var expected = new PreExpressionParseNode(new IntLiteralParseNode(1), post);

        TestParse("1-2", expected);
    }

    [TestMethod]
    public void TwoAdditions()
    {
        var post2 = new PostExpressionParseNode(BinaryOperator.Addition, new IntLiteralParseNode(3), new EmptyParseNode());
        var post = new PostExpressionParseNode(BinaryOperator.Addition, new IntLiteralParseNode(2), post2);
        var expected = new PreExpressionParseNode(new IntLiteralParseNode(1), post);
    
        TestParse("1+2+3", expected);
    }

    [TestMethod]
    public void AddThenSubtract()
    {
        var post2 = new PostExpressionParseNode(BinaryOperator.Subtraction, new IntLiteralParseNode(3), new EmptyParseNode());
        var post = new PostExpressionParseNode(BinaryOperator.Addition, new IntLiteralParseNode(2), post2);
        var expected = new PreExpressionParseNode(new IntLiteralParseNode(1), post);

        TestParse("1+2-3", expected);
    }

    [TestMethod]
    public void SubtractThenAdd()
    {
        var post2 = new PostExpressionParseNode(BinaryOperator.Addition, new IntLiteralParseNode(3), new EmptyParseNode());
        var post = new PostExpressionParseNode(BinaryOperator.Subtraction, new IntLiteralParseNode(2), post2);
        var expected = new PreExpressionParseNode(new IntLiteralParseNode(1), post);

        TestParse("1-2+3", expected);
    }
}
