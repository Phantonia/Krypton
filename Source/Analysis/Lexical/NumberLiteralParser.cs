using System;

namespace Krypton.Analysis.Lexical
{
    public static class NumberLiteralParser
    {
        public static uint ParseBinary(string input)
        {
            if (TryParseBinary(input, out uint output))
            {
                return output;
            }
            else
            {
                throw new FormatException();
            }
        }

        public static bool TryParseBinary(string input, out uint output)
        {
            checked
            {
                int count = 0;
                uint result = 0;

                for (int i = input.Length - 1; i >= 0; i--)
                {
                    switch (input[i])
                    {
                        case '0':
                            break;
                        case '1':
                            {
                                uint subResult = unchecked((uint)Math.Pow(2, count) + result);
                                if (subResult < 0)
                                {
                                    output = default;
                                    return false;
                                }
                                result = subResult;
                            }
                            break;
                        case '_':
                            continue;
                        default:
                            output = default;
                            return false;
                    }

                    count++;
                }

                output = result;
                return true;
            }
        }

        public static long ParseDecimal(string input)
        {
            if (TryParseDecimal(input, out long output))
            {
                return output;
            }
            else
            {
                throw new FormatException();
            }
        }

        public static bool TryParseDecimal(string input, out long output)
        {
            checked
            {
                int count = 0;
                long result = 0;

                for (int i = input.Length - 1; i >= 0; i--)
                {
                    if (input[i] == '_')
                    {
                        continue;
                    }

                    if (!char.IsDigit(input[i]))
                    {
                        output = default;
                        return false;
                    }

                    long subResult = unchecked((long)Math.Pow(10, count) * (input[i] - '0') + result);
                    if (subResult < 0)
                    {
                        output = default;
                        return false;
                    }

                    result = subResult;
                    count++;
                }

                output = result;
                return true;
            }
        }

        public static long ParseHexadecimal(string input)
        {
            if (TryParseHexadecimal(input, out long output))
            {
                return output;
            }
            else
            {
                throw new FormatException();
            }
        }

        public static bool TryParseHexadecimal(string input, out long output)
        {
            checked
            {
                int count = 0;
                long result = 0;

                for (int i = input.Length - 1; i >= 0; i--)
                {
                    if (input[i] == '_')
                    {
                        continue;
                    }

                    long subResult;

                    unchecked
                    {
                        if (char.IsDigit(input[i]))
                        {
                            subResult = (long)Math.Pow(16, count) * (input[i] - '0') + result;
                        }
                        else if (input[i] >= 'a' & input[i] <= 'f')
                        {
                            subResult = (long)Math.Pow(16, count) * (input[i] - 'a' + 10) + result;
                        }
                        else if (input[i] >= 'A' & input[i] <= 'F')
                        {
                            subResult = (long)Math.Pow(16, count) * (input[i] - 'A' + 10) + result;
                        }
                        else
                        {
                            output = default;
                            return false;
                        }
                    }

                    if (subResult < 0)
                    {
                        output = default;
                        return false;
                    }

                    result = subResult;
                    count++;
                }

                output = result;
                return true;
            }
        }

        public static double ParseRational(string input)
        {
            if (TryParseRational(input, out double output))
            {
                return output;
            }
            else
            {
                throw new FormatException();
            }
        }

        public static bool TryParseRational(string input, out double output)
        {
            checked
            {
                int count = 0;
                long numberAfterPoint = 0;
                int afterPointLength = 0;
                long result = 0;

                for (int i = input.Length - 1; i >= 0; i--)
                {
                    if (input[i] == '_')
                    {
                        continue;
                    }

                    if (input[i] == '.')
                    {
                        numberAfterPoint = result;
                        result = 0;
                        afterPointLength = count;
                        count = 0;
                        continue;
                    }

                    if (!char.IsDigit(input[i]))
                    {
                        output = default;
                        return false;
                    }

                    long subResult = unchecked((long)Math.Pow(10, count) * (input[i] - '0') + result);
                    if (subResult < 0)
                    {
                        output = default;
                        return false;
                    }

                    result = subResult;
                    count++;
                }

                double decimalPart = Math.Pow(10, -afterPointLength) * numberAfterPoint;
                output = result + decimalPart;
                return true;
            }
        }
    }
}
