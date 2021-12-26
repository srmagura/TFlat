using System.Text.Json.Serialization;
using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace UnitTests.Parser;

[TestClass]
public class ExpressionParserTests : ParserTest
{
   
    private static void TestExpressionParse(string code, ParseNode expected)
    {
        var tokens = TheLexer.Lex(code);
        var result = ExpressionParser.Parse(tokens, 0);

        Assert.IsNotNull(result);
        AssertParseTreesEqual(expected, result.Node);
        Assert.AreEqual(tokens.Length, result.ConsumedTokens);
    }

    [TestMethod]
    public void IntLiteral()
    {
        TestExpressionParse("2", new IntLiteralParseNode(2));
    }

    [TestMethod]
    public void StringLiteral()
    {
        TestExpressionParse("\"foo\"", new StringLiteralParseNode("foo"));
    }

    [TestMethod]
    public void Negation()
    {
        var expected = new UnaryOperationParseNode(UnaryOperator.Negation, new IntLiteralParseNode(1));
        TestExpressionParse("-1", expected);
    }

    [TestMethod]
    public void BinaryOperation()
    {
        var expected = new BinaryOperationParseNode(
            BinaryOperator.Addition, 
            new IntLiteralParseNode(1), 
            new IntLiteralParseNode(2)
        );
        TestExpressionParse("1 + 2", expected);

        expected = new BinaryOperationParseNode(
            BinaryOperator.Subtraction,
            new IntLiteralParseNode(1),
            new IntLiteralParseNode(2)
        );
        TestExpressionParse("1 - 2", expected);

        expected = new BinaryOperationParseNode(
            BinaryOperator.Multiplication,
            new IntLiteralParseNode(1),
            new IntLiteralParseNode(2)
        );
        TestExpressionParse("1 * 2", expected);

        expected = new BinaryOperationParseNode(
            BinaryOperator.Division,
            new IntLiteralParseNode(1),
            new IntLiteralParseNode(2)
        );
        TestExpressionParse("1 / 2", expected);

        expected = new BinaryOperationParseNode(
            BinaryOperator.IntegerDivision,
            new IntLiteralParseNode(1),
            new IntLiteralParseNode(2)
        );
        TestExpressionParse("1 // 2", expected);

        expected = new BinaryOperationParseNode(
            BinaryOperator.Exponentiation,
            new IntLiteralParseNode(1),
            new IntLiteralParseNode(2)
        );
        TestExpressionParse("1 ** 2", expected);
    }
}
