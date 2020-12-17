using Krypton.Analysis.Lexical.Lexemes;

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

        public static void ReportUnexpectedExpressionTerm(Lexeme lexeme)
        {
            ReportError(ErrorCode.UnexpectedExpressionTerm, $"Unexpected expression term {lexeme.Content}", lexeme.LineNumber);
        }

        public static void ReportMissingCommaOrParenthesis(string gotInstead, int lineNumber)
        {
            ReportError(ErrorCode.ExpectedCommaOrClosingParenthesis, $"A comma or a closing parenthesis was expected. Instead there is \"{gotInstead}\"!", lineNumber);
        }

        public static void ReportMissingIdentifier(string gotInstead, int lineNumber)
        {
            ReportError(ErrorCode.ExpectedIdentifier, $"An identifier was expected. Instead there is \"{gotInstead}\"!", lineNumber);
        }

        public static void ReportMissingEqualsOrSemicolon(string gotInstead, int lineNumber)
        {
            ReportError(ErrorCode.ExpectedEqualsOrSemicolon, $"An equals or a semicolon is expected. Instead there is \"{gotInstead}\"!", lineNumber);
        }
    }
}
