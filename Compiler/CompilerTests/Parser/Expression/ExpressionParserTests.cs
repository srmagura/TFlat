using TFlat.Compiler.Parser.Expression;

namespace CompilerTests.Parser.Expression;

[TestClass]
public class ExpressionParserTests : ParserTest
{
    private static void TestParse(string code, AstNode expected)
    {
        TestParseCore(ExpressionParser.Parse, code, expected);
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
}
