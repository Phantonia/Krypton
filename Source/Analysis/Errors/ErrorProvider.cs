using Krypton.Analysis.Ast;
using Krypton.Analysis.Lexical.Lexemes;
using System.Diagnostics;

namespace Krypton.Analysis.Errors
{
    public static class ErrorProvider
    {
        public static event ErrorEventHandler? Error;

        internal static void ReportError(ErrorCode errorCode, Compilation compilation, Node node, params string[] details)
        {
            ReportError(errorCode, compilation.Code, node.LineNumber, node.Index, details);
        }

        internal static void ReportError(ErrorCode errorCode, Compilation compilation, Lexeme lexeme, params string[] details)
        {
            ReportError(errorCode, compilation.Code, lexeme.LineNumber, lexeme.Index, details);
        }

        internal static void ReportError(ErrorCode errorCode, Compilation compilation, int lineNumber, int index, params string[] details)
        {
            ReportError(errorCode, compilation.Code, lineNumber, index, details);
        }

        internal static void ReportError(ErrorCode errorCode, string entireCode, Node node, params string[] details)
        {
            ReportError(errorCode, entireCode, node.LineNumber, node.Index, details);
        }

        internal static void ReportError(ErrorCode errorCode, string entireCode, Lexeme lexeme, params string[] details)
        {
            ReportError(errorCode, entireCode, lexeme.LineNumber, lexeme.Index, details);
        }

        internal static void ReportError(ErrorCode errorCode, string entireCode, int lineNumber, int index, params string[] details)
        {
            int column = GetColumn(entireCode, index);
            string offendingLine = GetOffendingLine(entireCode, lineNumber, ref column);
            ErrorEventArgs e = new(errorCode, ErrorMessages.Messages[errorCode], details, offendingLine, entireCode, lineNumber, index, column);
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
