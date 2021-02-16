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
            [ErrorCode.NewVariableInForWithoutDefaultValue] = "A variable declared in a For statement has to have " +
                                                              "an initial value",
            [ErrorCode.ForConditionHasToBeTrueOrComparisonWithIterationVariable] =
                "The condition after 'While' in a For loop has to be either the 'True' or a comparison with the " +
                "iteration variable as its left operand",
            [ErrorCode.ForWithPartHasToAssignIterationVariable] = "The 'With' part of a For loop has to assign " +
                                                                  "the iteration variable",
            [ErrorCode.ForNeitherWhileNorWith] = "The For statement neither has a 'While' nor a 'With' part while it " +
                                                 "has to have one or both of them",
            [ErrorCode.ExpectedOpenParenthesis] = "An open parenthesis '(' was expected",
            [ErrorCode.ExpectedKeywordAs] = "The keyword 'As' was expected",
            [ErrorCode.LetVariableAndConstMustBeInitialized] = "A variable declared with 'Let' and a constant " +
                                                               "must be immediately initialized",
            [ErrorCode.ConstantValueMustBeLiteralOrComplex] = "A constant must be initialized by a literal or " +
                                                              "a complex value in the form a + bi, e.g. '3 + 4i'",

            // Semantic errors: binding
            [ErrorCode.CantAssignUndeclaredVariable] = "This variable is not declared (at least not in scope)",
            [ErrorCode.CantRedeclareVariable] = "A variable with the same name is already declared in scope",
            [ErrorCode.ForNotVariable] = "The identifier does not refer to a local variable",
            [ErrorCode.CantFindIdentifierInScope] = "In this scope, this identifier is not declared",
            [ErrorCode.CantReAssignReadOnlyVariable] = "The variable can't be assigned again because it is read only",
            [ErrorCode.CantFindType] = "This type does not exist",
            [ErrorCode.LoopControlStatementNotThatDeep] = "There are not that many nested loops",
            [ErrorCode.PropertyDoesNotExistInType] = "This type does not have this property",

            // Semantic errors: types
            [ErrorCode.BinaryOperatorNotValidOnType] = "This binary operator cannot be used on these two types",
            [ErrorCode.UnaryOperatorNotValidOnType] = "This unary operator cannot be used on this type",
            [ErrorCode.CanOnlyCallFunctions] = "Only functions can be called",
            [ErrorCode.OnlyFunctionWithReturnTypeCanBeExpression] = "A function with no return type cannot be used as an expression",
            [ErrorCode.WrongNumberOfArguments] = "This function cannot be called with this number of arguments",
            [ErrorCode.CantConvertType] = "The source type cannot be converted to the target type",
            [ErrorCode.FunctionNotValidInContext] = "A function is not valid in the current context",
            [ErrorCode.ForIterationVariableHasToBeNumberType] = "The iteration variable of a For statement has to be one " +
                                                                "of the following types: Int, Rational, Complex",
            [ErrorCode.ReturnedValueEvenThoughFunctionDoesNotHaveReturnType] = "A value is returned from the function even though " +
                                                                               "it does not have a return type",
            [ErrorCode.ReturnedNoValueEvenThoughFunctionShouldReturn] = "The function is declared to return a value but this Return " +
                                                                        "statement doesn't return a value",
            [ErrorCode.ConstTypeHasToMatchLiteralTypeExactly] = "The type of the constant has to be exactly the type of the literal that is assigned to it",
            [ErrorCode.OperatorNotAvailableForTypes] = "This operator cannot be applied to two operands of those type",
        };

        public static ReadOnlyDictionary<ErrorCode, string> Messages => messages.MakeReadOnly();
    }
}
