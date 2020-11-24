using System.Collections.Immutable;

namespace Krypton.Analysis.Lexical
{
    public static class EscapeSequences
    {
        static EscapeSequences()
        {
            var builder = ImmutableDictionary.CreateBuilder<char, char>();
            builder.Add('a', '\a');
            builder.Add('f', '\f');
            builder.Add('n', '\n');
            builder.Add('r', '\r');
            builder.Add('s', '\b'); // b is used for binary char literals
            builder.Add('t', '\t');
            builder.Add('0', '\0');
            builder.Add('\'', '\'');
            builder.Add('"', '"');
            builder.Add('\\', '\\');
            EscapeCharacters = builder.ToImmutable();
        }

        public static ImmutableDictionary<char, char> EscapeCharacters { get; }

        public static bool TryParse(string input, out char output)
        {
            if (input.Length == 1)
            {
                output = input[0];
                return true;
            }
            else if (input.Length == 2)
            {
                if (EscapeCharacters.TryGetValue(input[1], out char escapeChar))
                {
                    output = escapeChar;
                    return true;
                }
                else
                {
                    output = input[1];
                    return false;
                }
            }
            else
            {
                switch (input[1])
                {
                    case 'd':
                        {
                            if (NumberLiteralParser.TryParseDecimal(input[2..], out long num))
                            {
                                output = (char)num;
                                return true;
                            }
                            else
                            {
                                output = '\0';
                                return true;
                            }
                        }
                    case 'x':
                        {
                            if (NumberLiteralParser.TryParseHexadecimal(input[2..], out long num))
                            {
                                output = (char)num;
                                return true;
                            }
                            else
                            {
                                output = '\0';
                                return true;
                            }
                        }
                    case 'b':
                        {
                            if (NumberLiteralParser.TryParseBinary(input[2..], out uint num))
                            {
                                output = (char)num;
                                return true;
                            }
                            else
                            {
                                output = '\0';
                                return true;
                            }
                        }
                    default:
                        output = default;
                        return false;
                }
            }
        }
    }
}
