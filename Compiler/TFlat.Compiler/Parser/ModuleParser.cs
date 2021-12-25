using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser;

internal class ModuleParser
{
    internal static ModuleParseNode Parse(Token[] tokens)
    {
        var i = 0;
        var functionDeclarations = new List<FunctionDeclarationParseNode>();

        while (i < tokens.Length)
        {
            var functionDeclarationResult = ParseFunctionDeclaration(tokens, i);
            if (functionDeclarationResult != null)
            {
                functionDeclarations.Add(functionDeclarationResult.Node);
                i += functionDeclarationResult.ConsumedTokens;
                continue;
            }

            throw new Exception("Parsing failure.");
        }

        return new ModuleParseNode(functionDeclarations.ToArray());
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

        var statements = new List<StatementParseNode>();

        while(i < tokens.Length)
        {
            if (tokens[i].Type == TokenType.CloseCurlyBrace)
            {
                i++;

                return new ParseResult<FunctionDeclarationParseNode>(
                    new FunctionDeclarationParseNode(name, exported, statements.ToArray()),
                    consumedTokens: i - position
                );
            }

            var statementParseResult = StatementParser.Parse(tokens, i);
            if (statementParseResult == null) return null;

            statements.Add(statementParseResult.Node);
            i += statementParseResult.ConsumedTokens;
        }

        return null;
    }
}
