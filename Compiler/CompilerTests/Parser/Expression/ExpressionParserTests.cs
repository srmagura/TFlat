using TFlat.Compiler.Parser.Expression;

namespace CompilerTests.Parser.Expression;

[TestClass]
public class ExpressionParserTests : ParserTest
{
    private static void TestParse(string code, AstNode expected)
    {
        TestParseCore(ExpressionParser.Parse, ExpressionToAst.Convert, code, expected);
    }

    private static void TestDoesNotParse(string code)
    {
        TestDoesNotParseCore(ExpressionParser.Parse, code);
    }

    [TestMethod]
    public void Empty()
    {
        TestDoesNotParse("");
    }

    [TestMethod]
    public void Literals()
    {
        TestParse("2", new IntLiteralAstNode(2));
        TestParse("\"foo\"", new StringLiteralAstNode("foo"));
        TestParse("false", new BoolLiteralAstNode(false));
        TestParse("true", new BoolLiteralAstNode(true));
    }

    [TestMethod]
    public void BasicMath()
    {
        var divide23 = new BinaryOperationAstNode(
            BinaryOperator.Division,
            new IntLiteralAstNode(2),
            new IntLiteralAstNode(3)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntLiteralAstNode(1),
            divide23
        );

        TestParse("1+2/3", expected);
    }

    [TestMethod]
    public void Parentheses()
    {
        var expected = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(7)
        );

        TestParse("((1 + (7)))", expected);
    }

    [TestMethod]
    public void NumericNegation()
    {
        var add17 = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(7)
        );

        var expected = new UnaryOperationAstNode(
            UnaryOperator.NumericNegation,
            add17
        );

        TestParse("-(1+7)", expected);
    }

    [TestMethod]
    public void IntegerDivisionAndExponentiation()
    {
        var exponent23 = new BinaryOperationAstNode(
            BinaryOperator.Exponentiation,
            new IntLiteralAstNode(2),
            new IntLiteralAstNode(3)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.IntegerDivision,
            new IntLiteralAstNode(17),
            exponent23
        );

        TestParse(@"17 \\ 2**3", expected);
    }

    [TestMethod]
    public void ExponentationComesBeforeNumericNegation()
    {
        var exponent12 = new BinaryOperationAstNode(
            BinaryOperator.Exponentiation,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(2)
        );

        var expected = new UnaryOperationAstNode(
            UnaryOperator.NumericNegation,
            exponent12
        );

        TestParse(@"-1**2", expected);
    }

    [TestMethod]
    public void Modulus()
    {
        var modulus172 = new BinaryOperationAstNode(
            BinaryOperator.Modulus,
            new IntLiteralAstNode(17),
            new IntLiteralAstNode(2)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Subtraction,
            new IntLiteralAstNode(1),
            modulus172
        );

        TestParse("1 - 17 % 2", expected);
    }
}
