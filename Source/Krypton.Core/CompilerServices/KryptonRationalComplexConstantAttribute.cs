namespace Krypton.Core.CompilerServices
{
    public sealed class KryptonRationalComplexConstantAttribute : KryptonConstantAttribute
    {
        public KryptonRationalComplexConstantAttribute(long realNumerator,
                                                       long realDenominator,
                                                       long imaginaryNumerator,
                                                       long imaginaryDenominator)
            : base(ConstantType.RationalComplex)
        {
            Rational real = new(realNumerator, realDenominator);
            Rational imaginary = new(imaginaryNumerator, imaginaryDenominator);
            Value = new RationalComplex(real, imaginary);
        }

        public override object ObjectValue => Value;

        public RationalComplex Value { get; }
    }
}
