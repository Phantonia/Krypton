using System;

namespace Krypton.Analysis.Errors
{
    public sealed class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(ErrorCode errorCode, string errorMessage, int lineNumber)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            LineNumber = lineNumber;
        }

        public ErrorCode ErrorCode { get; }

        public string ErrorMessage { get; }

        public int LineNumber { get; }
    }
}
