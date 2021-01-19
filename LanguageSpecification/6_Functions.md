# 6 Functions

A function is encapsulated functionality that can be called several times. It can be passed values as arguments and return a value.

## 6.1 Declaration syntax

A function declaration uses the following syntax:

```
func_decl = "Func" identifier param_list [type_spec_with_as] block;
param_list = "(" param {not_first_param} ")";
not_first_param = "," param;
param = identifier "As" type_spec;
type_spec_with_as = "As" type_spec;
```

The name of the function has to be a legal identifier as specified in <u>1.x Identifiers</u>. Additionally, it may not be used by another function or a constant, be it built-in or declared in the code.

The parameter list is described in <u>8.1 Parameters</u>.

The return type has to be a legal datatype as specified in <u>5 Datatypes</u>.

The body may be any collection of statements as specified in <u>6 Statements</u>.

## 6.2 Parameters

The parameter list is a comma separated list of the following syntax:

```
identifier As type
```

The identifier has to be a legal identifier and type has to be a legal datatype.

Parameters are available in the body of the function as variables of the specified type.

## 6.3 The return type

The `As` clause specifies the value that is going to be returned by the function and is going to be the value in an expression if the function call is used as an argument.

It can be omitted. In that case, the function doesn't return anything. A call to such a function may only be used as a statement but not as an expression.

## 6.4 The `Return` statement

The `Return` statement directly terminates execution of the function and returns the variable that the function call expression has.

Its syntax is:

```
Return value; ... Returns a value
Return;       ... Returns no value
```

If a return type is specified, `Return` with value has to be used. In that case, the type of the value has to match the return type.

If no return is type is specified, only `Return` without value is permitted.

## 6.5 Scope

The function body is a new scope. See <u>4.5 Scope</u>.