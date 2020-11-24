# Krypton

**Krypton** is a flexible yet statically typed language for quick scripts. It directly supports rational number (i.e. fractions) and complex numbers.

At this point, it can't do anything. *But* it is gonna grow and become something, I promise ^^

Some syntactic impression ^^
(I'm using visual basic for syntax highlighting. It doesn't work 100%, but this language most closely matches Krypton's syntax)

```vb
Out "Hello world uwu"; ... Outputs "Hello world uwu"

Var complex As Complex = 3 + 4i;
Out complex; ... "3 + 4i"
Out complex.Real; ... "3"
Out complex.Imag; ... "4"
Out complex.Magnitude; ... "5"

Var fraction As Rational = 1 Over 2;
Out fraction; ... "1/2"
Out fraction.Numerator; ... "1"
Out fraction.Denominator; ... "2"

Var fraction2 = 2 Over 4; ... As can be left out
Out fraction2; ... "1/2"

Var input As String = Input();
If (input Is x As Int)
{
        Out x ** 2; ... Squares the input
}
Else
{
        Out "Please enter an integer";
}
```



