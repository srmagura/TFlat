using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;

namespace CompilerTests.Parser;

public abstract class ParserTest : AstTest
{
    internal static void TestParseCore<T>(Func<Token[], int, ParseResult<T>?> parseFunc, string code, AstNode expected)
        where T : ParseNode
    {
        var tokens = TheLexer.Lex(code);
        var parseResult = parseFunc(tokens, 0);
        Assert.IsNotNull(parseResult);
        Assert.AreEqual(
            tokens.Length,
            parseResult.ConsumedTokens,
            "It did not consume the expected number of tokens."
        );

        var actual = ExpressionToAst.Convert(parseResult.Node);
        AssertAstsEqual(expected, actual);
    }
}
