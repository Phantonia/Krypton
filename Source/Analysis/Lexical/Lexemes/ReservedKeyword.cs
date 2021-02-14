using Krypton.Framework;

namespace Krypton.Analysis.Lexical.Lexemes
{
    // Except for NoKeyword these have to be the exact Krypton keyword.
    // Adding a new member will reserve new keywords.
    public enum ReservedKeyword
    {
        NoKeyword = 0, // this is not a Krypton keyword, this is the default value of the type ReservedKeyword

        // keywords that represent operators:
        // the constant here directly maps to the corresponding Krypton.Framework.Operator
        And = Operator.AndKeyword,
        Div = Operator.DivKeyword,
        Mod = Operator.ModKeyword,
        Not = Operator.NotKeyword,
        Or = Operator.OrKeyword,
        Xor = Operator.XorKeyword,

        // remaining keywords are counted upwards from 100
        As = 100,
        Block,
        Const,
        Continue,
        Else,
        For,
        Func,
        If,
        Leave,
        Let,
        Return,
        Var,
        While,
        With,
    }
}
