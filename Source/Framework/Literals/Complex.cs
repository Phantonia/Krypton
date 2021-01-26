namespace Krypton.Framework.Literals
{
    // When I have time I could add operators etc., but for now that's not necessary
    public readonly struct Complex
    {
        public Complex(Rational real, Rational imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public Rational Imaginary { get; }

        public Rational Real { get; }
    }
}
