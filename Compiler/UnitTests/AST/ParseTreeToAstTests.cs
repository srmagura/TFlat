using TFlat.Compiler.AST;
using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

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

        var moduleAst = new ModuleAstNode(
            new[]
            {
                new FunctionDeclarationAstNode(
                    "main",
                    Exported: true,
                    new []
                    {
                        statementAst
                    }
                )
            }
        );

        AssertAstsEqual(moduleAst, ast);
    }
}
