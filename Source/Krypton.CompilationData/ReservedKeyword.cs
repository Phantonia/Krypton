namespace Krypton.CompilationData
{
    public enum ReservedKeyword
    {
        NoKeyword = 0, // this is not a Krypton keyword, this is the default value of the type ReservedKeyword

        // keywords that represent operators:
        // the constant here directly maps to the corresponding Krypton.CompilationData.Operator
        And = Operator.AndKeyword,
        Div = Operator.DivKeyword,
        Mod = Operator.ModKeyword,
        Not = Operator.NotKeyword,
        Or = Operator.OrKeyword,
        Xor = Operator.XorKeyword,

        False = ReservedKeywords.OperatorsEnd,
        True,

        As = ReservedKeywords.ModifiersEnd,
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
        To,
        Var,
        While,
        With,
    }
}
