# 5 Datatypes

Datatypes are used to guarantee the correctness of the program. Every function and every operation is defined with its types. Usage of a function or operation with arguments of wrong types is a compile time error.

## 5.1 Datatypes of literals

Every kind of literal has a built-in datatype associated with it. For a list of built-in datatypes, see <u>5.4 Built-in datatypes</u>.

| Kind of literal   | Datatype  | Example         |
| ----------------- | --------- | --------------- |
| Integer literal   | `Int`     | `42`            |
| Real literal      | `Real`    | `3.14159265`    |
| Imaginary literal | `Complex` | `4i`            |
| Character literal | `Char`    | `'a'`           |
| String literal    | `String`  | `"Hello world"` |
| Boolean literal   | `Bool`    | `True`, `False` |

## 5.2 Default values

The default value is the value of a variable of a type that has not been assigned a value.

The default value of a type can be used in a typed context using the `Default` keyword and in any context using the `Default(T)` syntax, where `T` is the type. For example, the default value of the type `Int` can be used the following way: `Default(Int)`.

The following list lists the default values for built-in datatypes:

| Datatype   | Default value |
| ---------- | ------------- |
| `Int`      | `0`           |
| `Real`     | `0.0`         |
| `Complex`  | `0 + 0i`      |
| `Rational` | `0 Over 1`    |
| `Char`     | `'\0'`        |
| `String`   | `""`          |
| `Bool`     | `False`       |

### 5.2.1 Nullable types

The artificial default value `Null` can be understood as the absence of a value. `Null` can only be used in a context with a nullable type.

To use a nullable type, the type has to followed by a question mark character `?`.

For example, to allow the value `Null` for the type `Int`, the type has to be written the following way: `Int?`

Assigning a value of a non-nullable type to the corresponding nullable type is always legal. The other way round is only allowed if the value is explicitly converted with a `To` expression.

An example:

```
Var nonNullable As Int = 4;
Var nullable As Int? = nonNullable; ... legal

nonNullable = nullable; ... illegal
nonNullable = nullable To Int; ... legal
```

The conversion with `To` is equivalent to the following pseudocode:

```
If nullable != Null
{
        nonNullable = ValueOf(nullable);
        ... this is not legal Krypton. ValueOf represent the underlying value of the nullable value
}
Else
{
        nonNullable = Default;
}
```

Every operation exists in a **lifted** way:

If an operation exists on a non-nullable type, then the same operation exists with one or two nullable operands. It will be null if either one of the operands is null, else it will be the result of the operation but typed as nullable.

An example:

```
Var x As Int = 4;
Var y As Int = 12;
Out x + y; ... + exists for int

Var nx As Int?;
Var ny As Int?;

nx = 4;
ny = 12;
Out nx + ny; ... 16

nx = Null;
ny = 12;
Out nx + ny; ... Null

nx = Null;
ny = Null;
Out nx + ny; ... Null
```

## 5.3 Type inference

If an expression has a type, it may be assigned to a variable declaration which lacks the `As` clause. The type of the variable is said to be **inferred**.

Inference works the following way:

- If the whole expression is a function call, the type is inferred to be the return type of this function.
- If the whole expression is an operation, the type is inferred to be the return type of the rightmost appearance of the operator with the lowest precedence.
- If the whole expression is a literal with a type (see <u>5.1 Datatypes of literals</u>), the type is inferred to be this literal kind's type.
- If the whole expression is a read access to a variable, the type is inferred to be the type that this variable was declared with (or that was inferred for this variable)

