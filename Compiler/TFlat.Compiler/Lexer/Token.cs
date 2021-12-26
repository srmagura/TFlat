namespace TFlat.Compiler.Lexer;

internal enum TokenType
{
    Identifier,

    // Literals
    IntLiteral,
    StringLiteral,

    // Keywords
    ExportKeyword,
    FunKeyword,
    LetKeyword,
    ConstKeyword,

    VoidKeyword,
    IntKeyword,
    StringKeyword,
    BoolKeyword,

    // Separators
    Semicolon,
    Colon,
    OpenParen,
    CloseParen,
    OpenCurlyBrace,
    CloseCurlyBrace,

    // Operators
    Plus,
    Minus,
    Asterisk,
    DoubleAsterisk,
    Slash,
    DoubleBackslash,
    SingleEqual
}

internal record SimpleToken(TokenType Type, string Value);

internal record Token(TokenType Type, string Value, int Line, int Column);
