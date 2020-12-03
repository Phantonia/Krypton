# 10 Identifier binding

Identifiers are bound at the beginning of semantical analysis. Binding refers to the act of associating an identifier with the entity that is declared with this name.

Different binding semantics are applied to these different contexts:

## 10.1 In an expression

If a local variable declared with the identifier that is about to be bound is in scope (see <u>X Scope</u>, binding always chooses it, even if a function with the same name exists. This is the case even when the variable is called even though it does not allow it, while the call to the function would have been legal. An example:

```
Func Test()
{
	Var Identifier = 4;
	Identifier(); ... error
}

Func Identifier()
{
	Out "Called the function";
}
```

In this code snippet, a compile time error is reported. A variable of type `Int` is not callable. The function is not considered, even though the call would have been legal if the identifier would have been bound to the function.

If no local variable with this identifier is found, then built-in and declared functions are considered. If a function with this name is found, the identifier is bound to that function.

If neither a variable nor a function is found, a compile time error is reported.

## 10.2 In a type expression

In a type expression, only datatypes are considered when binding identifiers. The only identifiers that are allowed here are the built-in types:

- `Int`
- `Real`
- `Rational`
- `Complex`
- `Char`
- `String`
- `Bool`

Because of this, it is perfectly valid to have a variable or function in scope which is declared with one of these identifiers:

```
If Real()
{
	Out "Everything is real";
}
Else
{
	Out "This is just a dream";
}

Var r As Real; ... can still use the type Real.

Func Real() As Bool
{
	Return RandomReal() > 0.9;
}
```

