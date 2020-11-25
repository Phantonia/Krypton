# 3 Expressions

Expressions compute values at runtime. They can be used in the following contexts:

- As initializers of variables
- As arguments of functions
- As the argument of an `Out` statement
- As the argument of a `Return` statement
- etc.

The following types of expressions exist:

## 3.1 Constants

Constants are literals or declared constants. They yield the value of the constant and have the *datatype* that the kind of literal is associated with. More information in <u>5 Datatypes</u>.

## 3.2 Variable access

Variables yield the value they were assigned most recently at runtime and have the datatype they were declared with. More information in <u>4 Variables</u>.

## 3.3 Operations

Operations use a **unary** or **binary operator**. An example would be `x + 4`.

### Unary operators

Unary operators modify the subexpression they are used with. Let `x` be a subexpression.

`-x` negates the value of x (in case the operator is defined )

## 3.4 Function calls