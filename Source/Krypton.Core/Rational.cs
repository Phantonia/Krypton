using System;

namespace Krypton.Core
{
    // When I have time I could add operators etc., but for now that's not necessary
    public struct Rational : IEquatable<Rational>
    {
        public static Rational Zero => default;

        public static Rational MaxValue => new(long.MaxValue, 1);

        public static Rational MinValue => new(long.MinValue, 1);

        public static Rational Epsilon => new(1, long.MaxValue);

        public static Rational NegativeEpsilon => new(-1, long.MaxValue);

        public Rational(long numerator, long denominator)
        {
            this.numerator = numerator;
            denominatorMinusOne = unchecked(denominator - 1);
            cancelled = false;
        }

        private bool cancelled;
        private long denominatorMinusOne; // actually denominator - 1, else default(Rational) would be 0/0
        private long numerator;

        public long Denominator
        {
            get
            {
                if (!cancelled)
                {
                    long gcd = Gcd(unchecked(denominatorMinusOne + 1), numerator);

                    denominatorMinusOne = unchecked((denominatorMinusOne + 1) / gcd - 1); // ???
                    //denominator /= gcd;
                    numerator /= gcd;

                    cancelled = true;
                }

                return unchecked(denominatorMinusOne + 1);
            }
        }

        public long Numerator
        {
            get
            {
                if (!cancelled)
                {
                    long gcd = Gcd(unchecked(denominatorMinusOne + 1), numerator);

                    denominatorMinusOne /= gcd;
                    numerator /= gcd;

                    cancelled = true;
                }

                return numerator;
            }
        }

        public override bool Equals(object? obj)
            => obj is Rational rational && Equals(rational);

        public bool Equals(Rational other)
        {
            long thisDenominator = unchecked(denominatorMinusOne + 1);
            long otherDenominator = unchecked(other.denominatorMinusOne + 1);

            return thisDenominator * other.numerator == numerator * otherDenominator;
        }

        public override int GetHashCode()
            => HashCode.Combine(numerator / unchecked(denominatorMinusOne + 1));

        public Rational Negate() => new(-numerator, unchecked(denominatorMinusOne + 1));

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        public static implicit operator Rational(long i) => new(i, 1);

        public static explicit operator long(Rational r)
            => r.numerator / unchecked(r.denominatorMinusOne + 1);

        public static explicit operator double(Rational r) => (double)r.Numerator / r.Denominator;

        public static bool operator ==(Rational left, Rational right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rational left, Rational right)
        {
            return !(left == right);
        }

        // "Inspired" by https://www.codeproject.com/tips/161824/fast-integer-algorithms-greatest-common-divisor-an
        private static long Gcd(long x, long y)
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