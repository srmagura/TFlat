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

    private static FunctionCallStatementAstNode Print(AstNode argument)
    {
        return new FunctionCallStatementAstNode(
            new FunctionCallAstNode(
                "print",
                new[] { argument }
            )
        );
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
                Print(new StringLiteralAstNode("hello world"))
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
                Print(new IntLiteralAstNode(3))
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
                Print(new StringLiteralAstNode(".14159"))
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
                Print(new VariableReferenceAstNode("a"))
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
                Print(new VariableReferenceAstNode("my_variable")),
                new AssignmentStatementAstNode(
                    "my_variable",
                    new IntLiteralAstNode(3)
                ),
                Print(new VariableReferenceAstNode("my_variable")),
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
    public void MathOperators()
    {
        var actual = Compile(CodeFixtures.MathOperators);

        var main = new FunctionDeclarationAstNode(
            "main",
            Exported: false,
            Statements: new AstNode[]
            {
                Print(new UnaryOperationAstNode(
                    UnaryOperator.Negation, 
                    new IntLiteralAstNode(3)
                )),
                Print(new BinaryOperationAstNode(
                    BinaryOperator.Addition,
                    new IntLiteralAstNode(1),
                    new IntLiteralAstNode(2)
                )),
                Print(new BinaryOperationAstNode(
                    BinaryOperator.Subtraction,
                    new IntLiteralAstNode(1),
                    new IntLiteralAstNode(2)
                )),
                Print(new BinaryOperationAstNode(
                    BinaryOperator.Multiplication,
                    new IntLiteralAstNode(1),
                    new IntLiteralAstNode(2)
                )),
                Print(new BinaryOperationAstNode(
                    BinaryOperator.IntegerDivision,
                    new IntLiteralAstNode(1),
                    new IntLiteralAstNode(2)
                )),
                Print(new BinaryOperationAstNode(
                    BinaryOperator.IntegerDivision,
                    new IntLiteralAstNode(2),
                    new IntLiteralAstNode(2)
                )),
                Print(new BinaryOperationAstNode(
                    BinaryOperator.Exponentiation,
                    new IntLiteralAstNode(2),
                    new IntLiteralAstNode(3)
                )),
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
