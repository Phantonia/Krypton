namespace Krypton.Framework
{
    public enum SpecialCodeGenerationKind
    {
        None = 0,
        IntPowerInt, // (new Rational({exp}, 1).exponentiate(new Rational({exp}, 1)
        IntegerDivision, // Math.floor({exp} / {exp})
        IntToRationalDivision, // new Rational({exp}, {exp})
        IntToRational, // new Rational({exp},1)
        IntToComplex, // new Complex(new Rational({exp},1),new Rational(1,0))
        RationalToComplex, // new Complex({exp},new Rational(1,0))
        GetLengthProp, // ({exp}).length
        ConsoleLog, // console.log({exp})
    }
}
