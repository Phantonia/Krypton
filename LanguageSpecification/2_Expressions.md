# 2 Expressions

Expressions compute values at runtime. They can be used in the following contexts:

- As initializers of variables
- As arguments of function call
- As a return value
- As subexpressions, so as operands to other kinds of expressions

Expression have the following syntax:

```
expression = identifier
           | literal
           | binary_operation
           | function_call_expressions
           | member_access;
```

## 2.1 Literals

Literals yield the value of the literal and have the datatype that the kind of literal is associated with. More information in <u>5 Datatypes</u>.

```
literal = string_literal
        | char_literal
        | int_literal
        | imag_literal
        | rational_literal
        | "True"
        | "False";
```

## 2.2 Identifiers

Identifiers represent local variables, constants, or functions.

### 2.2.1 Variables

If the identifier is bound to a local variable (<u>10 Identifier binding</u>), the expression has the type of that variable. More information in <u>4 Variables</u>.

### 2.2.2 Constants

If the identifier is bound to a constant (<u>10 Identifier binding</u>), the expression has the type and value of that constant. More information in <u>X Constants</u>.

### 2.2.3 Functions

If the identifier is bound to a function (<u>10 Identifier binding</u>), the expression can be invoked. See <u>X Functions</u> and <u>2.4 Function call expressions</u>.

## 2.3 Operations

Operations use a unary or binary operator. An example would be `x + 4`.

These operators are only valid on types that allow them. More information in the framework documentation.

### 2.3.1 Unary operations

Unary operators modify the subexpression they are used with.

A unary operation uses the following syntax:

```
unary_op_exp = unary_operator exp;
unary_operator = "~"
               | "-"
               | "Not";
```

The following unary operators exist:

`-x`, `~x` and `Not x`

### 2.3.2 Binary operations

Binary operators take two subexpressions as their left and right operands and return a value based on those two expressions.

A binary operation uses the following syntax:

```
binary_op_exp = exp binary_operator exp;
binary_operator = "**"
                | "*"
                | "/"
                | "Div"
                | "Mod"
                | "+"
                | "-"
                | "&"
                | "^"
                | "|"
                | "->"
                | "<-"
                | "<"
                | "<="
                | ">="
                | ">"
                | "=="
                | "!="
                | "And"
                | "Xor"
                | "Or";
```

A binary operations returns a value based on their two operands. The functionality and type of the expression is specified by the framework documentation.

The following binary operators exist:

`x + y`, `x - y`, `x * y`, `x / y`, `x Div y`, `x Mod y`, `x ** y`, `x & y`, `x ^ y`, `x -> y`, `x <- y`, `x | y`, `x == y`, `x != y`, `x > y`, `x < y`, `x >= y`, `x <= y`, `x And y`, `x Or y`, `x Xor y`

### 2.3.3 Operator precedence

Operator precedence decides the ambiguity of a case like this: `x + y * z`. Evaluating this as `(x + y) * z` yields a different result than `x + (y * z)`. Operators  with a higher precedence bind more closely. In this case, `*` has a higher precedence, so the second variant is chosen.

This is a full table of precedence:

| Precedence | Name of group          | Concrete operators                     |
| ---------- | ---------------------- | -------------------------------------- |
| 11         | Unary number operators | `-x`, `~x`                             |
| 10         | Exponentiation         | `x ** y`                               |
| 9          | Multiplicative         | `x * y`, `x / y`, `x Div y`, `x Mod y` |
| 8          | Additive               | `x + y`, `x - y`                       |
| 7          | Bitwise                | `x & y`, `x ^ y`, `x | y`              |
| 6          | Shift                  | `x -> y`, `x <- y`                     |
| 5          | Comparison             | `x < y`, `x <= y`, `x >= y`, `x > y`   |
| 4          | Equality               | `x == y`, `x != y`                     |
| 3          | Logical Not            | `Not x`                                |
| 2          | Logical And            | `x And y`                              |
| 1          | Logical exclusive Or   | `x Xor y`                              |
| 0          | Logical Or             | `x Or y`                               |

Unlike the `**` operator, chains of the same operator are always analyzed the following way: `a op b op c` = `(a op b) op c`. For the `**` operator, it is done the other way round: `a ** b ** c` = `a ** (b ** c)`.

### 2.3.4 Short circuiting

The Logical `And` and `Or` operators are short circuiting. That means that the right operand is not evaluated if it cannot change the result of the operation.

In case of the `And` operator, the right operand is only evaluated if the left operand returned `True`. If it returned `False`, the whole operation returns `False` without evaluating the right operand. If it is `True`, the whole operation returns the result of the right operand.

In case of the `Or` operator, the right operand is only evaluated if the left operand returned `False`. If it returned `True`, the whole operation returns `True` without evaluating the right operand. If it is `False`, the whole operation returns the result of the right operand.

## 2.4 Function call expressions

An identifier may be followed by a comma separated list of expression enclosed in `()`. This list may be empty.

```
func_call_exp = id "(" [arglist] ")";
arglist = exp {notfirst_arg};
notfirst_arg = "," exp;
```

The function (see <u>7 Function</u>) associated with the identifier (see <u>10 Identifier binding</u>) is executed when this expression is evaluated. It yields the return value of this function.

## 2.5 Property get expressions

A property get expression reads the content of a property of an expression. It uses the following syntax:

```
prop_get_exp = exp "." id;
```

The property has to exist on the type of the expression (see <u>5.x Properties</u>), else a compile-time error is reported. The property get expression has the type of this property.

This is an example for a property get expression:

```
Var z As Complex = 3 + 4i;
Var r = z.Real; ... z.Real is the property get
```

## 2.6 Expressions with and without type

If an expression has a type, it can be assigned to a variable declared without type specifier (`As` clause). If the variable has a type specifier, the expression has to implicitly convertible to that type.

If an expression does not have a type, it cannot be assigned to a variable declared without type specifier. The expression has to be implicitly convertible to the type specified by that type specifier.

The following table specifies which expression have a type and which do not:

| Expression kind             | Has a type | Type or conversion rule                                      |
| --------------------------- | ---------- | ------------------------------------------------------------ |
| Literal                     | Yes        | Type specified by <u>5.x Literal types</u>                   |
| Variable                    | Yes        | Type the variable is declared with                           |
| Constant                    | Yes        | Type the constant is declared with                           |
| Operation                   | Yes        | Type specified by the framework documentation                |
| Function call               | Yes        | Type specified by the function declaration's type specifier  |
| `Default(T)` for any type T | Yes        | T                                                            |
| `Default`                   | No         | Convertible to any type, equivalent to `Default(T)` where T is the target type |

