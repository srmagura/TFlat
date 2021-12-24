using Microsoft.VisualStudio.TestTools.UnitTesting;
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
}
