using TFlat.Compiler.AST;
using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser.Expression;

namespace CompilerTests.Parser.Expression;

[TestClass]
public class AdditionParserTests : ParserTest
{
    private static void TestParse(string code, AstNode expected)
    {
        TestParseCore(AdditionParser.Parse, ExpressionToAst.Convert, code, expected);
    }

    [TestMethod]
    public void IntLiteral()
    {
        TestParse("2", new IntLiteralAstNode(2));
    }

    [TestMethod]
    public void OneAddition()
    {
        var expected = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(2)
        );

        TestParse("1+2", expected);
    }

    [TestMethod]
    public void OneSubtraction()
    {
        var expected = new BinaryOperationAstNode(
            BinaryOperator.Subtraction,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(2)
        );

        TestParse("1-2", expected);
    }

    [TestMethod]
    public void TwoAdditions()
    {
        var addition12 = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(2)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            addition12,
            new IntLiteralAstNode(3)
        );

        TestParse("1+2+3", expected);
    }

    [TestMethod]
    public void AddThenSubtract()
    {
        var addition12 = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(2)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Subtraction,
            addition12,
            new IntLiteralAstNode(3)
        );

        TestParse("1+2-3", expected);
    }

    [TestMethod]
    public void MultiplyThenSubtract()
    {
        var multiply12 = new BinaryOperationAstNode(
            BinaryOperator.Multiplication,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(2)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Subtraction,
            multiply12,
            new IntLiteralAstNode(3)
        );

        TestParse("1*2-3", expected);
    }

    [TestMethod]
    public void AddThenDivide()
    {
        var divide12 = new BinaryOperationAstNode(
            BinaryOperator.Division,
            new IntLiteralAstNode(1),
            new IntLiteralAstNode(2)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntLiteralAstNode(0),
            divide12
        );

        TestParse("0+1/2", expected);
    }
}
