# 5 Statements

A statement is an executable portion of code.

```
stmt = single_stmt
     | parent_stmt;

single_stmt = local_decl_stmt
            | assignment_stmt
            | func_call_stmt
            | leave_stmt
            | continue_stmt
            | return_stmt;

parent_stmt = if_stmt
            | while_stmt
            | for_stmt
            | block_stmt;
```

## 5.1 Function call statements

A function call can be used as a statement. It uses the same syntax as a function call expression (see <u>3.4 Function call expressions</u>):

```
func_call_stmt = func_call_exp ";";

func_call_exp = id arglist;
arglist = "(" [arglist_not_empty] ")";
arglist_not_empty = exp {notfirst_arg};
notfirst_arg = "," exp;
```

This is an example of a function call statement:

```
Output("Hello world");
```

A function call statement has to be terminated by a semicolon character `;`. If this it not the case, a compile-time error is reported.

## 5.2 Variable declaration statements

A variable declaration uses the following syntax:

```
local_decl_stmt = var_decl_with_type
                | var_decl_without_type;
                | let_decl;
var_decl_with_type = "Var" id "As" typespec [initializer] ";";
var_decl_without_type = "Var" id initializer ";";
let_decl = "Let" id [typespec_with_as] initializer ";";

initializer = "=" exp;
typespec_with_as = "As" typespec;
```

A variable declaration statement has to be terminated by a semicolon character `;`. If this is not the case, a compile-time error is reported.

This is an example of a variable declaration statement:

```
Var x As String = "";
```

For more information, see <u>4 Variables</u>.

## 5.3 Variable assignment statements

A variable assignment uses the following syntax:

```
assignment_stmt = id initializer ";";
initializer = "=" exp;
```

A variable assignment statement has to be terminated by a semicolon character `;`. If this is not the case, a compile-time error is reported.

## 5.4 `Return` statements

A `Return` statement immediately terminates execution of a function. If the function has a return type, it provides the return value of this function call. If a `Return` statement is used in top level code, execution of the whole script terminates.

A `Return` statement uses the following syntax:

```
continue_stmt = "Continue" [int_literal] ";";
```

The expression has to be of the return type or implicitly convertible to it. If there is no return type, there must not be an expression. If the expression is not of the correct type or the presence / absence of the expression does not match the presence / absence of a return type, a compile-time error is reported.

A `Return` statement has to be terminated by a semicolon character `;`. If this it not the case, the compile-time error 101 is reported.

## 5.5 `Block` statements

A `Block` statement creates a new scope for local variables. It uses the following syntax:

```
block_statement = "Block" statement_block;
statement_block = "{" {statement} "}";
```

## 5.6 `If` statements

An `If` statement selects a block of code to execute based on at least one condition. It use the following syntax:

```
if_stmt = if_part {elseif_part} [else_part];
if_part = "If" exp block;
elseif_part = "Else" "If" exp block;
else_part = "Else" block;

block = "{" {stmt} "}";
```

### 5.6.1 `If` part

The `If` part starts an `If` statement. The condition is any expression that returns `Bool`. The statements in the block is only executed if the condition evaluates as `True`. If it doesn't and there exists at least one `Else If` or `Else` part, the next part is started.

### 5.6.2 `Else If` part

The `If` part may be followed by an unlimited number of `Else If` statements. If and only if the original condition evaluates as `False`, the condition for the next `Else If` is evaluated. If it evaluates as `True`, its statements are executed. Else the next `Else If` - if it exists - is considered. Here, the same constraints that are in place for `condition` are also in place for the other conditions.

It is a compile-time error for an `Else If` part to appear in code unless it directly follows an `If` or `Else If` part.

### 5.6.3 `Else` part

An `If` or `Else If` part may be followed by exactly one `Else` part. If the conditions of each of the `If` and `Else If` parts evaluated as `False`, the `Else` block is executed. If it doesn't exist, execution resumes after the last `Else If` statement.

It is a compile-time error for an `Else` statement to appear in code unless it directly follows an `If` or `Else If` statement.

## 5.7 `While` statements

A `While` statement executes multiple times, as long as the condition always evaluates as `True`. It uses the following syntax:

```
while_stmt = "While" exp block;
```

A `While` statement is a loop, so `Leave` and `Continue` statements can interact with it.

## 5.8 `For` statements

A `For` statement executes multiple times for a specific range of numbers. The current number can be used by reading from the iteration variable.

A `For` statement uses the following syntax:

```
for_stmt = "For" for_var_part for_parts block;
for_parts = for_while_part for_with_part
          | for_while_part
          | for_with_part;

for_var_part = id [initializer]
             | "Var" id [typespec_with_as] initializer;
			 
for_while_part = "While" for_condition;
for_condition = for_condition_leading_var
              | for_condition_trailing_var
              | "True";
for_condition_leading_var = id comparison_operator exp;
for_condition_trailing_var = exp comparison_operator id;
comparison_operator = ">"
                    | "<"
                    | ">="
                    | "<=";
					
for_with_part = "With" exp;
```

If the `For` keyword is followed by an identifier, that identifier has to correspond to a local variable. This local variable is then used as the iteration variable. It can optionally be assigned a new value. If this is not the case, the initial value is the value that the variable holds at that time.

If the `For` keyword is followed by a variable declaration (so the `Var` keyword, an identifier, optionally a type specifier, and an assignment), a new iteration variable is declared. It is scoped to the body of the block of the `For` statement.

The variable is only accepted if is of one of the following types: `Int`, `Rational` or `Complex`.

After the `While` keyword a condition specifies when the loop will run (this is equivalent to the condition for a `While` loop). This condition is a comparison of the iteration variable with any other expression of that type. Therefore, it has to use one of the following operators: `<`, `<=`, `>` or `>=`. It does not matter whether the iteration variable appears on the left or right of that operation. Alternatively, it may be the Boolean literal `True`. The `While` part may be omitted. In that case, `True` is assumed. If the condition is neither a comparison nor the Boolean literal `True`, a compile-time error is reported.

After the `With` keyword there is an assignment to the iteration variable. This assignment has to be valid in terms of its types. After each iteration, the expression that is assigned to the variable is evaluated and assigned to the variable. The `With` part may be omitted. In that case, `iterationVariable = iterationVariable + 1` is assumed.

It is a compile-time error to omit both the `While` as well as the `With` part.

A `For` statement is a loop, so `Leave` and `Continue` statements can interact with it.

## 5.9 `Leave` statements

A `Leave` statement leaves the body of a loop. It uses the following syntax:

```
leave_stmt = "Leave" [int_literal] ";";
```

The integer literal represents the number of nested loops to leave. If it is not provided, 1 is assumed. For example:

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

It is a compile-time error for a `Leave` statement to appear without being in a loop or the integer literal to be greater than the number of nested loops.

A `Leave` statement has to be terminated by a semicolon character `;`. If this it not the case, a compile-time error is reported.

## 5.10 `Continue` statements

A `Continue` statement terminates the current execution of the loop body. It reevaluates the condition and depending on whether it evaluates as `True` executes the loop body or leaves the loop. It uses the following syntax:

```
continue_stmt = "Continue" [int_literal] ";";
```

The integer literal represents the number of nested loops to continue. If it is not provided, 1 is assumed. It behaves exactly like the `Leave` statement in choosing the loop to continue.

A `Continue` statement has to be terminated by a semicolon character `;`. If this it not the case, a compile-time error is reported.