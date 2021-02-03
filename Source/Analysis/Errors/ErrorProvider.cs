namespace Krypton.Analysis.Errors
{
    public static class ErrorProvider
    {
        public static event ErrorEventHandler? Error;

        internal static void ReportError(ErrorCode errorCode, int lineNumber, params string[] details)
        {
            Error?.Invoke(new ErrorEventArgs(errorCode, lineNumber, ErrorMessages.Messages[errorCode], details));
        }
    }
}
