class Rational {
    private numerator: number;
    private denominator: number;
    private cancelled: boolean = false;

    public constructor(numerator: number, denominator: number) {
        this.numerator = numerator;
        this.denominator = denominator;
    }

    public static createFromFloatingPoint(float: number): Rational {
        const exponent = 10;

        let power = Math.pow(10, exponent);

        let newNumerator = float * power;
        let newDenominator = power;

        let gcd = Rational.gcd(newNumerator, newDenominator);

        newNumerator /= gcd;
        newDenominator /= gcd;

        return new Rational(newNumerator, newDenominator);
    }

    public getNumerator(): number {
        this.cancel();
        return this.numerator;
    }

    public getDenominator(): number {
        this.cancel();
        return this.denominator;
    }

    public exponentiate(right: Rational): Rational {
        let thisAsNumber = this.approximate();
        let rightAsNumber = right.approximate();
        let result = Math.pow(thisAsNumber, rightAsNumber);

        return Rational.createFromFloatingPoint(result);
    }

    public multiply(right: Rational): Rational {
        return new Rational(this.numerator * right.numerator, this.denominator * right.denominator);
    }

    public divide(right: Rational): Rational {
        return this.multiply(right.invert());
    }

    public mod(right: Rational): Rational {
        return Rational.createFromFloatingPoint(this.approximate() % right.approximate());
    }

    public add(right: Rational): Rational {
        let newNumerator = (this.numerator * right.denominator) + (right.numerator * this.denominator);
        let newDenominator = this.denominator * right.denominator;

        return new Rational(newNumerator, newDenominator);
    }

    public subtract(right: Rational): Rational {
        return this.add(right.negate());
    }

    public isLessThan(right: Rational): boolean {
        return this.approximate() < right.approximate();
    }

    public isLessThanOrEquals(right: Rational): boolean {
        return this.approximate() <= right.approximate();
    }

    public isGreaterThanOrEquals(right: Rational): boolean {
        return this.approximate() >= right.approximate();
    }

    public isGreaterThan(right: Rational): boolean {
        return this.approximate() > right.approximate();
    }

    public equals(right: Rational): boolean {
        return this.approximate() === right.approximate();
    }

    public notEquals(right: Rational): boolean {
        return this.approximate() !== right.approximate();
    }

    public negate(): Rational {
        return new Rational(-this.numerator, this.denominator);
    }

    public toString(): string {
        return `${this.getNumerator()}/${this.getDenominator()}`;
    }

    private cancel(): void {
        if (!this.cancelled) {
            let gcd = Rational.gcd(this.numerator, this.denominator);

            this.denominator /= gcd;
            this.numerator /= gcd;

            this.cancelled = true;
        }
    }

    private invert(): Rational {
        return new Rational(this.denominator, this.numerator);
    }

    private approximate(): number {
        return this.numerator / this.denominator;
    }

    private static gcd(x: number, y: number): number {
        x = Math.abs(x);
        y = Math.abs(y);

        if (x == y) {
            return x;
        }

        if (x > y && x % y == 0) {
            return y;
        }

        if (y > x && y % x == 0) {
            return x;
        }

        let gcd: number = 1;

        while (y != 0) {
            gcd = y;
            y = x % y;
            x = gcd;
        }

        return gcd;
    }
}

class Complex {
    private readonly real: Rational;
    private readonly imaginary: Rational;

    public constructor(real: Rational, imaginary: Rational) {
        this.real = real;
        this.imaginary = imaginary;
    }

    // **

    public multiply(right: Complex): Complex {
        // Multiplication:  (a + bi)(c + di) = (ac - bd) + (bc + ad)i
        let resultReal: Rational = this.real.multiply(right.real).subtract(this.imaginary.multiply(right.imaginary))
        let resultImag: Rational = this.real.multiply(right.imaginary).add(right.real.multiply(this.imaginary));
        return new Complex(resultReal, resultImag);
    }

    public divide(right: Complex): Complex {
        return new Complex(this.real.multiply(right))
    }
}