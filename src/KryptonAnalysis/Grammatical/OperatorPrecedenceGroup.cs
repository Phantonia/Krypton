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
        RationalDivision,
        IntegerDivision,
        RealDivision,
        Modulo,
        Multiplication,
        Exponantiation,
        BitwiseNotAndNegation
    }
}
