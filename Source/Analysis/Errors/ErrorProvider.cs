using System.Diagnostics;
using System.Text;

namespace Krypton.Analysis.Errors
{
    public static class ErrorProvider
    {
        public static event ErrorEventHandler? Error;

        internal static void ReportError(Compilation compilation, ErrorCode errorCode, int lineNumber, int index, params string[] details)
        {
            int column = GetColumn(compilation.Code, index);
            string offendingLine = GetOffendingLine(compilation.Code, lineNumber, ref column);
            ErrorEventArgs e = new(errorCode, ErrorMessages.Messages[errorCode], details, offendingLine, compilation.Code, lineNumber, index, column);
            Error?.Invoke(e);
        }

        internal static int GetColumn(string str, int index)
        {
            for (int i = index; i >= 0; i--)
            {
                if (str[i] == '\n')
                {
                    return index - i;
                }
            }

            return index;
        }

        internal static string GetOffendingLine(string code, int lineNumber, ref int column)
        {
            string[] lines = code.Split('\n');
            string offendingLine = lines[lineNumber - 1];

            for (int i = 0; i < offendingLine.Length; i++)
            {
                if (!char.IsWhiteSpace(offendingLine[i]))
                {
                    column -= i;
                    return offendingLine.Trim();
                }
            }

            Debug.Fail(message: null);
            return null;
        }
    }
}
