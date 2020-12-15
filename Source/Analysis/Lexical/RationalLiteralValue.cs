using System;
using System.Globalization;

namespace Krypton.Analysis.Lexical
{
    // This struct only represents rational literals in the form a * 10^(-n).
    // For example: 3.14159 is represented as 314159 * 10^(-5).
    // It does not represent any rational number as handled by Krypton.
    public readonly struct RationalLiteralValue : IEquatable<RationalLiteralValue>
    {

        public RationalLiteralValue(long numerator, uint power)
        {
            this.numerator = numerator;
            this.power = power;
        }

        private readonly long numerator;

        private readonly uint power;

        public long Numerator
        {
            get
            {
                (long numerator, _) = Cancel();
                return numerator;
            }
        }

        public uint Power
        {
            get
            {
                (_, uint power) = Cancel();
                return power;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is RationalLiteralValue other && Equals(other);
        }

        public bool Equals(RationalLiteralValue other)
        {
            (long numerator1, uint power1) = Cancel();
            (long numerator2, uint power2) = other.Cancel();

            return numerator1 == numerator2 & power1 == power2;
        }

        public override int GetHashCode()
        {
            (long numerator, uint power) = Cancel();
            return HashCode.Combine(numerator, power);
        }

        public override string ToString()
        {
            return ((double)this).ToString(CultureInfo.InvariantCulture);
        }

        public static explicit operator double(RationalLiteralValue value)
        {
            return value.Numerator / Math.Pow(10, value.Power);
        }

        public static bool operator ==(RationalLiteralValue left, RationalLiteralValue right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RationalLiteralValue left, RationalLiteralValue right)
        {
            return !(left == right);
        }

        private (long, uint) Cancel()
        {
            long numerator = this.numerator;
            uint power = this.power;

            while (numerator % 10 == 0)
            {
                numerator /= 10;
                power--;
            }

            return (numerator, power);
        }
    }
}
