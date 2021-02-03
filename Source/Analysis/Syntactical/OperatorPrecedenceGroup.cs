namespace Krypton.Analysis.Syntactical
{
    // The order matters. If you need to add a new group,
    // do it so that order is preserved and (if possible)
    // no group changes its number.
    public enum OperatorPrecedenceGroup
    {
        None = 0,
        LogicalOr = 10,
        LogicalXor = 20,
        LogicalAnd = 30,
        LogicalNot = 40,
        Equality = 50,
        Comparison = 60,
        Shift = 70,
        Bitwise = 80,
        Additive = 90,
        Multiplicative = 100,
        Exponantiation = 110,
        BitwiseNotAndNegation = 120
    }
}
