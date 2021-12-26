using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlat.Compiler.Parser;

internal static class ParseResultUtil
{
    internal static ParseResult<ParseNode> Generic<T>(ParseResult<T> result)
        where T : ParseNode
    {
        return new ParseResult<ParseNode>(result.Node, result.ConsumedTokens);
    }
}
