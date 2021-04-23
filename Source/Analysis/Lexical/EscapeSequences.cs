﻿using Krypton.Utilities;
using System;
using System.Collections.Generic;

namespace Krypton.Analysis.Lexical
{
    internal static class EscapeSequences
    {
        static EscapeSequences()
        {
            Dictionary<char, char> dict = new()
            {
                { 'a', '\a' },
                { 'f', '\f' },
                { 'n', '\n' },
                { 'r', '\r' },
                { 'b', '\b' },
                { 't', '\t' },
                { '0', '\0' },
                { '\'', '\'' },
                { '"', '"' },
                { '\\', '\\' }
            };
            EscapeCharacters = dict.Finalize();
        }

        public static FinalDictionary<char, char> EscapeCharacters { get; }

        public static char Parse(ReadOnlySpan<char> input)
        {
            if (TryParse(input, out char output))
            {
                return output;
            }
            else
            {
                throw new FormatException();
            }
        }

        public static bool TryParse(ReadOnlySpan<char> input, out char output)
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
                    case 'u':
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
                    default:
                        output = default;
                        return false;
                }
            }
        }
    }
}
