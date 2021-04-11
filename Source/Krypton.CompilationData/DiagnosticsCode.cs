namespace Krypton.CompilationData
{
    public enum DiagnosticsCode
    {
        None = 0,

        // Lexical errors
        UnknownToken = 1,
        UnclosedStringLiteral = 2,
        UnclosedCharLiteral = 3,
        EscapeSequenceError = 4,
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
        ExpectedOpenParenthesis = 116,
        ExpectedKeywordAs = 117,
        LetVariableAndConstMustBeInitialized = 118,

        // Semantic errors: binding
        CantAssignUndeclaredVariable = 401,
        CantRedeclareVariable = 402,
        ForNotVariable = 403,
        CantFindIdentifierInScope = 404, // 404, get it? This error code must stay
        CantReAssignReadOnlyVariable = 405,
        ConstantValueMustBeLiteralOrComplex = 406,
        CantFindType = 407,
        LoopControlStatementNotThatDeep = 408,
        PropertyDoesNotExistInType = 409,
        DuplicateParameter = 410,
        CantRedeclareGlobalSymbol = 411,

        // Semantic errors: types
        BinaryOperatorNotValidOnType = 601,
        UnaryOperatorNotValidOnType = 602,
        CanOnlyCallFunctions = 603,
        OnlyFunctionWithReturnTypeCanBeExpression = 604,
        WrongNumberOfArguments = 605,
        CantConvertType = 606,
        FunctionNotValidInContext = 607,
        ForIterationVariableHasToBeNumberType = 608,
        ReturnedValueEvenThoughFunctionDoesNotHaveReturnType = 609,
        ReturnedNoValueEvenThoughFunctionShouldReturn = 610,
        ConstTypeHasToMatchLiteralTypeExactly = 611,
        OperatorNotAvailableForTypes = 612,
    }
}
