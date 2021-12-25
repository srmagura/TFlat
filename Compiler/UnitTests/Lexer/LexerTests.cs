using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TFlat.Compiler.Lexer;

namespace UnitTests.Lexer;

[TestClass]
public class LexerTests
{
    [TestMethod]
    public void ItConsumesWhitespace()
    {
        Assert.AreEqual(0, TheLexer.Lex("\n \t   \r\n").Count);
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
        Assert.ThrowsException<Exception>(() => TheLexer.Lex("\"").Count);
        Assert.ThrowsException<Exception>(() => TheLexer.Lex("\"\n\"").Count);
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
}
