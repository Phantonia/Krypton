namespace Krypton.Framework
{
    public enum SpecialOperationGenerationKind
    {
        None = 0,
        IntPowerInt, // (new Rational({exp}, 1).exponentiate(new Rational({exp}, 1)
        IntegerDivision, // Math.floor({exp} / {exp})
        IntToRationalDivision, // new Rational({exp}, {exp})
    }
}
