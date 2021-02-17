using System.Text;

namespace Krypton.Analysis.Lexical
{
    internal static class StringLiteralParser
    {
        public static bool TryParse(string input, out string output)
        {
            StringBuilder sb = new StringBuilder();

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
                else
                {
                    for (int j = i; j <= input.Length; j++)
                    {
                        if (j == input.Length)
                        {
                            output = string.Empty;
                            return false;
                        }

                        if (input[j] == ';')
                        {
                            if (EscapeSequences.TryParse(input[i..j], out escapeCharacter))
                            {
                                sb.Append(escapeCharacter);
                                i = j;
                                break;
                            }
                            else
                            {
                                output = string.Empty;
                                return false;
                            }
                        }
                    }
                }
            }

            output = sb.ToString();
            return true;
        }
    }
}
