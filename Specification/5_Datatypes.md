# 5 Datatypes

## 5.1 Usage of datatypes

## 5.2 Datatypes of literals

Every kind of literal has a built-in datatype associated with it. For a list of built-in datatypes, see <u>5.4 Built-in datatypes</u>.

| Kind of literal   | Datatype   | Example         |
| ----------------- | ---------- | --------------- |
| Integer literal   | `Int`      | `42`            |
| Rational literal  | `Rational` | `3.14159265`    |
| Imaginary literal | `Complex`  | `4i`            |
| Character literal | `Char`     | `'a'`           |
| String literal    | `String`   | `"Hello world"` |
| Boolean literal   | `Bool`     | `True`, `False` |

## 5.3 Default values

The default value is the value of a variable of a type that has not been assigned a value.

The default value of a type can be used in a typed context using the `Default` keyword and in any context using the `Default(T)` syntax, where `T` is the type. For example, the default value of the type `Int` can be used the following way: `Default(Int)`.

The following list lists the default values for built-in datatypes:

| Datatype   | Default value |
| ---------- | ------------- |
| `Int`      | `0`           |
| `Rational` | `0.0` / `0/1` |
| `Complex`  | `0 + 0i`      |
| `Char`     | `'\0'`        |
| `String`   | `""`          |
| `Bool`     | `False`       |

### 5.3.1 Nullable types

The artificial default value `Null` can be understood as the absence of a value. `Null` can only be used in a context with a nullable type.

To use a nullable type, the type has to followed by a question mark character `?`.

For example, to allow the value `Null` for the type `Int`, the type has to be written the following way: `Int?`

Assigning a value of a non-nullable type to the corresponding nullable type is always legal. The other way round is only allowed if the value is explicitly converted with a `To` expression.

An example:

```vb
Var nonNullable As Int = 4;
Var nullable As Int? = nonNullable; ... legal

nonNullable = nullable; ... illegal
nonNullable = nullable To Int; ... legal
```

The conversion with `To` is equivalent to the following pseudocode:

```vb
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

```vb
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

## 5.4 Built-in datatypes

### 5.4.1 Number types

### 5.4.2 The `Char` type

### 5.4.3 The `String` type

### 5.4.4 The `Bool` type

## 5.5 Array types

## 5.6 Type inference