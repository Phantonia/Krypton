namespace Krypton.Analysis.Errors
{
    public static class ErrorProvider
    {
        public static event ErrorEventHandler? Error;

        public static void ReportError(ErrorCode errorCode, string errorMessage, int lineNumber)
        {
            Error?.Invoke(new ErrorEventArgs(errorCode, errorMessage, lineNumber));
        }

        public static void ReportMissingClosingParenthesis(string gotInstead, int lineNumber)
        {
            ReportError(ErrorCode.ExpectedClosingParenthesis, $"A closing parenthesis was expected. Instead there is \"{gotInstead}\"!", lineNumber);
        }

        public static void ReportMissingSemicolonError(string gotInstead, int lineNumber)
        {
            ReportError(ErrorCode.ExpectedSemicolon, $"A semicolon to end the statement was expected. Instead there is \"{gotInstead}\"!", lineNumber);
        }
    }
}
