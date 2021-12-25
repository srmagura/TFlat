using System;
using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace UnitTests.Parser;

[TestClass]
public class ModuleParserTests : ParserTest
{
    [TestMethod]
    public void ItParsesHelloWorld()
    {
        var code = @"
export fun main(): void {
    print(""hello world"");
}
        ";
        var tokens = TheLexer.Lex(code);
        
        var result = ModuleParser.Parse(tokens, 0);

        var statementParseTree = new StatementParseNode(
            new FunctionCallParseNode(
                "print",
                new ArgumentListParseNode(
                    new[]
                    {
                        new ExpressionParseNode(
                            new StringLiteralParseNode("hello world")
                        )
                    }
                )
            )
        );

        var moduleParseTree = new ModuleParseNode(
            new []
            {
                new FunctionDeclarationParseNode(
                    "main",
                    Exported: true,
                    new []
                    {
                        statementParseTree
                    }
                )
            }
        );

        AssertParseTreesEqual(moduleParseTree, result?.Node);
        Console.WriteLine(SerializeParseTree(result.Node));
    }
}
