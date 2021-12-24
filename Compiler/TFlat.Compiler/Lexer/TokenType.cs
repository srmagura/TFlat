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
    Colon,
    OpenParen,
    CloseParen,
}

