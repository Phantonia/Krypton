# 7 Control flow statements

Control flow statements are statements that control which statements to execute in which order, and where variables are accessible. The following control flow statements exist:

## 7.2 `Block` statement

A `Block` statements has no effect on the flow of the program. However variables declared inside are only in scope inside.

A `Block` statement uses the following syntax:

```
Block
{
	... Statements
}
```

## 7.3 `If` statements

### 7.3.1 `If`

An `If` statement makes a decision which snippet of code to execute based on a Boolean condition.

```
If condition
{
	... Statements
}
```

`condition` is any expression that returns `Bool` or `Bool?`. The statements in the `{ }` block is only executed, if the condition is `True`, so not if it is `False` or `Null`.

### 7.3.2 `Else If`

An `If` statement may be followed by an unlimited number of `Else If` statements. If and only if the original condition evaluates as `False` or `Null`, the condition for the next `Else If` is evaluated. If it evaluates as `True`, its statements are executed. Else the next `Else If` - if it exists - is considered.

```
If condition
{
	... statements
}
Else If otherCondition
{
	... statements
}
Else If yetAnotherCondition
{
	... statements
}
```

Here, the same constraints that are in place for `condition` are also in place for the other conditions.

It is a compile time error for an `Else If` statement to appear in code unless it directly follows an `If` or `Else If` statement.

### 7.3.3 `Else`

An `If` or `Else If` statement may be followed by exactly one `Else` statement. If the conditions of each of the `If` and `Else If` statements evaluated as either `False` or `Null`, the `Else` block is executed. If it doesn't exist, execution resumes after the last `Else If` statement.

```
If condition
{
	... statements
}
Else
{
	... statements
}
```

