using TFlat.Compiler.Lexer;

namespace UnitTests.Lexer;

[TestClass]
public class LexerTests
{
    [TestMethod]
    public void ItConsumesWhitespace()
    {
        Assert.AreEqual(0, TheLexer.Lex("\n \t   \r\n").Length);
    }

    [TestMethod]
    public void ItLexesIdentifier()
    {
        var token = TheLexer.Lex("print").Single();
        Assert.AreEqual(new Token(TokenType.Identifier, "print", 1, 1), token);
    }

    [TestMethod]
    public void ItLexesIdentifierWithWhitespace()
    {
        var token = TheLexer.Lex("\n\t  print\r\n").Single();
        Assert.AreEqual(new Token(TokenType.Identifier, "print", 2, 4), token);
    }

    [TestMethod]
    public void ItLexesMultipleIdentifiers()
    {
        var tokens = TheLexer.Lex("a b");

        var expected = new List<Token>
        {
            new Token(TokenType.Identifier, "a", 1, 1),
            new Token(TokenType.Identifier, "b", 1, 3),
        };

        CollectionAssert.AreEqual(expected, tokens);
    }

    [TestMethod]
    public void ItLexesStringLiterals()
    {
        var token = TheLexer.Lex("\"\"").Single();
        Assert.AreEqual(new Token(TokenType.StringLiteral, "\"\"", 1, 1), token);

        token = TheLexer.Lex("\"foo\"").Single();
        Assert.AreEqual(new Token(TokenType.StringLiteral, "\"foo\"", 1, 1), token);

        token = TheLexer.Lex("\"foo bar{\"").Single();
        Assert.AreEqual(new Token(TokenType.StringLiteral, "\"foo bar{\"", 1, 1), token);
    }

    [TestMethod]
    public void ItThrowsOnUnterminatedStringLiterals()
    {
        Assert.ThrowsException<Exception>(() => TheLexer.Lex("\""));
        Assert.ThrowsException<Exception>(() => TheLexer.Lex("\"\n\""));
    }

    [TestMethod]
    public void ItLexesSeparators()
    {
        var token = TheLexer.Lex("(").Single();
        Assert.AreEqual(new Token(TokenType.OpenParen, "(", 1, 1), token);

        token = TheLexer.Lex(")").Single();
        Assert.AreEqual(new Token(TokenType.CloseParen, ")", 1, 1), token);

        token = TheLexer.Lex("{").Single();
        Assert.AreEqual(new Token(TokenType.OpenCurlyBrace, "{", 1, 1), token);

        token = TheLexer.Lex("}").Single();
        Assert.AreEqual(new Token(TokenType.CloseCurlyBrace, "}", 1, 1), token);

        token = TheLexer.Lex(":").Single();
        Assert.AreEqual(new Token(TokenType.Colon, ":", 1, 1), token);
    }

    [TestMethod]
    public void ItLexesKeywords()
    {
        var token = TheLexer.Lex("export").Single();
        Assert.AreEqual(new Token(TokenType.ExportKeyword, "export", 1, 1), token);

        token = TheLexer.Lex("fun").Single();
        Assert.AreEqual(new Token(TokenType.FunKeyword, "fun", 1, 1), token);

        token = TheLexer.Lex("void").Single();
        Assert.AreEqual(new Token(TokenType.VoidKeyword, "void", 1, 1), token);
    }

    private static SimpleToken ToSimpleToken(Token token)
    {
        return new SimpleToken(token.Type, token.Value);
    }

    [TestMethod]
    public void ItLexesHelloWorld()
    {
        var simpleTokens = TheLexer.Lex(CodeFixtures.HelloWorld)
            .Select(ToSimpleToken)
            .ToList();

        var expected = new List<SimpleToken>
        {
            new SimpleToken(TokenType.FunKeyword, "fun"),
            new SimpleToken(TokenType.Identifier, "main"),
            new SimpleToken(TokenType.OpenParen, "("),
            new SimpleToken(TokenType.CloseParen, ")"),
            new SimpleToken(TokenType.Colon, ":"),
            new SimpleToken(TokenType.VoidKeyword, "void"),
            new SimpleToken(TokenType.OpenCurlyBrace, "{"),
            new SimpleToken(TokenType.Identifier, "print"),
            new SimpleToken(TokenType.OpenParen, "("),
            new SimpleToken(TokenType.StringLiteral, "\"hello world\""),
            new SimpleToken(TokenType.CloseParen, ")"),
            new SimpleToken(TokenType.Semicolon, ";"),
            new SimpleToken(TokenType.CloseCurlyBrace, "}"),
        };

        CollectionAssert.AreEqual(expected, simpleTokens);
    }
}