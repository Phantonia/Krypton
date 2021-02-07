using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Errors
{
    public static class ErrorMessages
    {
        private static readonly Dictionary<ErrorCode, string> messages = new()
        {
            // Lexical errors
            [ErrorCode.UnknownLexeme] = "Unexpected character",
            [ErrorCode.UnclosedStringLiteral] = "String literal is not closed on the same line",
            [ErrorCode.UnclosedCharLiteral] = "Character literal is not closed on the same line",
            [ErrorCode.HexLiteralWithMixedCase] = "Hexadecimal literals can not have letters a-f " +
                                                  "that are sometimes uppercase and sometimes lowercase",

            // Syntax errors
            [ErrorCode.ExpectedSemicolon] = "This statement has to be ended by a semicolon ';'",
            [ErrorCode.ExpectedClosingParenthesis] = "A closing parenthesis ')' was expected",
            [ErrorCode.UnexpectedExpressionTerm] = "This token is not legal at this part of an expression",
            [ErrorCode.ExpectedCommaOrClosingParenthesis] = "A comma ',' for the next parameter or a closing " +
                                                            "parenthesis ')' to close the function call was expected",
            [ErrorCode.ExpectedIdentifier] = "An identifier was expected",
            [ErrorCode.ExpectedEqualsOrSemicolon] = "An equals '=' to specify this variable's initial value or a " +
                                                    "semicolon ';' to end the variable declaration was expected",
            [ErrorCode.ExpectedClosingBrace] = "Expected a closing brace '}'",
            [ErrorCode.ExpectedOpeningBrace] = "Expected an opening brace '{'",
            [ErrorCode.ExpectedExpressionTerm] = "An expression term was expected",
            [ErrorCode.OnlyFunctionCallExpressionCanBeStatement] = "This kind of expression is not a legal statement",
            [ErrorCode.ExpectedAsOrEquals] = "Expected the keyword 'As' to specify the type or an equals '=' to " +
                                             "specify the initial value",

            // Semantic errors: binding
            [ErrorCode.CantAssignUndeclaredVariable] = "This variable is not declared (at least not in scope)",
            [ErrorCode.CantRedeclareVariable] = "A variable with the same name is already declared in scope",

            // Semantic errors: types
            [ErrorCode.BinaryOperatorNotValidOnType] = "This binary operator cannot be used on these two types",
            [ErrorCode.UnaryOperatorNotValidOnType] = "This unary operator cannot be used on this type",
            [ErrorCode.CanOnlyCallFunctions] = "Only functions can be called",
            [ErrorCode.OnlyFunctionWithReturnTypeCanBeExpression] = "A function with no return type cannot be used as an expression",
            [ErrorCode.WrongNumberOfArguments] = "This function cannot be called with this number of arguments",
            [ErrorCode.CantConvertType] = "The source type cannot be converted to the target type",
        };

        public static ReadOnlyDictionary<ErrorCode, string> Messages => messages.MakeReadOnly();
    }
}
