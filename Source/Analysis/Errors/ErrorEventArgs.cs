using Krypton.Utilities;
using System;
using System.Diagnostics;
using System.Text;

namespace Krypton.Analysis.Errors
{
    public delegate void ErrorEventHandler(ErrorEventArgs e);

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public sealed class ErrorEventArgs : EventArgs
    {
        internal ErrorEventArgs(ErrorCode errorCode,
                                string message,
                                string[] details,
                                string offendingLine,
                                string entireCode,
                                int lineNumber,
                                int index,
                                int column)
        {
            ErrorCode = errorCode;
            Message = message;
            Details = details.MakeReadOnly();
            OffendingLine = offendingLine;
            EntireCode = entireCode;
            LineNumber = lineNumber;
            Index = index;
            Column = column;
        }

        public string EntireCode { get; }

        public int Column { get; }

        public ReadOnlyList<string> Details { get; }

        public ErrorCode ErrorCode { get; }

        public int Index { get; }

        public int LineNumber { get; }

        public string Message { get; }

        public string OffendingLine { get; }

        public string GetFullMessage()
        {
            StringBuilder sb = new();

            sb.Append("Error ")
              .Append((int)ErrorCode)
              .Append(" on line ")
              .Append(LineNumber)
              .Append(':')
              .AppendLine()
              .Append(Message)
              .AppendLine();

            foreach (string detail in Details)
            {
                sb.AppendLine(detail);
            }

            sb.AppendLine(OffendingLine)
              .Append(' ', repeatCount: Column - 1)
              .Append('\u25b2'); // ▲

            return sb.ToString();
        }

        private string GetDebuggerDisplay()
        {
            return $"Error {(int)ErrorCode} ({ErrorCode})";
        }
    }
}
