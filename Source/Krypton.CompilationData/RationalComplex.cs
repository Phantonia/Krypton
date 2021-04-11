using System;

namespace Krypton.CompilationData
{
    public readonly struct RationalComplex : IEquatable<RationalComplex>
    {
        public RationalComplex(Rational real, Rational imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public Rational Imaginary { get; }

        public Rational Real { get; }

        public override bool Equals(object? obj)
            => obj is RationalComplex complex && Equals(complex);

        public bool Equals(RationalComplex other)
            => Real.Equals(other.Real) && Imaginary.Equals(other.Imaginary);

        public override int GetHashCode()
            => HashCode.Combine(Real, Imaginary);

        public static bool operator ==(RationalComplex left, RationalComplex right)
            => left.Equals(right);

        public static bool operator !=(RationalComplex left, RationalComplex right)
            => !(left == right);
    }
}
