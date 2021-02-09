namespace Krypton.Analysis.Errors
{
    public enum ErrorCode
    {
        None = 0,

        // Lexical errors
        UnknownLexeme = 1,
        UnclosedStringLiteral = 2,
        UnclosedCharLiteral = 3,
        UnknownEscapeSequence = 4,
        HexLiteralWithMixedCase = 5,

        // Syntax errors
        ExpectedSemicolon = 101,
        ExpectedClosingParenthesis = 102,
        UnexpectedExpressionTerm = 103,
        ExpectedCommaOrClosingParenthesis = 104,
        ExpectedIdentifier = 105,
        ExpectedEqualsOrSemicolon = 106,
        ExpectedClosingBrace = 107,
        ExpectedOpeningBrace = 108,
        ExpectedExpressionTerm = 109,
        OnlyFunctionCallExpressionCanBeStatement = 110,
        ExpectedAsOrEquals = 111,
        NewVariableInForWithoutDefaultValue = 112,
        ForConditionHasToBeTrueOrComparisonWithIterationVariable = 113,
        ForWithPartHasToAssignIterationVariable = 114,
        ForNeitherWhileNorWith = 115,

        // Semantic errors: binding
        CantAssignUndeclaredVariable = 401,
        CantRedeclareVariable = 402,

        // Semantic errors: types
        BinaryOperatorNotValidOnType = 601,
        UnaryOperatorNotValidOnType = 602,
        CanOnlyCallFunctions = 603,
        OnlyFunctionWithReturnTypeCanBeExpression = 604,
        WrongNumberOfArguments = 605,
        CantConvertType = 606,

    }
}
