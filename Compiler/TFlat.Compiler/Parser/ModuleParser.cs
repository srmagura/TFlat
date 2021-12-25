using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser;

internal class ModuleParser
{
    internal static ParseResult<ModuleParseNode>? Parse(Token[] tokens)
    {
        var i = 0;

        // TODO support more than one function declaration
        var functionDeclarationResult = ParseFunctionDeclaration(tokens, i);
        if (functionDeclarationResult == null) return null;

        return new ParseResult<ModuleParseNode>(
            new ModuleParseNode(new[] { functionDeclarationResult.Node }),
            functionDeclarationResult.ConsumedTokens
        );
    }

    private static ParseResult<FunctionDeclarationParseNode>? ParseFunctionDeclaration(Token[] tokens, int position)
    {
        var i = position;

        var exported = tokens[i].Type == TokenType.ExportKeyword;
        if(exported) i++;

        if (tokens[i].Type != TokenType.FunKeyword) return null;
        i++;

        if (tokens[i].Type != TokenType.Identifier) return null;
        var name = tokens[i].Value;
        i++;

        if (tokens[i].Type != TokenType.OpenParen) return null;
        i++;

        if (tokens[i].Type != TokenType.CloseParen) return null;
        i++;

        if (tokens[i].Type != TokenType.Colon) return null;
        i++;

        if (tokens[i].Type != TokenType.VoidKeyword) return null;
        i++;

        if (tokens[i].Type != TokenType.OpenCurlyBrace) return null;
        i++;

        // TODO support multiple statements
        var statementParseResult = StatementParser.Parse(tokens, i);
        if (statementParseResult == null) return null;

        i += statementParseResult.ConsumedTokens;
        if (tokens[i].Type != TokenType.CloseCurlyBrace) return null;
        i++;

        var statements = new [] { statementParseResult.Node };

        return new ParseResult<FunctionDeclarationParseNode>(
            new FunctionDeclarationParseNode(name, exported, statements),
            consumedTokens: i - position
        );
    }
}
