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
        var parseTree = ModuleParser.Parse(tokens);
        return ParseTreeToAst.ConvertModule(parseTree);
    }

    [TestMethod]
    public void HelloWorld()
    {
        var actual = Compile(CodeFixtures.HelloWorld);

        var main = new FunctionDeclarationAstNode(
            "main",
            Exported: false,
            new[]
            {
                new FunctionCallStatementAstNode(
                    new FunctionCallAstNode(
                        "print",
                        new[]
                        {
                            new StringLiteralAstNode("hello world")
                        }
                    )
                )
            }
        );

        var expected = new ModuleAstNode(
            new[]
            {
                main
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
                new FunctionCallStatementAstNode(
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
            "main",
            Exported: false,
            Statements: new[]
            {
                new FunctionCallStatementAstNode(
                    new FunctionCallAstNode(
                        "print3",
                        Array.Empty<AstNode>()
                    )
                ),
                new FunctionCallStatementAstNode(
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

    [TestMethod]
    public void ConstVariable()
    {
        var actual = Compile(CodeFixtures.ConstVariable);

        var main = new FunctionDeclarationAstNode(
            "main",
            Exported: false,
            Statements: new AstNode[]
            {
                new VariableDeclarationAndAssignmentStatementAstNode(
                    "a", 
                    Const: true,
                    new StringLiteralAstNode("apple")
                ),
                new FunctionCallStatementAstNode(
                    new FunctionCallAstNode(
                        "print",
                        new []
                        {
                            new VariableReferenceAstNode("a")
                        }
                    )
                )
            }
        );

        var expected = new ModuleAstNode(
            new[]
            {
                main
            }
        );

        AssertAstsEqual(expected, actual);
    }

    [TestMethod]
    public void LetVariable()
    {
        var actual = Compile(CodeFixtures.LetVariable);

        var main = new FunctionDeclarationAstNode(
            "main",
            Exported: false,
            Statements: new AstNode[]
            {
                new VariableDeclarationAndAssignmentStatementAstNode(
                    "my_variable",
                    Const: false,
                    new IntLiteralAstNode(7)
                ),
                new FunctionCallStatementAstNode(
                    new FunctionCallAstNode(
                        "print",
                        new []
                        {
                            new VariableReferenceAstNode("my_variable")
                        }
                    )
                ),
                new AssignmentStatementAstNode(
                    "my_variable",
                    new IntLiteralAstNode(3)
                ),
                new FunctionCallStatementAstNode(
                    new FunctionCallAstNode(
                        "print",
                        new []
                        {
                            new VariableReferenceAstNode("my_variable")
                        }
                    )
                )
            }
        );

        var expected = new ModuleAstNode(
            new[]
            {
                main
            }
        );

        AssertAstsEqual(expected, actual);
    }
}
