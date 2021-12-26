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
    SingleEqual
}

