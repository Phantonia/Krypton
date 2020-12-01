# 4 Variables

A variable is a named storage location. It is declared with the following syntax:

```vb
Var {identifier} [As {Datatype}] [= {expression}];
Let {identifier} [As {Datatype}] = {expression};
```

Syntax in square brackets `[ ]` can be left out. More information under <u>4.2 As clause</u> and <u>4.3 Assigned value</u>.

Syntax in curly brackets `{ }` has to be replaced.

The following table describes the meaning of the different parts.

| Part          | Meaning                                                      |
| ------------- | ------------------------------------------------------------ |
| `Var` / `Let` | The keyword used to declare the variable. The difference is described under <u>4.4 Var vs Let</u>. |
| identifier    | The name of the new variable. It has to be a legal identifier as described in <u>2.x Identifiers</u>. |
| expression    | The value of the variable. It can be any legal expression as described in <u>3 Expression</u>. |

## 4.1 Identifier

Any legal identifier may be used as the name of the variable. It is a compile time error to declare a variable with an identifier that a different variable that is in scope (more in <u>4.5 Scope</u>) already uses.

## 4.2 As clause

The `As` keyword starts the As clause. It is used to specify the *datatype* of the new variable. This datatype may be any legal type expression as described in <u>5 Datatypes</u>.

The As clause may be left out if an expression is given and it has a type (see <u>3.x Expressions with type</u>) but has to be provided if the expression does not have a type (see <u>3.x Expression without type</u>) or there is no expression in the first place. In case that it is left out, it is inferred from the expression (see <u>x Type inference</u>)

## 4.3 Assigned value

For `Var` variables optionally and for `Let` variables necessarily, after an equals character `=` an expression provides an initial value for the new variable.

This value has to match the type specified by the As clause if it has a type or has to be convertible to this type if it doesn't have a type.

If there is no assigned value and the variable is declared using `Var` it is assigned with the default value of the type specified by the As clause (see <u>5.x Default value of a datatype</u>).

## 4.4 Var vs Let

The value of a variable that has been declared using `Var` may be replaced after declaration with a new(not necessarily different) value. It is a compile time error for this new value to have a different type than the initial variable.

On the other hand, a variable declared with `Let` is immutable. That means that it is a compile time error to reassign this variable. A value has to be assigned at declaration because this value will not change for the lifetime of the variable.

## 4.5 Scope

The scope of a variable means the area of code where the variable may be used and assigned to. For top level code this scope is the whole top level code; for functions it is the function body. This does not apply if the variable is declared in a nested scope. For more information on nested scopes, see <u>x Control flow statements</u>.

This is a list of control flow statements that create a new scope:

- `Block`
- `If`
- `While`
- `For`
- `With`
- `Do`

## 4.6 Variable expressions

A variable is a legal expression. The expression has the type of the variable.