namespace TFlat.Compiler.Lexer;

internal record SimpleToken(TokenType Type, string Value);

internal record Token(TokenType Type, string Value, int Line, int Column);
