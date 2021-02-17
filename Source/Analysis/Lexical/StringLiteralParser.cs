using Krypton.Utilities;
using System.Text;

namespace Krypton.Analysis.Lexical
{
    internal static class StringLiteralParser
    {
        public static bool TryParse(string input, out string output)
        {
            StringBuilder sb = new(capacity: input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != '\\')
                {
                    sb.Append(input[i]);
                }
                else if (EscapeSequences.EscapeCharacters.TryGetValue(input[i + 1], out char escapeCharacter))
                {
                    i++;
                    sb.Append(escapeCharacter);
                }
                else if (input.TryGet(i + 1) == 'u')
                {
                    i += 2;

                    if (input.TryGet(i + 3) == null)
                    {
                        output = string.Empty;
                        return false;
                    }

                    string numberString = input[i..(i + 4)];
                    if (numberString.Contains('_'))
                    {
                        output = string.Empty;
                        return false;
                    }

                    if (!NumberLiteralParser.TryParseHexadecimal(input[i..(i + 4)], out long number))
                    {
                        output = string.Empty;
                        return false;
                    }

                    if (number > char.MaxValue || number < char.MinValue)
                    {
                        output = string.Empty;
                        return false;
                    }

                    sb.Append((char)number);

                    i += 3;
                }
                else
                {
                    output = string.Empty;
                    return false;
                }
            }

            output = sb.ToString();
            return true;
        }
    }
}
