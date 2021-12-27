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
        var pre = new PreExpressionParseNode(new IntLiteralParseNode(1), post);

        TestParse("1+2", pre);
    }

    [TestMethod]
    public void OneSubtraction()
    {
        TestParse("1-2", new BinaryOperationParseNode(BinaryOperator.Subtraction, new IntLiteralParseNode(1), new IntLiteralParseNode(2)));
    }

    [TestMethod]
    public void TwoAdditions()
    {
        var t1 = new BinaryOperationParseNode(BinaryOperator.Addition, new IntLiteralParseNode(1), new IntLiteralParseNode(2));
        var t2 = new BinaryOperationParseNode(BinaryOperator.Addition, t1, new IntLiteralParseNode(3));
        TestParse("1+2+3", t2);
    }

    [TestMethod]
    public void AddThenSubtract()
    {
        var t1 = new BinaryOperationParseNode(BinaryOperator.Addition, new IntLiteralParseNode(1), new IntLiteralParseNode(2));
        var t2 = new BinaryOperationParseNode(BinaryOperator.Subtraction, t1, new IntLiteralParseNode(3));
        TestParse("1+2-3", t2);
    }

    [TestMethod]
    public void SubtractThenAdd()
    {
        var t1 = new BinaryOperationParseNode(BinaryOperator.Subtraction, new IntLiteralParseNode(1), new IntLiteralParseNode(2));
        var t2 = new BinaryOperationParseNode(BinaryOperator.Addition, t1, new IntLiteralParseNode(3));
        TestParse("1-2+3", t2);
    }
}
