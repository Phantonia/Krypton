# X Compile-time Errors

A compile-time error is reported if the compiler spotted the violation of one of the rules specified by the Krypton Specification. Error codes are guaranteed to be consistent; the error message may be subject to change.

For the same illegal code it is not guaranteed that the same error is reported in case that there are multiple errors.

## X.1 Lexical errors

| Error code | Short description            | Example  | In spec |
| ---------- | ---------------------------- | -------- | ------- |
| 001        | Invalid lexeme               | `§`      | 2.X     |
| 002        | Unclosed string literal      | `"xyz`   | 2.X     |
| 003        | Unclosed char literal        | `'xyz`   | 2.X     |
| 004        | Unrecognized escape sequence | `"\m"`   | 2.X     |
| 005        | Hex literal with mixed case  | `0x4f5F` | 2.X     |

## X.2 Syntax errors

| Error code | Short description                                        | Example                      | In spec |
| ---------- | -------------------------------------------------------- | ---------------------------- | ------- |
| 101        | Semicolon expected                                       | `Output("Hello world")`      | ???     |
| 102        | `As` or equals expected                                  | `Var x;`                     | ???     |
| 103        | Equals or semicolon expected                             | `Var x As Int`               | ???     |
| 104        | Only function call expressions may be used as statements | `4 * 4;`                     | ???     |
| 105        | Identifier expected                                      | `Var As Int;`                | ???     |
| 201        | Unexpected expression term                               | `4 + ;`                      | ???     |
| 202        | Unclosed parenthesis                                     | `(4 + 5`                     | ???     |
| 203        | Expected comma or closing parenthesis                    | `SomeFunction(5`             | ???     |
| 204        | `Let` variable without initial value                     | `Let x As Int;`              | 4.3     |
| 301        | `Else` or `Else If` without preceding `If` or `Else If`  | `Else { }`                   | 6.5     |
| 302        | No loop found to leave or continue                       | `Leave;`                     | 6.x     |
| 303        | Wrong kind of condition in `For` loop                    | `For i = 0 While i != 6 { }` | 6.8     |
| 304        | Both `While` as well as `With` are omitted               | `For i = 0 { }`              | 6.8     |

## X.3 Semantical errors

| Error code | Short description                             | Example                             | In spec |
| ---------- | --------------------------------------------- | ----------------------------------- | ------- |
| 501        | Expression type does not match variable type  | `Var x As Int = "string";`          | 4.3     |
| 502        | Expression type does not match parameter type | `Func F(x As Int) { } F("xyz");`    | ???     |
| 503        | Expression type does not match return type    | `Func F() As Int { Return "xyz"; }` | ???     |
| 504        | Expression type does not have member          | `("string").Imaginary`              | ???     |
| 601        | Reassignment of `Let` variable                | `Let x = 4; x = 6;`                 | 4.4     |
