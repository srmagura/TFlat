using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace UnitTests.Parser;

[TestClass]
public class StatementParserTests : ParserTest
{
    private static void TestStatementParse(string code, ParseNode expected)
    {
        var tokens = TheLexer.Lex(code);
        var result = StatementParser.Parse(tokens, 0);

        Assert.IsNotNull(result);
        AssertParseTreesEqual(expected, result.Node);
        Assert.AreEqual(tokens.Length, result.ConsumedTokens);
    }

    [TestMethod]
    public void FunctionCallWithNoArguments()
    {
        var code = @"f();";

        var expected = new FunctionCallStatementParseNode(
            new FunctionCallParseNode(
                "f",
                Array.Empty<ParseNode>()
            )
        );

        TestStatementParse(code, expected);
    }

    [TestMethod]
    public void PrintIntLiteral()
    {
        var code = @"print(3);";

        var expected = new FunctionCallStatementParseNode(
            new FunctionCallParseNode(
                "print",
                new[]
                {
                    new IntLiteralParseNode(3)
                }
            )
        );

        TestStatementParse(code, expected);
    }

    [TestMethod]
    public void PrintStringLiteral()
    {
        var code = @"print(""hello world"");";

        var expected = new FunctionCallStatementParseNode(
            new FunctionCallParseNode(
                "print",
                new[]
                {
                    new StringLiteralParseNode("hello world")
                }
            )
        );

        TestStatementParse(code, expected);
    }

    [TestMethod]
    public void VariableDeclaration()
    {
        var tokens = TheLexer.Lex("const a: string");
        var result = StatementParser.ParseVariableDeclaration(tokens, 0);

        var expected = new VariableDeclarationParseNode(
            "a",
            Const: true,
            TypeAnnotation: new TypeParseNode("string")
        );

        Assert.IsNotNull(result);
        AssertParseTreesEqual(expected, result.Node);
        Assert.AreEqual(tokens.Length, result.ConsumedTokens);
    }

    [TestMethod]
    public void ConstVariable()
    {
        var code = @"const a: string = ""apple"";";

        var expected = new VariableDeclarationAndAssignmentStatementParseNode(
            new VariableDeclarationParseNode(
                "a",
                Const: true,
                new TypeParseNode("string")
            ),
            new StringLiteralParseNode("apple")
        );

        TestStatementParse(code, expected);
    }

    [TestMethod]
    public void LetVariable()
    {
        var code = @"let my_variable: int = 7;";

        var expected = new VariableDeclarationAndAssignmentStatementParseNode(
            new VariableDeclarationParseNode(
                "my_variable",
                Const: false,
                new TypeParseNode("int")
            ),
            new IntLiteralParseNode(7)
        );

        TestStatementParse(code, expected);
    }

    [TestMethod]
    public void Assignment()
    {
        var code = @"my_variable = 3;";

        var expected = new AssignmentStatementParseNode(
            "my_variable",
            new IntLiteralParseNode(3)
        );

        TestStatementParse(code, expected);
    }

    [TestMethod]
    public void PrintVariable()
    {
        var code = @"print(a);";

        var expected = new FunctionCallStatementParseNode(
            new FunctionCallParseNode(
                "print",
                new[]
                {
                    new IdentifierExpressionParseNode("a")
                }
            )
        );

        TestStatementParse(code, expected);
    }
}
