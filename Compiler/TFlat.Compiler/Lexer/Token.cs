namespace TFlat.Compiler.Lexer;

internal record Token(
    TokenType Type,
    string Value,
    int StartPosition
);
