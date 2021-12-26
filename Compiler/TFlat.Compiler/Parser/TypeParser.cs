using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser;

internal static class TypeParser
{
    internal static ParseResult<TypeParseNode>? Parse(Token[] tokens, int position)
    {
        var i = position;

        if(tokens[i].Type == TokenType.StringKeyword)
        {
            return new ParseResult<TypeParseNode>(
                new TypeParseNode("string"),
                1
            );
        }

        return null;
    }
}
