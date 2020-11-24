using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using System.Text;

namespace Krypton.Analysis.Utilities
{
    public static class Extensions
    {
        public static char? TryGet(this string str, int index)
        {
            return str.Length > index ? str[index] : (char?)null;
        }

        public static bool IsHex(this char chr, out bool? isUpper)
        {
            if (chr >= '0' & chr <= '9')
            {
                isUpper = null;
                return true;
            }

            if (chr >= 'a' & chr <= 'f')
            {
                isUpper = false;
                return true;
            }

            if (chr >= 'A' & chr <= 'F')
            {
                isUpper = true;
                return true;
            }

            isUpper = null;
            return false;
        }

        public static bool IsBinary(this char chr)
        {
            return chr == '0' | chr == '1';
        }

        public static bool IsLetterOrUnderscore(this char chr)
        {
            return (chr >= 'a' & chr <= 'z')
                 | (chr >= 'A' & chr <= 'Z')
                 | (chr == '_');
        }
    }
}
