# 8 Functions

A function is encapsulated functionality that can be called several times. It can be passed values as **arguments** and **return** a value.

## 8.1 Declaration syntax

```
Func name(paramlist) As returnType
{
	...body
}
```

The name of the function has to be a legal identifier as specified in <u>1.x Identifiers</u>.

The paramlist is described in <u>8.1 Parameters</u>.

The return type has to be a legal datatype as specified in <u>5 Datatypes</u>.

The body may be any collection of statements as specified in <u>6 Statements</u>.

## 8.2 Parameters

The parameter list is a comma separated list of the following syntax:

```
identifier As type
```

The identifier has to be a legal identifier and type has to be a legal datatype.

Parameters are available in the body of the function as variables of the specified type.

## 8.3 The `Return` statement

