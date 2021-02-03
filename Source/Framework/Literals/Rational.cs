using System;

namespace Krypton.Framework.Literals
{
    // When I have time I could add operators etc., but for now that's not necessary
    public struct Rational
    {
        public Rational(long numerator, long denominator)
        {
            this.numerator = numerator;
            this.denominator = unchecked(denominator - 1);
            cancelled = false;
        }

        private bool cancelled;
        private long denominator; // actually denominator - 1, else default(Rational) would be 0/0
        private long numerator;

        public long Denominator
        {
            get
            {
                if (!cancelled)
                {
                    long gcd = GCD(unchecked(denominator + 1), numerator);

                    denominator /= gcd;
                    numerator /= gcd;

                    cancelled = true;
                }

                return unchecked(denominator + 1);
            }
        }

        public long Numerator
        {
            get
            {
                if (!cancelled)
                {
                    long gcd = GCD(unchecked(denominator + 1), numerator);

                    denominator /= gcd;
                    numerator /= gcd;

                    cancelled = true;
                }

                return numerator;
            }
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        public static explicit operator double(Rational r) => (double)r.Numerator / r.Denominator;

        // "Inspired" by https://www.codeproject.com/tips/161824/fast-integer-algorithms-greatest-common-divisor-an
        private static long GCD(long x, long y)
        {
            x = Math.Abs(x);
            y = Math.Abs(y);

            long gcd = 1;

            if (x == y)
            {
                return x;
            }
           
            if (x > y && x % y == 0)
            {
                return y;
            }

            if (y > x && y % x == 0)
            {
                return x;
            }

            // Euclid algorithm to find GCD (a,b):
            // estimated maximum iterations: 
            // 5* (number of dec digits in smallest number)
            while (y != 0)
            {
                gcd = y;
                y = x % y;
                x = gcd;
            }

            return gcd;
        } 
    }
}