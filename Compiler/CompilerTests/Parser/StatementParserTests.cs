using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser.Module;

namespace CompilerTests.Parser;

[TestClass]
public class StatementParserTests : ParserTest
{
    private static void TestParse(string code, AstNode expected)
    {
        TestParseCore(StatementParser.Parse, StatementToAst.Convert, code, expected);
    }

    [TestMethod]
    public void FunctionCallWithNoArguments()
    {
        var expected = new FunctionCallStatementAstNode(
            new FunctionCallAstNode(
                "f",
                Array.Empty<AstNode>()
            )
        );

        TestParse("f();", expected);
    }

    [TestMethod]
    public void PrintIntLiteral()
    {
        var expected = new FunctionCallStatementAstNode(
            new FunctionCallAstNode(
                "print",
                new[]
                {
                    new IntLiteralAstNode(3)
                }
            )
        );

        TestParse("print(3);", expected);
    }

    [TestMethod]
    public void PrintStringLiteral()
    {
        var expected = new FunctionCallStatementAstNode(
            new FunctionCallAstNode(
                "print",
                new[]
                {
                    new StringLiteralAstNode("hello world")
                }
            )
        );

        TestParse(@"print(""hello world"");", expected);
    }

    [TestMethod]
    public void VariableDeclaration()
    {
        var expected = new VariableDeclarationStatementAstNode("a");

        TestParse("let a: string;", expected);
    }

    [TestMethod]
    public void ConstVariable()
    {
        var expected = new VariableDeclarationAndAssignmentStatementAstNode(
            "a",
            Const: true,
            new StringLiteralAstNode("apple")
        );

        TestParse(@"const a: string = ""apple"";", expected);
    }

    [TestMethod]
    public void LetVariableWithAssignment()
    {
        var expected = new VariableDeclarationAndAssignmentStatementAstNode(
            "my_variable",
            Const: false,
            new IntLiteralAstNode(7)
        );

        TestParse("let my_variable: int = 7;", expected);
    }

    [TestMethod]
    public void Assignment()
    {
        var expected = new AssignmentStatementAstNode(
            "my_variable",
            new IntLiteralAstNode(3)
        );

        TestParse("my_variable = 3;", expected);
    }

    [TestMethod]
    public void PrintVariable()
    {
        var expected = new FunctionCallStatementAstNode(
            new FunctionCallAstNode(
                "print",
                new[]
                {
                    new VariableReferenceAstNode("a")
                }
            )
        );

        TestParse("print(a);", expected);
    }
}
