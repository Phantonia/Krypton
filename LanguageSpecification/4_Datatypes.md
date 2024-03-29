# 4 Datatypes

Datatypes are used to guarantee the correctness of the program. Every function and every operation is defined with its types. Violations of type safety cause one of the compile time errors 500 <= x < 600.

## 4.1 Datatypes of literals

Every kind of literal has a built-in datatype associated with it.

| Kind of literal   | Datatype   | Example         |
| ----------------- | ---------- | --------------- |
| Integer literal   | `Int`      | `42`            |
| Rational literal  | `Rational` | `3.14159265`    |
| Imaginary literal | `Complex`  | `4i`            |
| Character literal | `Char`     | `'a'`           |
| String literal    | `String`   | `"Hello world"` |
| Boolean literal   | `Bool`     | `True`, `False` |

## 4.2 Default values

The default value is the value of a variable of a type that has not been assigned a value.

The default value of a type can be used in a typed context using the `Default` keyword and in any context using the `Default(T)` syntax, where `T` is the type. For example, the default value of the type `Int` can be used the following way: `Default(Int)`.

The following list lists the default values for built-in datatypes:

| Datatype   | Default value |
| ---------- | ------------- |
| `Int`      | `0`           |
| `Rational` | `0.0`         |
| `Complex`  | `0 + 0i`      |
| `Char`     | `'\0'`        |
| `String`   | `""`          |
| `Bool`     | `False`       |

## 4.3 Properties

A property of a type is accessible from expressions of this type. It has a type and is read only.

## 4.4 Type inference

If an expression has a type, it may be assigned to a variable declaration which lacks the `As` clause. The type of the variable is said to be inferred.

Inference works the following way:

- If the whole expression is a function call, the type is inferred to be the return type of this function.
- If the whole expression is an operation, the type is inferred to be the return type of the rightmost appearance of the operator with the lowest precedence (leftmost, if this operator is `**`).
- If the whole expression is a literal with a type (see <u>5.1 Datatypes of literals</u>), the type is inferred to be this literal kind's type.
- If the whole expression is a read access to a variable, the type is inferred to be the type that this variable was declared with (or that was inferred for this variable).

## 4.5 Conversions

If a type `X` is implicitly convertible to the type `Y`, an expression of type `X` can be used where an expression of type `Y` is expected. Implicit conversions are specified by the framework documentation. 