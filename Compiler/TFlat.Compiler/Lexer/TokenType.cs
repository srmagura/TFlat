namespace TFlat.Compiler.Lexer;

internal enum TokenType
{
    Identifier,

    // Literals
    StringLiteral,

    // Keywords
    ExportKeyword,
    FunKeyword,
    VoidKeyword,

    // Separators
    Semicolon,
    Colon,
    OpenParen,
    CloseParen,
    OpenCurlyBrace,
    CloseCurlyBrace,
}

