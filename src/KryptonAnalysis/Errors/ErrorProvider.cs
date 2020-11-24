namespace Krypton.Analysis.Errors
{
    public static class ErrorProvider
    {
        public static event ErrorEventHandler? Error;

        public static void ReportError(ErrorCode errorCode, string errorMessage, int lineNumber)
        {
            Error?.Invoke(new ErrorEventArgs(errorCode, errorMessage, lineNumber));
        }
    }
}
