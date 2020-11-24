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

        public static bool IsHex(this char chr)
        {
            return (chr >= '0' & chr <= '9')
                 | (chr >= 'a' & chr <= 'f')
                 | (chr >= 'A' & chr <= 'F');
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
