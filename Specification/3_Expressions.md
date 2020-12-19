# 3 Expressions

Expressions compute values at runtime. They can be used in the following contexts:

- As initializers of variables
- As arguments of function call
- As a return value
- As subexpressions, so as operands to other kinds of expressions
- etc.

The following kinds of expressions exist:

## 3.1 Constants

Constants are literals or declared constants. They yield the value of the constant and have the datatype that the kind of literal is associated with. More information in <u>5 Datatypes</u>.

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

`x + y`, `x - y`, `x * y`, `x / y`, `x Div y`, `x Mod y`, `x ** y`, `x & y`, `x ^ y`, `x | y`, `x == y`, `x != y`, `x > y`, `x < y`, `x >= y`, `x <= y`, `x And y`, `x Or y`, `x Xor y`

### 3.3.3 Operator precedence

Operator precedence decides the ambiguity of a case like this: `x + y * z`. Evaluating this as `(x + y) * z` yields a different result than `x + (y * z)`. Operators  with a higher precedence bind more closely. In this case, `*` has a higher precedence, so the second variant is chosen.

This is a full table of precedence:

| Precedence | Name of group          | Concrete operators                     |
| ---------- | ---------------------- | -------------------------------------- |
| 11         | Unary number operators | `-x`, `~x`                             |
| 10         | Exponentiation         | `x ** y`                               |
| 9          | Multiplicative         | `x * y`, `x / y`, `x Div y`, `x Mod y` |
| 8          | Additive               | `x + y`, `x - y`                       |
| 7          | Bitwise                | `x & y`, `x ^ y`, `x | y`              |
| 6          | Shift                  | `x Right y`, `x Left y`                |
| 5          | Comparison             | `x < y`, `x <= y`, `x >= y`, `x > y`   |
| 4          | Equality               | `x == y`, `x != y`                     |
| 3          | Logical Not            | `Not x`                                |
| 2          | Logical And            | `x And y`                              |
| 1          | Logical XOr            | `x Xor y`                              |
| 0          | Logical Or             | `x Or y`                               |

### 3.3.4 Short circuiting

The Logical `And` and `Or` operators are short circuiting. This means that the right operand is not evaluated, if it cannot change the result of the operation.

In case of the `And` operator, the right operand is only evaluated, if the left operand returned `True`. If it is `False`, the whole operation returns `False` without evaluating the right operand. If it is `True`, the whole operation returns the result of the right operand.

In case of the `Or` operator, the right operand is only evaluated, if the left operand returned `False`. If it is `True`, the whole operation returns `True` without evaluating the right operand. If it is `False`, the whole operation returns the result of the right operand.

## 3.4 Function call expressions

An identifier may be followed by a comma separated list of expression enclosed in `( )`. This list may be empty.

The function (see <u>8 Function</u>) associated with this identifier (see <u>10 Identifier binding</u>) is executed when this expression is evaluated. It yields the return value of this function.

## 3.x Expressions with and without type

If an expression has a type, it can be assigned to a variable declared without `As` clause. If the variable has an `As` clause, the expression has to implicitly convertible to that type.

If an expression does not have a type, it cannot be assigned to a variable declared without `As`. The expression has to be implicitly convertible to the type specified by that `As` clause.

The following table specifies which expression have a type and which do not:

| Expression kind        | Has a type | Type or conversion rule                                      |
| ---------------------- | ---------- | ------------------------------------------------------------ |
| Literal                | Yes        | Type specified by ???                                        |
| Variable access        | Yes        | Type the variable is declared with                           |
| Operation              | Yes        | Type specified by the Framework Documentation                |
| Function call          | Yes        | Type specified by the function declaration's `As` clause     |
| `Default(T)` for any T | Yes        | T                                                            |
| `Default`              | No         | Convertible to any type, equivalent to `Default(T)` where T is the target type |

