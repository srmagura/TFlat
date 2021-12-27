using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace CompilerTests.Parser.Expression;

[TestClass]
public class MultiplicationParserTests : ParserTest
{
    private static void TestParse(string code, AstNode expected)
    {
        TestParseCore(MultiplicationParser.Parse, code, expected);
    }

    [TestMethod]
    public void IntLiteral()
    {
        TestParse("2", new IntLiteralAstNode(2));
    }

    [TestMethod]
    public void OneMultiplcation()
    {
        var expected = new BinaryOperationAstNode(
            BinaryOperator.Multiplication,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(2)
        );

        TestParse("1*2", expected);
    }

    [TestMethod]
    public void OneDivision()
    {
        var expected = new BinaryOperationAstNode(
            BinaryOperator.Division,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(2)
        );

        TestParse("1/2", expected);
    }

    [TestMethod]
    public void OneIntegerDivision()
    {
        var expected = new BinaryOperationAstNode(
            BinaryOperator.IntegerDivision,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(2)
        );

        TestParse(@"1\\2", expected);
    }

    [TestMethod]
    public void TwoMultiplications()
    {
        var multiply12 = new BinaryOperationAstNode(
            BinaryOperator.Multiplication,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(2)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Multiplication,
            multiply12,
            new IntLiteralAstNode(3)
        );

        TestParse("1*2*3", expected);
    }

    [TestMethod]
    public void MultiplyThenDivideThenIntegerDivide()
    {
        var multiply12 = new BinaryOperationAstNode(
            BinaryOperator.Multiplication,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(2)
        );

        var divideBy3 = new BinaryOperationAstNode(
            BinaryOperator.Division,
            multiply12,
            new IntLiteralAstNode(3)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.IntegerDivision,
            divideBy3,
            new IntLiteralAstNode(4)
        );

        TestParse(@"1*2/3\\4", expected);
    }
}
