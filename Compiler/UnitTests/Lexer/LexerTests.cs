using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TFlat.Compiler.Lexer;

namespace UnitTests.Lexer;

[TestClass]
public class LexerTests
{
    [TestMethod]
    public void LexIdentifier()
    {
        var token = TheLexer.Lex("print").Single();
        Assert.AreEqual(new Token(TokenType.Identifier, "print", 0), token);
    }
}
