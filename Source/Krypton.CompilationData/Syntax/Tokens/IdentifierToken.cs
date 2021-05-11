using System;
using System.Diagnostics;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class IdentifierToken : Token
    {
        public IdentifierToken(ReadOnlyMemory<char> identifier,
                               int lineNumber,
                               Trivia leadingTrivia)
            : base(identifier, lineNumber, leadingTrivia)
        {
#if DEBUG
            Debug.Assert(identifier.Span[0] is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or '_');

            foreach (char c in identifier.Span[1..])
            {
                Debug.Assert(c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or (>= '0' and <= '9') or '_');
            }
#endif
        }
    }
}
