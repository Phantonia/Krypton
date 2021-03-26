using System.Diagnostics;

namespace Krypton.CompilationData.Syntax.Tokens
{
    // Feel free to add members, but NEVER
    // change the values of existing members
    // (and don't overload a value).
    // When you add a member, please provide
    // an explicit initializer to prevent
    // changing implicit values. Thx ^^
    // Keep operators of the same precedence in
    // the same "tens".
    public enum Operator
    {
        None = 0,
        Tilde = 01, // ~
        NotKeyword = 02, // Not

        DoubleAsterisk = 11, // **

        Asterisk = 21, // *
        ForeSlash = 22, // /
        DivKeyword = 23, // Div
        ModKeyword = 24, // Mod

        Plus = 31, // +
        Minus = 32, // -

        Ampersand = 41, // &
        Caret = 42, // ^
        Pipe = 43, // |

        SingleRightArrow = 44, // ->
        SingleLeftArrow = 45, // <-

        LessThan = 51, // <
        LessThanEquals = 52, // <=
        GreaterThanEquals = 53, // >=
        GreaterThan = 54, // >

        DoubleEquals = 55, // ==
        ExclamationEquals = 56, // !=

        AndKeyword = 61, // And
        XorKeyword = 62, // Xor
        OrKeyword = 63, // Or
    }

    internal static class OperatorHelper
    {
        public static int GetPrecedence(this Operator @operator)
        {
            return int.MaxValue - (int)@operator / 10;
        }

        public static string ToText(this Operator @operator)
        {
            return @operator switch
            {
                Operator.Tilde => "~",
                Operator.DoubleAsterisk => "**",
                Operator.Asterisk => "*",
                Operator.ForeSlash => "/",
                Operator.DivKeyword => "Div",
                Operator.ModKeyword => "Mod",
                Operator.Plus => "+",
                Operator.Minus => "-",
                Operator.Ampersand => "&",
                Operator.Caret => "^",
                Operator.Pipe => "|",
                Operator.SingleRightArrow => "->",
                Operator.SingleLeftArrow => "<-",
                Operator.LessThan => "<",
                Operator.LessThanEquals => "<=",
                Operator.GreaterThanEquals => ">=",
                Operator.GreaterThan => ">",
                Operator.DoubleEquals => "==",
                Operator.ExclamationEquals => "!=",
                Operator.NotKeyword => "Not",
                Operator.AndKeyword => "And",
                Operator.XorKeyword => "Xor",
                Operator.OrKeyword => "Or",
                Operator.None => throw new System.NotImplementedException(),
                _ => OnFailure()
            };

            static string OnFailure()
            {
                Debug.Fail(message: null);
                return null;
            }
        }
    }
}
