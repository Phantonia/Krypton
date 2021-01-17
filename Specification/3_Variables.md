# 3 Variables

A variable is a named storage location. It is declared with the following syntax:

```
local_declaration = var_decl_with_type
                  | var_decl_without_type;
                  | let_decl;
var_decl_with_type = "Var" identifier "As" type_spec [initializer];
var_decl_without_type = "Var" identifier initializer;
let_decl = "Let" identifier [type_spec_with_as] initializer;
initializer = "=" expression;
type_spec_with_as = "As" type_spec;
```

## 3.1 Identifier

Any legal identifier may be used as the name of the variable. It is a compile time error to declare a variable with an identifier that a different variable that is in scope (more in <u>4.5 Scope</u>) already uses.

## 3.2 As clause

The `As` keyword starts the As clause. It is used to specify the type of the new variable. This datatype may be any legal type expression as described in <u>5 Datatypes</u>.

The `As` clause may be left out if an expression is given and it has a type (see <u>3.5 Expressions with and without type</u>) but has to be provided if the expression does not have a type (see <u>3.x Expression with and without type</u>) or there is no expression in the first place. In case that it is left out, it is inferred from the expression (see <u>x Type inference</u>)

## 3.3 Assigned value

For `Var` variables optionally and for `Let` variables necessarily, after an equals character `=` an expression provides an initial value for the new variable.

This value has to match the type specified by the As clause if it has a type or has to be convertible to this type if it doesn't have a type. Else, the compile-time error 501 is reported.

If there is no assigned value and the variable is declared using `Var` it is assigned with the default value of the type specified by the As clause (see <u>5.x Default value of a datatype</u>).

## 3.4 `Var` vs `Let`

The value of a variable that has been declared using `Var` may be replaced after declaration with a new (not necessarily different) value. It is a compile time error for this new value to have a different type than the initial variable.

On the other hand, a variable declared with `Let` is immutable. That means that it is a compile time error to reassign this variable. A value has to be assigned at declaration because this value will not change for the lifetime of the variable.

## 3.5 Scope

The scope of a variable means the area of code where the variable may be used and assigned to. A function body or the top level code both create a scope, however the following statements create nested scopes:

- `Block`
- `If`
- `While`
- `For`

If in a scope a nested scope is created, the original scope is considered a parent scope of the nested scope. See this example:

```
Block
{ ... Parent scope
	Block
	{ ... Nested scope
		
	}
}
```

A variable is in scope in the scope that it is declared in and all of that scope's nested scopes.

If a variable is declared even though a different variable with the same name is in scope, a compile-time error is reported.