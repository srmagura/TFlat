using System.Text;
using System.Text.RegularExpressions;

namespace TFlat.Compiler.Lexer;

internal static class TheLexer
{
    public static IReadOnlyList<Token> Lex(string s)
    {
        var position = 0;
        var tokens = new List<Token>();

        bool TryLex(Func<string, int, Token?> func)
        {
            var token = LexIdentifier(s, position);
            if (token == null) return false;

            tokens.Add(token);
            position += token.Value.Length;
            return true;
        }

        while (position < s.Length)
        {
            if (TryLex(LexIdentifier)) continue;
        }

        return tokens;
    }

    private static Token? LexIdentifier(string s, int position)
    {
        var sb = new StringBuilder();

        for (var i = position; i < s.Length; i++)
        {
            var isCharOrUnderscore = char.IsLetter(s[i]) || s[i] == '_';

            if (i == 0 && !isCharOrUnderscore) return null;
            if (!isCharOrUnderscore && !char.IsDigit(s[i])) return null;

            sb.Append(s[i]);
        }

        if (sb.Length == 0) return null;

        return new Token(TokenType.Identifier, sb.ToString(), position);
    }
}
