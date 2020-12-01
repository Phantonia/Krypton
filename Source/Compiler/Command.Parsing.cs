using Krypton.Analysis.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Compiler
{
    partial class Command
    {
        public static partial bool TryParse(string input, [NotNullWhen(true)] out Command? command)
        {
            int startIndex = 0;
            int index = 0;

            for (; index < input.Length; index++)
            {
                if (input[index] == ' ')
                {
                    string cmd = input[startIndex..index];

                    if (cmd.Equals("Compile", StringComparison.OrdinalIgnoreCase))
                    {
                        return TryParseCompileCommand(input, index + 1, out command);
                    }
                }
            }
        }

        private static string NextWord(string input, ref int index)
        {
            int startIndex = index;

            for (; index < input.Length; index++)
            {
                if (input[index] == ' ')
                {
                    string word = input[startIndex..index];
                    index++;
                    return word;
                }
            }
        }

        private static bool TryParseCompileCommand(string input, int index, [NotNullWhen(true)] out Command? command)
        {
            string? filePath = null;
            TargetLanguage language = TargetLanguage.None;

            int startIndex = index;

            while (true)
            {
                string nextWord = NextWord(input, ref index);

                if (filePath != null && nextWord.Equals("As", StringComparison.OrdinalIgnoreCase))
                {
                    if (!Enum.TryParse(NextWord(input, ref index), out language))
                    {
                        command = null;
                        return false;
                    }
                }
                else if (language != TargetLanguage.None && nextWord.Equals("To", StringComparison.OrdinalIgnoreCase))
                {
                    // TODO
                }
                else
                {
                    command = null;
                    return false;
                };
            }
        }

        private enum TargetLanguage
        {
            None,
            Js
        }
    }
}
