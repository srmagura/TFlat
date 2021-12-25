using System.Text;
using System.Text.RegularExpressions;

namespace TFlat.Compiler.Lexer;

internal static class TheLexer
{
    public static List<Token> Lex(string s)
    {
        s = Preprocess(s);

        var position = 0;
        var tokens = new List<Token>();

        var line = 1;
        var lineStartPosition = 0;

        // Returns true if any whitespace consumed
        bool ConsumeWhitespace()
        {
            var anyConsumed = false;

            while (position < s.Length)
            {
                if (!char.IsWhiteSpace(s[position]))
                    break;

                if (s[position] == '\n')
                {
                    line++;
                    lineStartPosition = position + 1;
                }

                position++;
                anyConsumed = true;
            }

            return anyConsumed;
        }

        // Returns true if func returned a token
        bool TryLex(Func<string, int, SimpleToken?> func)
        {
            var simpleToken = func(s, position);
            if (simpleToken == null) return false;

            tokens.Add(
                new Token(
                    simpleToken.Type,
                    simpleToken.Value,
                    line,
                    position - lineStartPosition + 1
                )
            );
            position += simpleToken.Value.Length;
            return true;
        }

        while (position < s.Length)
        {
            if (ConsumeWhitespace()) continue;

            if (TryLex(LexIdentifier)) continue;
            if (TryLex(LexStringLiteral)) continue;

            throw new Exception("Failed to identify the next token.");
        }

        return tokens;
    }

    private static string Preprocess(string s)
    {
        return s.Replace("\r\n", "\n");
    }

    private static SimpleToken? LexIdentifier(string s, int position)
    {
        var sb = new StringBuilder();

        for (var i = position; i < s.Length; i++)
        {
            var isLetterOrUnderscore = char.IsLetter(s[i]) || s[i] == '_';

            if (i == 0 && !isLetterOrUnderscore) return null;
            if (!isLetterOrUnderscore && !char.IsDigit(s[i])) break;

            sb.Append(s[i]);
        }

        if (sb.Length == 0) return null;

        return new SimpleToken(TokenType.Identifier, sb.ToString());
    }

    //
    // LITERALS
    //

    private static SimpleToken? LexStringLiteral(string s, int position)
    {
        if (s[position] != '"') return null;

        var sb = new StringBuilder();

        for (var i = position + 1; i < s.Length; i++)
        {
            if (s[i] == '"')
            {
                return new SimpleToken(
                    TokenType.StringLiteral, 
                    s.Substring(position, i - position + 1)
                );
            }

            if (s[i] == '\n') return null;

            sb.Append(s[i]);
        }

        return null;
    }
}
