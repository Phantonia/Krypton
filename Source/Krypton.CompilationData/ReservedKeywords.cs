using System;
using System.Collections.Generic;
using System.Linq;

namespace Krypton.CompilationData
{
    public static class ReservedKeywords
    {
        internal const ReservedKeyword OperatorsEnd = (ReservedKeyword)100;
        internal const ReservedKeyword LiteralsEnd = (ReservedKeyword)200;
        internal const ReservedKeyword ModifiersEnd = (ReservedKeyword)300;

        static ReservedKeywords()
        {
            foreach ((string keyword, ReservedKeyword value) in Enum.GetNames<ReservedKeyword>()
                                                                    .Zip(Enum.GetValues<ReservedKeyword>()))
            {
                if (value != ReservedKeyword.NoKeyword)
                {
                    keywords[keyword] = value;
                }
            }
        }

        private static readonly Dictionary<string, ReservedKeyword> keywords = new();

        public static bool IsBooleanLiteralKeyword(ReservedKeyword keyword, out bool value)
        {
            switch (keyword)
            {
                case ReservedKeyword.True:
                    value = true;
                    return true;
                case ReservedKeyword.False:
                    value = false;
                    return true;
                default:
                    value = default;
                    return false;
            }
        }

        public static bool IsKeyword(ReadOnlySpan<char> keyword, out ReservedKeyword reservedKeyword)
        {
            string str = new(keyword);

            if (keywords.TryGetValue(str, out reservedKeyword))
            {
                return true;
            }

            reservedKeyword = default;
            return false;
        }

        public static bool IsLiteralKeyword(ReservedKeyword keyword)
        {
            return !IsOperatorKeyword(keyword, out _) && keyword < LiteralsEnd;
        }

        public static bool IsOperatorKeyword(ReadOnlySpan<char> keyword, out Operator @operator)
        {
            if (IsKeyword(keyword, out ReservedKeyword reservedKeyword))
            {
                return IsOperatorKeyword(reservedKeyword, out @operator);
            }

            @operator = default;
            return false;
        }

        public static bool IsOperatorKeyword(ReservedKeyword keyword, out Operator @operator)
        {
            @operator = (Operator)(int)keyword;
            return keyword < OperatorsEnd;
        }
    }
}
