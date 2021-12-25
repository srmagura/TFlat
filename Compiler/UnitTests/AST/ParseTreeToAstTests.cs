using TFlat.Compiler.AST;
using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;
using TFlat.Shared;

namespace UnitTests.AST;

[TestClass]
public class ParseTreeToAstTests : AstTest
{
    [TestMethod]
    public void ItCreatesAnAstForHelloWorld()
    {
        var tokens = TheLexer.Lex(CodeFixtures.HelloWorld);
        var parseResult = ModuleParser.Parse(tokens);
        Assert.IsNotNull(parseResult);

        var ast = ParseTreeToAst.ConvertModule(parseResult.Node);

        var statementAst = new StatementAstNode(
            new FunctionCallAstNode(
                "print",
                new[]
                {
                    new ExpressionAstNode(
                        new StringLiteralAstNode("hello world")
                    )
                }
            )
        );

        var expectedAst = new ModuleAstNode(
            new[]
            {
                new FunctionDeclarationAstNode(
                    "main",
                    Exported: false,
                    new []
                    {
                        statementAst
                    }
                )
            }
        );
        AssertAstsEqual(expectedAst, ast);
    }
}
