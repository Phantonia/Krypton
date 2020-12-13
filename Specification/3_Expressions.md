# 3 Expressions

Expressions compute values at runtime. They can be used in the following contexts:

- As initializers of variables
- As arguments of functions
- As the argument of a `Return` statement
- As subexpressions, so as operands to other kinds of expressions
- etc.

The following types of expressions exist:

## 3.1 Constants

Constants are literals or declared constants. They yield the value of the constant and have the *datatype* that the kind of literal is associated with. More information in <u>5 Datatypes</u>.

## 3.2 Variable access

Variables yield the value they were assigned most recently at runtime and have the datatype they were declared with. More information in <u>4 Variables</u>.

## 3.3 Operations

Operations use a **unary** or **binary operator**. An example would be `x + 4`.

These operators are only valid on types that allow them. More information in the framework documentation.

### 3.3.1 Unary operators

Unary operators modify the subexpression they are used with.

The following unary operators exist:

`-x`, `~x` and `Not x`

### 3.3.2 Binary operators

Binary operators take two subexpressions as their left and right operands and return a value based on their two operands.

The following binary operators exist:

`x + y`, `x - y`, `x * y`, `x / y`, `x Div y`, `x Over y`, `x Mod y`, `x ** y`, `x & y`, `x ^ y`, `x | y`, `x == y`, `x != y`, `x > y`, `x < y`, `x >= y`, `x <= y`, `x And y`, `x Or y`, `x Xor y`

### 3.3.3 Operator precedence

Operator precedence decides the ambiguity of a case like this: `x + y * z`. Evaluating this as `(x + y) * z` yields a different result than `x + (y * z)`. Operators  with a higher precedence bind more closely. In this case, `*` has a higher precedence, so the second variant is chosen.

This is a full table of precedence:

| Precedence | Name of group          | Concrete operators                                 |
| ---------- | ---------------------- | -------------------------------------------------- |
| 11         | Unary number operators | `-x`, `~x`                                         |
| 10         | Exponentiation         | `x ** y`                                           |
| 9          | Multiplicative         | `x * y`, `x / y`, `x Div y`, `x Mod y`, `x Over y` |
| 8          | Additive               | `x + y`, `x - y`                                   |
| 7          | Bitwise                | `x & y`, `x ^ y`, `x | y`                          |
| 6          | Shift                  | `x Right y`, `x Left y`                            |
| 5          | Comparison             | `x < y`, `x <= y`, `x >= y`, `x > y`               |
| 4          | Equality               | `x == y`, `x != y`                                 |
| 3          | Logical Not            | `Not x`                                            |
| 2          | Logical And            | `x And y`                                          |
| 1          | Logical XOr            | `x Xor y`                                          |
| 0          | Logical Or             | `x Or y`                                           |

### 3.3.4 Short circuiting

The Logical `And` and `Or` operators are short circuiting. That means that the right operand is only evaluated in case the left operand did not evaluate in a way that determines the result of the operation no matter the right operand.

In case of the `And` operator, the right operand is only evaluated, if the left operand returned `true`. If it is `false`, the whole operation returns `false` without evaluating the right operand. If it is `true`, the whole operation returns the result of the right operand.

In case of the `Or` operator, the right operand is only evaluated, if the left operand returned `false`. If it is `true`, the whole operation returns `true` without evaluating the right operand. If it is `false`, the whole operation returns the result of the right operand.

## 3.4 Function calls