# 6 Statements

A statement is executable. The following kinds of statements exist:

## 6.1 Function call statements

A function call can be used as a statement. It uses the same syntax as a function call expression (see <u>3.4 Function call expressions</u>).

A function call statement has to be terminated by a semicolon character `;`. If this it not the case, the compile-time error 101 is reported.

## 6.2 Variable declaration statement

See <u>4.x Variable declaration statement</u>.

A variable declaration statement has to be terminated by a semicolon character `;`. If this is not the case, the compile-time error 101 is reported.

## 6.3 Variable assignment statement

See <u>4.x Variable assignment statement</u>.

A variable assignment statement has to be terminated by a semicolon character `;`. If this is not the case, the compile-time error 101 is reported.

## 6.4 `Return` statement

A `Return` statement immediately terminates execution of a function. If the function has a return type, it provides the return value of this function call. If a `Return` statement is used in top level code, execution of the whole script terminates.

A `Return` statement uses the following syntax:

```
Return; ... if this is in a function without return type or top level code
Return expression; ... if this is in a function with return type
```

The expression has to be of the return type or implicitly convertible to it.

A `Return` statement has to be terminated by a semicolon character `;`. If this it not the case, the compile-time error 101 is reported.

## 6.5 `Block` statement

A `Block` statement creates a new scope for local variable. It uses the following syntax:

```
Block
{
	statements
}
```

Where statements is an ordered list of any kind of statement. Nested `Block` statements are permitted.

## 6.6 `If` statement

### 6.6.1 `If`

An `If` statement makes a decision which snippet of code to execute based on a Boolean condition.

```
If condition
{
	... Statements
}
```

`condition` is any expression that returns `Bool`. The statements in the `{ }` block is only executed, if the condition evaluates as `True`.

### 6.6.2 `Else If`

An `If` statement may be followed by an unlimited number of `Else If` statements. If and only if the original condition evaluates as `False`, the condition for the next `Else If` is evaluated. If it evaluates as `True`, its statements are executed. Else the next `Else If` - if it exists - is considered.

```
If condition
{
	statements
}
Else If otherCondition
{
	statements
}
Else If yetAnotherCondition
{
	statements
}
```

Here, the same constraints that are in place for `condition` are also in place for the other conditions.

It is the compile time error 301 for an `Else If` statement to appear in code unless it directly follows an `If` or `Else If` statement.

### 6.6.3 `Else`

An `If` or `Else If` statement may be followed by exactly one `Else` statement. If the conditions of each of the `If` and `Else If` statements evaluated as either `False` or `Null`, the `Else` block is executed. If it doesn't exist, execution resumes after the last `Else If` statement.

```
If condition
{
	statements
}
Else
{
	statements
}
```

It is the compile time error 301 for an `Else` statement to appear in code unless it directly follows an `If` or `Else If` statement.

## 6.7 `While` statement

A `While` statement executes multiple times, as long as the condition always evaluates as `True`. It uses the following syntax:

```
While condition
{
	statements
}
```

It creates a new scope for local variables.

A `While` statement is a loop, so it permits `Leave` and `Continue` statements.

## 6.8 `For` statement

A `For` statement executes multiple times for a specific range of numbers. The current number can be used by accessing the iteration variable.

It can declare a new iteration variable. In that case, the syntax is:

```
For identifier As type = initial_value While condition With with_expression { statements }
For identifier = initial_value While condition With with_expression { statements }
```

This declares a new variable called *identifier* of type *type* with the initial value *initial_value* that is scoped to the body of the `For` statement.

*type* has to be either `Int`, `Rational` or `Complex`. As for any variable declaration, the assigned value (*initial_value*) has to be of that type. The `As` clause may be omitted. The type has to inferred as one of the previously listed types.

*condition* is a comparison of the variable declared as *identifier* with any other expression of that type. Therefore, it has to use one of the following operators: `<`, `<=`, `>` or `>=`. It does not matter whether the iteration variable appears on the left or right of that operation. Alternatively, it may be the Boolean literal `True`. The `While` part may be omitted. In that case, `True` is assumed. If *condition* is neither a comparison nor the Boolean literal `True`, the compile time error 303 is reported. 

*with_expression* is an expression, that is assigned to the iteration variable after each iteration. It has to include this variable somewhere and has to be of the same type. The `With` part may be omitted. In that case, `identifier + 1` is assumed.

It is the compile time error 304 to omit both the `While` as well as the `With` part.

It can also use an existing variable, if that variable is of type `Int`, `Rational` or `Complex`. In that case, the syntax is:

```
For identifier While condition With with_expression { statements }
```

The initial value is the value of the variable once execution reaches the `For` statement. The same constraints for *condition* and *with_expression* are in place and both may be omitted, but only if the other one is there.

A `For` statement is a loop, so it permits `Leave` and `Continue` statements.

## 6.9 `Leave` statement

A `Leave` statement leaves the body of a loop. It uses the following syntax:

```
Leave;
Leave integer_literal;
```

*integer_literal* is an integer literal that represents the number of nested loops to leave. If *integer_literal* is not provided, 1 is assumed. For example:

```
While True
{
	While True
	{
		Leave 2;
	}
}
```

This would leave the outer `While` loop.

It is the compile time error 302 for a `Leave` statement to appear without being in a loop or *integer_literal* to be greater than the number of nested loops.

A `Leave` statement has to be terminated by a semicolon character `;`. If this it not the case, the compile-time error 101 is reported.

## 6.10 `Continue` statement

A `Continue` statement terminates the current execution of the loop body. It reevaluates the condition and depending on whether it evaluates as `True` executes the loop body or leaves the loop. It uses the following syntax:

```
Continue;
Continue integer_literal;
```

*integer_literal* is an integer literal that represents the number of nested loops to continue. If *integer_literal* is not provided, 1 is assumed. For an example, see <u>6.3 `Leave` statement</u>.

A `Continue` statement has to be terminated by a semicolon character `;`. If this it not the case, the compile-time error 101 is reported.