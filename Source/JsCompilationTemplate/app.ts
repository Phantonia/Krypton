/* This is the template for Krypton code that is compiled to javascript.
 * For that purpose, this typescript code is compiled to javascript and then minified.
 * The tests at the bottom are removed.
 */

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

        let newNumerator = Math.floor(float * power);
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
        let newNumerator = Math.pow(Math.pow(this.numerator, right.numerator), 1 / right.denominator);
        let newDenominator = Math.pow(Math.pow(this.denominator, right.numerator), 1 / right.denominator);

        if (Number.isInteger(newNumerator) && Number.isInteger(newDenominator)) {
            return new Rational(newNumerator, newDenominator);
        }

        return Rational.createFromFloatingPoint(newNumerator / newDenominator);
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
        return this.approximate().toString();
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

    public approximate(): number {
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

    public getMagnitude(): Rational {
        let r1 = this.real;
        let r2 = this.imaginary;

        // m = sqrt(r1**2 + r2**2) = (r1**2 + r2**2)**(1/2)
        return r1.exponentiate(new Rational(2, 1))
            .add(r2.exponentiate(new Rational(2, 1)))
            .exponentiate(new Rational(1, 2));
    }

    public exponentiate(right: Complex): Complex {
        let rho = this.getMagnitude();
        let theta = callWithRational2(Math.atan2, this.imaginary, this.real);
        let newRho = right.real.multiply(theta).add(right.imaginary.multiply(callWithRational1(Math.log, rho)));
        let t = rho.exponentiate(right.real).multiply(Rational.createFromFloatingPoint(Math.E).exponentiate(right.imaginary.negate().multiply(theta)));
        return new Complex(t.multiply(callWithRational1(Math.cos, newRho)), t.multiply(callWithRational1(Math.sin, newRho)));
    }

    public multiply(right: Complex): Complex {
        // Multiplication:  (a + bi)(c + di) = (ac - bd) + (bc + ad)i
        let resultReal: Rational = this.real.multiply(right.real).subtract(this.imaginary.multiply(right.imaginary))
        let resultImag: Rational = this.real.multiply(right.imaginary).add(right.real.multiply(this.imaginary));
        return new Complex(resultReal, resultImag);
    }

    public divide(right: Complex): Complex {
        let a = this.real;
        let b = this.imaginary;
        let c = right.real;
        let d = right.imaginary;

        if (abs(d).isLessThan(abs(c))) {
            let doc = d.divide(c);
            let resultReal = a.add(b.multiply(doc)).divide(c.add(d.multiply(doc)));
            let resultImag = b.subtract(a.multiply(doc)).divide(c.add(d.multiply(doc)));
            return new Complex(resultReal, resultImag);
        } else {
            let cod = c.divide(d);
            let resultReal = b.add(a.multiply(cod)).divide(d.add(c.multiply(cod)));
            let resultImag = a.negate().add(b.multiply(cod)).divide(d.add(c.multiply(cod)));
            return new Complex(resultReal, resultImag);
        }
    }

    public add(right: Complex): Complex {
        let resultReal = this.real.add(right.real);
        let resultImag = this.imaginary.add(right.imaginary);
        return new Complex(resultReal, resultImag);
    }

    public subtract(right: Complex): Complex {
        return this.add(right.negate());
    }

    public equals(right: Complex): boolean {
        return this.real.equals(right.real)
            && this.imaginary.equals(right.imaginary);
    }

    public notEquals(right: Complex): boolean {
        return !this.equals(right);
    }

    public negate(): Complex {
        return new Complex(this.real.negate(), this.imaginary.negate());
    }

    public toString(): string {
        return `${this.real.toString()} + ${this.imaginary.toString()}i`;
    }
}

function callWithRational1(f: (n: number) => number, r: Rational) {
    return Rational.createFromFloatingPoint(f(r.approximate()));
}

function callWithRational2(f: (x: number, y: number) => number, x: Rational, y: Rational) {
    return Rational.createFromFloatingPoint(f(x.approximate(), y.approximate()));
}

function abs(num: Rational): Rational {
    if (num.getNumerator() < 0) {
        return num.negate();
    }

    return num;
}

// CUT HERE ... everything from here on will be cut in the final template
// Complex test *
{
    let r1 = new Rational(1, 1);
    let i1 = new Rational(2, 1);
    let r2 = new Rational(4, 1);
    let i2 = new Rational(3, 1);
    let z1 = new Complex(r1, i1);
    let z2 = new Complex(r2, i2);
    console.log(z1.multiply(z2).toString());
}

// Complex test ** (does not work)
{
    let r1 = new Rational(1, 1);
    let i1 = new Rational(2, 1);
    let r2 = new Rational(4, 1);
    let i2 = new Rational(3, 1);
    let z1 = new Complex(r1, i1);
    let z2 = new Complex(r2, i2);
    console.log(z1.exponentiate(z2).toString());
}

// Complex test /
{
    let r1 = new Rational(1, 1);
    let i1 = new Rational(2, 1);
    let r2 = new Rational(4, 1);
    let i2 = new Rational(3, 1);
    let z1 = new Complex(r1, i1);
    let z2 = new Complex(r2, i2);
    console.log(z1.divide(z2).toString());
}

// Complex test +
{
    let r1 = new Rational(1, 1);
    let i1 = new Rational(2, 1);
    let r2 = new Rational(4, 1);
    let i2 = new Rational(3, 1);
    let z1 = new Complex(r1, i1);
    let z2 = new Complex(r2, i2);
    console.log(z1.add(z2).toString());
}

// Complex test -
{
    let r1 = new Rational(1, 1);
    let i1 = new Rational(2, 1);
    let r2 = new Rational(4, 1);
    let i2 = new Rational(3, 1);
    let z1 = new Complex(r1, i1);
    let z2 = new Complex(r2, i2);
    console.log(z1.subtract(z2).toString());
}

// Complex test magnitude
{
    let r2 = new Rational(4, 1);
    let i2 = new Rational(3, 1);
    let z2 = new Complex(r2, i2);
    console.log(z2.getMagnitude().toString());
}

""; // used to break in visual studio