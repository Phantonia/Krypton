using System;

namespace Krypton.Analysis.Errors
{
    public delegate void ErrorEventHandler(ErrorEventArgs e);

    public sealed class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(ErrorCode errorCode, int lineNumber, string errorMessage, string[] details)
        {
            ErrorCode = errorCode;
            Message = errorMessage;
            LineNumber = lineNumber;
            Details = details;
        }

        public string[] Details { get; }

        public ErrorCode ErrorCode { get; }

        public string Message { get; }

        public int LineNumber { get; }
    }
}
