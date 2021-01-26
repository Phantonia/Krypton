# Operators

This is a list of operations that are defined on the built-in types:

## String

| Operator | Operation      | Parameter types    | Return type |
| -------- | -------------- | ------------------ | ----------- |
| `+`      | Concatenation. | `String`, `String` | `String`    |

## Int

| Operator | Operation              | Parameter types | Return type |
| -------- | ---------------------- | --------------- | ----------- |
| `~`      | Bitwise Not            | `Int`           | `Int`       |
| `-`      | Negation               | `Int`           | `Int`       |
| `*`      | Multiplication         | `Int`, `Int`    | `Int`       |
| `Div`    | Integer division       | `Int`, `Int`    | `Int`       |
| `Mod`    | Modulo                 | `Int`, `Int`    | `Int`       |
| `+`      | Addition               | `Int`, `Int`    | `Int`       |
| `-`      | Subtraction            | `Int`, `Int`    | `Int`       |
| `&`      | Bitwise And            | `Int`, `Int`    | `Int`       |
| `^`      | Bitwise Xor            | `Int`, `Int`    | `Int`       |
| \|       | Bitwise Or             | `Int`, `Int`    | `Int`       |
| `->`     | Bitwise right shift    | `Int`, `Int`    | `Int`       |
| `<-`     | Bitwise left shift     | `Int`, `Int`    | `Int`       |
| `<`      | Less than              | `Int`, `Int`    | `Bool`      |
| `<=`     | Less than or equals    | `Int`, `Int`    | `Bool`      |
| `>=`     | Greater than or equals | `Int`, `Int`    | `Bool`      |
| `>`      | Greater than           | `Int`, `Int`    | `Bool`      |
| `==`     | Equality               | `Int`, `Int`    | `Bool`      |
| `!=`     | Inequality             | `Int`, `Int`    | `Bool`      |

## Rational

| Operator | Operation              | Parameter types        | Return type |
| -------- | ---------------------- | ---------------------- | ----------- |
| `-`      | Negation               | `Rational`             | `Rational`  |
| `**`     | Exponentiation         | `Rational`, `Rational` | `Rational`  |
| `*`      | Multiplication         | `Rational`, `Rational` | `Rational`  |
| `/`      | Rational division      | `Rational`, `Rational` | `Rational`  |
| `Mod`    | Modulo                 | `Rational`, `Rational` | `Rational`  |
| `+`      | Addition               | `Rational`, `Rational` | `Rational`  |
| `-`      | Subtraction            | `Rational`, `Rational` | `Rational`  |
| `<`      | Less than              | `Rational`, `Rational` | `Bool`      |
| `<=`     | Less than or equals    | `Rational`, `Rational` | `Bool`      |
| `>=`     | Greater than or equals | `Rational`, `Rational` | `Bool`      |
| `>`      | Greater than           | `Rational`, `Rational` | `Bool`      |
| `==`     | Equality               | `Rational`, `Rational` | `Bool`      |
| `!=`     | Inequality             | `Rational`, `Rational` | `Bool`      |

## Complex

| Operator | Operation         | Parameter types        | Return type |
| -------- | ----------------- | ---------------------- | ----------- |
| `-`      | Unary negation    | `Complex`              | `Complex`   |
| `**`     | Exponentiation    | `Complex`, `Complex`   | `Complex`   |
| `*`      | Multiplication    | `Complex`, `Complex`   | `Complex`   |
| `/`      | Rational division | `Complex`, `Complex`   | `Complex`   |
| `+`      | Addition          | `Complex`, `Complex`   | `Complex`   |
| `-`      | Subtraction       | `Complex`, `Complex`   | `Complex`   |
| `==`     | Equality          | `Complex`, `Complex`   | `Bool`      |
| `!=`     | Inequality        | `Complex`, `Complex`   | `Bool`      |

## Bool

| Operator | Operation   | Parameter types | Return type |
| -------- | ----------- | --------------- | ----------- |
| `==`     | Equality    | `Bool`, `Bool`  | `Bool`      |
| `!=`     | Inequality  | `Bool`, `Bool`  | `Bool`      |
| `Not`    | Logical Not | `Bool`          | `Bool`      |
| `And`    | Logical And | `Bool`, `Bool`  | `Bool`      |
| `Xor`    | Logical Xor | `Bool`, `Bool`  | `Bool`      |
| `Or`     | Logical Or  | `Bool`, `Bool`  | `Bool`      |
