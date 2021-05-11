namespace Krypton.Core
{
    // When I have time I could add operators etc., but for now that's not necessary
    public readonly struct RationalComplex
    {
        public RationalComplex(Rational real, Rational imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public Rational Imaginary { get; }

        public Rational Real { get; }
    }
}
