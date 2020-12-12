namespace Krypton.Analysis.Grammatical
{
    public enum OperatorPrecedenceGroup
    {
        Depending,
        LogicalOr,
        LogicalXor,
        LogicalAnd,
        LogicalNot,
        Equality,
        Comparison,
        BitwiseShift,
        BitwiseOr,
        BitwiseXor,
        BitwiseAnd,
        Additive,
        Multiplicative,
        Exponantiation,
        BitwiseNotAndNegation
    }
}
