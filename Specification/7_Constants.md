# 7 Constants

A constant is a named literal. It always points to the same value and can never change. It is declared globally.

It uses the following syntax:

```
const_decl = "Const" id [typespec_with_as] "=" literal ";";
typespec_with_as = "As" typespec;
```

It is a compile-time error for a constant to have the same identifier as another constant or a function, be it built-in or declared in the program.

Reading from a constant is an expression. This expression has the type of the constant (be it explicitly specified or inferred).