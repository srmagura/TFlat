using TFlat.Compiler.Parser.Expression;

namespace CompilerTests.Parser.Expression;

[TestClass]
public class ExpressionParserTests : ParserTest
{
    private static void TestParse(string code, AstNode expected)
    {
        // TODO will change from AdditionParser to ExpressionParser in the future
        TestParseCore(AdditionParser.Parse, ExpressionToAst.Convert, code, expected);
    }

    private static void TestDoesNotParse(string code)
    {
        // TODO will change from AdditionParser to ExpressionParser in the future
        TestDoesNotParseCore(AdditionParser.Parse, code);
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
    public void Negation()
    {
        var add17 = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(7)
        );

        var expected = new UnaryOperationAstNode(
            UnaryOperator.Negation,
            add17
        );

        TestParse("-(1+7)", expected);
    }
}
