using TFlat.Compiler.AST;
using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;
using TFlat.Shared;

namespace UnitTests.AST;

[TestClass]
public class CodeToAstTests : AstTest
{
    private static ModuleAstNode Compile(string code)
    {
        var tokens = TheLexer.Lex(code);
        var result = ModuleParser.Parse(tokens);
        Assert.IsNotNull(result);
        Assert.AreEqual(result.ConsumedTokens, tokens.Length);

        return ParseTreeToAst.ConvertModule(result.Node);
    }

    [TestMethod]
    public void HelloWorld()
    {
        var actual = Compile(CodeFixtures.HelloWorld);

        var statement = new StatementAstNode(
            new FunctionCallAstNode(
                "print",
                new[]
                {
                    new StringLiteralAstNode("hello world")
                }
            )
        );

        var expected = new ModuleAstNode(
            new[]
            {
                new FunctionDeclarationAstNode(
                    "main",
                    Exported: false,
                    new []
                    {
                        statement
                    }
                )
            }
        );

        AssertAstsEqual(expected, actual);
    }

    [TestMethod]
    public void MultipleFunctions()
    {
        var actual = Compile(CodeFixtures.MultipleFunctions);

        var print3 = new FunctionDeclarationAstNode(
            "print3",
            Exported: false,
            Statements: new[]
            {
                new StatementAstNode(
                    new FunctionCallAstNode(
                        "print",
                        new[]
                        {
                            new IntLiteralAstNode(3)
                        }
                    )
                )
            }
        );

        var main = new FunctionDeclarationAstNode(
            "print3",
            Exported: false,
            Statements: new[]
            {
                new StatementAstNode(
                    new FunctionCallAstNode(
                        "print3",
                        Array.Empty<AstNode>()
                    )
                ),
                new StatementAstNode(
                    new FunctionCallAstNode(
                        "print",
                        new[]
                        {
                            new StringLiteralAstNode(".14159")
                        }
                    )
                )
            }
        );

        var expected = new ModuleAstNode(
            new[]
            {
                print3,
                main
            }
        );

        AssertAstsEqual(expected, actual);
    }
}
