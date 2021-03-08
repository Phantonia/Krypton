/* This is the template for Krypton code that is compiled to javascript.
 * For that purpose, this typescript code is compiled to javascript and then minified.
 * The tests at the bottom are removed.
 */
class Rational {
    constructor(numerator, denominator) {
        this.cancelled = false;
        this.numerator = numerator;
        this.denominator = denominator;
    }
    static createFromFloatingPoint(float) {
        const exponent = 10;
        let power = Math.pow(10, exponent);
        let newNumerator = Math.floor(float * power);
        let newDenominator = power;
        let gcd = Rational.gcd(newNumerator, newDenominator);
        newNumerator /= gcd;
        newDenominator /= gcd;
        return new Rational(newNumerator, newDenominator);
    }
    getNumerator() {
        this.cancel();
        return this.numerator;
    }
    getDenominator() {
        this.cancel();
        return this.denominator;
    }
    exponentiate(right) {
        let newNumerator = Math.pow(Math.pow(this.numerator, right.numerator), 1 / right.denominator);
        let newDenominator = Math.pow(Math.pow(this.denominator, right.numerator), 1 / right.denominator);
        if (Number.isInteger(newNumerator) && Number.isInteger(newDenominator)) {
            return new Rational(newNumerator, newDenominator);
        }
        return Rational.createFromFloatingPoint(newNumerator / newDenominator);
    }
    multiply(right) {
        return new Rational(this.numerator * right.numerator, this.denominator * right.denominator);
    }
    divide(right) {
        return this.multiply(right.invert());
    }
    mod(right) {
        return Rational.createFromFloatingPoint(this.approximate() % right.approximate());
    }
    add(right) {
        let newNumerator = (this.numerator * right.denominator) + (right.numerator * this.denominator);
        let newDenominator = this.denominator * right.denominator;
        return new Rational(newNumerator, newDenominator);
    }
    subtract(right) {
        return this.add(right.negate());
    }
    isLessThan(right) {
        return this.approximate() < right.approximate();
    }
    isLessThanOrEquals(right) {
        return this.approximate() <= right.approximate();
    }
    isGreaterThanOrEquals(right) {
        return this.approximate() >= right.approximate();
    }
    isGreaterThan(right) {
        return this.approximate() > right.approximate();
    }
    equals(right) {
        return this.approximate() === right.approximate();
    }
    notEquals(right) {
        return this.approximate() !== right.approximate();
    }
    negate() {
        return new Rational(-this.numerator, this.denominator);
    }
    toString() {
        return this.approximate().toString();
    }
    cancel() {
        if (!this.cancelled) {
            let gcd = Rational.gcd(this.numerator, this.denominator);
            this.denominator /= gcd;
            this.numerator /= gcd;
            this.cancelled = true;
        }
    }
    invert() {
        return new Rational(this.denominator, this.numerator);
    }
    approximate() {
        return this.numerator / this.denominator;
    }
    static gcd(x, y) {
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
        let gcd = 1;
        while (y != 0) {
            gcd = y;
            y = x % y;
            x = gcd;
        }
        return gcd;
    }
}
class Complex {
    constructor(real, imaginary) {
        this.real = real;
        this.imaginary = imaginary;
    }
    getMagnitude() {
        let r1 = this.real;
        let r2 = this.imaginary;
        // m = sqrt(r1**2 + r2**2) = (r1**2 + r2**2)**(1/2)
        return r1.exponentiate(new Rational(2, 1))
            .add(r2.exponentiate(new Rational(2, 1)))
            .exponentiate(new Rational(1, 2));
    }
    exponentiate(right) {
        let rho = this.getMagnitude();
        let theta = callWithRational2(Math.atan2, this.imaginary, this.real);
        let newRho = right.real.multiply(theta).add(right.imaginary.multiply(callWithRational1(Math.log, rho)));
        let t = rho.exponentiate(right.real).multiply(Rational.createFromFloatingPoint(Math.E).exponentiate(right.imaginary.negate().multiply(theta)));
        return new Complex(t.multiply(callWithRational1(Math.cos, newRho)), t.multiply(callWithRational1(Math.sin, newRho)));
    }
    multiply(right) {
        // Multiplication:  (a + bi)(c + di) = (ac - bd) + (bc + ad)i
        let resultReal = this.real.multiply(right.real).subtract(this.imaginary.multiply(right.imaginary));
        let resultImag = this.real.multiply(right.imaginary).add(right.real.multiply(this.imaginary));
        return new Complex(resultReal, resultImag);
    }
    divide(right) {
        let a = this.real;
        let b = this.imaginary;
        let c = right.real;
        let d = right.imaginary;
        if (abs(d).isLessThan(abs(c))) {
            let doc = d.divide(c);
            let resultReal = a.add(b.multiply(doc)).divide(c.add(d.multiply(doc)));
            let resultImag = b.subtract(a.multiply(doc)).divide(c.add(d.multiply(doc)));
            return new Complex(resultReal, resultImag);
        }
        else {
            let cod = c.divide(d);
            let resultReal = b.add(a.multiply(cod)).divide(d.add(c.multiply(cod)));
            let resultImag = a.negate().add(b.multiply(cod)).divide(d.add(c.multiply(cod)));
            return new Complex(resultReal, resultImag);
        }
    }
    add(right) {
        let resultReal = this.real.add(right.real);
        let resultImag = this.imaginary.add(right.imaginary);
        return new Complex(resultReal, resultImag);
    }
    subtract(right) {
        return this.add(right.negate());
    }
    equals(right) {
        return this.real.equals(right.real)
            && this.imaginary.equals(right.imaginary);
    }
    notEquals(right) {
        return !this.equals(right);
    }
    negate() {
        return new Complex(this.real.negate(), this.imaginary.negate());
    }
    toString() {
        return `${this.real.toString()} + ${this.imaginary.toString()}i`;
    }
}
function callWithRational1(f, r) {
    return Rational.createFromFloatingPoint(f(r.approximate()));
}
function callWithRational2(f, x, y) {
    return Rational.createFromFloatingPoint(f(x.approximate(), y.approximate()));
}
function abs(num) {
    if (num.getNumerator() < 0) {
        return num.negate();
    }
    return num;
}
const E = Rational.createFromFloatingPoint(Math.E);
const PI = Rational.createFromFloatingPoint(Math.PI);
const PHI = Rational.createFromFloatingPoint((1 + Math.sqrt(5)) / 2);
const TAU = Rational.createFromFloatingPoint(2 * Math.PI);
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
//# sourceMappingURL=app.js.map