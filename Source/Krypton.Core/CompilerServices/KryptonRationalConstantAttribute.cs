namespace Krypton.Core.CompilerServices
{
    public sealed class KryptonRationalConstantAttribute : KryptonConstantAttribute
    {
        public KryptonRationalConstantAttribute(long numerator, long denominator) : base(ConstantType.Rational)
        {
            Value = new Rational(numerator, denominator);
        }

        public override object ObjectValue => Value;

        public Rational Value { get; }
    }
}
