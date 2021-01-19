# 1 Lexical structure

## 1.1 Case

Krypton programs are entirely case sensitive. That means, that an identifier `Something` and an identifier `something` (note the different casing of the "S") do not mean the same thing and can coexist. Casing also affects keywords. For example, the keyword `Var` is used for declaring a variable and due to being a reserved keyword, it cannot be used as and identifier, while `var` is a valid identifier.

## 1.2 Whitespace

For the most part, programs are whitespace insensitive. That means, that no whitespace affects the meaning of a script. For example, code can have any number of blank lines, and can be indented with any number of tabs or spaces.

As an example, all three of these scripts are legal and mean the same thing:

```
Output("What's your name?");
Var name = Input();
If name.Length > 10
{
	Output("Oh, you have a long name!");
}
Out "Hello " + name;
```

```
Output("What's your name?")Var name=Input();If name.Length>10{Out"Oh, you have a long name!";}Out"Hello "+name;
```

```
Output("What's your name?");
	Var name = Input();
		If name.Length > 10
			{
        		Output("Oh, you have a long name!");
					}
    
    
    
Output("Hello " + name);
```

The only situation in which a whitespace character changes the meaning of a program is between two identifiers, two reserved keywords or one identifier and one keyword. Consider the following situation:

```
Var x As String;
VarxAsString;
```

The first one declares a variable of datatype `String` whereas the second one is a compile time error because a single identifier can never be used as a whole statement.

## 1.3 Comments

A comment is a portion of code that is ignored after lexical analysis.

There are three kinds of comments:

### 1.3.1 Single line comments

A single line comment is opened using three dot characters `.` and does not have to be closed as it is considered closed at the end of the line.

```
Output("Hello world!"); ... this is a comment
```

###  1.3.2 Normal multiline comment

A normal multiline comment is opened using two consecutive greater than characters `>` and closed using two consecutive less than characters `<`. Because of this syntax, it is possible for a not nested multiline comment to span multiple lines. However, there is no requirement for that.

A normal multiline comment works like a nested multiline comment with the exception of the balancing rule, which does not apply to weak multiline comments. Thus, this comment is properly closed:

```
>> This is a comment >> It is closed there: <<
```

As you can see, normal multiline comments only use two greater/less than signs.

It is legal for a weak multiline comment to not be closed at all. In that case the whole rest of the code will be ignored.

### 1.3.3 Nested multiline comment

A nested multiline comment works like a normal multiline comment, except that it uses three consecutive greater than characters for the starting and the ending lexeme.

```
Output("Hello world!"); >>> This is a comment <<<
>>> This is also a comment
It is not closed on the same line <<<
```

An additional rule applies. The number of starting lexemes `>>>` has to equal the number of ending lexemes `<<<`. That means, that the following comment is not closed, for example:

```
>>> This is comment >>> It is not closed there: <<<
```

That is because there are two starting lexemes but only one ending lexeme. To properly close this comment, the code has to look like this:

```
>>> This is a comment >>> It is not closed there: <<< But there: <<<
```

Both kinds of comments ignore the starting and ending lexemes of the other kind. That means, that this strict multiline comment is closed:

```
>>> Strict >> something <<<
```

And these comments aren't:

```
>>> Comment <<
>> Comment <<<
```

It is legal for a nested multiline comment to not be closed at all. In that case, the whole rest of the code will be ignored.

## 1.4 Identifiers

Identifiers are strings of characters that obey the following rules:

- The first character is a letter or an underscore
- The second to last character are all letters, underscores and/or numbers
- The identifier as a whole does not equal a reserved keyword (case sensitive)

This is the syntax for an identifier:

```
id = id_first_char {id_other_char};
id_other_char = id_first_char | number;
id_first_char = letter | "_";
```

(Here, `letter` corresponds to the characters in the range `0x41` ("A") to `0x5A` ("Z") and `0x61` ("a") to `0x7A` ("z"), and `number` corresponds to the characters in the range `0x30` ("0") to `0x39` ("9"). )

These are examples of valid identifiers: `PascalCase`, `camelCase`, `snake_case`, `SCREAMING_SNAKE_CASE`, `___` (these are three consecutive underscores)
These are examples of invalid identifiers: `3D`, `$abc`, `If`

## 1.5 Reserved keywords

Reserved keywords are strings of letters with a special meaning in the language. An identifier cannot equal a reserved keyword.

This is a full list of reserved keywords:

- `And`
- `Continue`
- `Div`
- `Func`
- `Leave`
- `Left`
- `Let`
- `Mod`
- `Or`
- `Return`
- `Right`
- `Var`
- `With`
- `Xor`

## 1.6 String literals

A **string** is a string of characters.

A string literal is a hardcoded string in the script itself. It starts and ends with a double quotation mark character `"`. It is the compile time error 002 to start a string literal but not end it on the same line.

The backslash character `\` can stop the string literal from ending. If a quotation mark is prepended by backslash character, it will not close the string literal but instead exist in the string literal itself as a character. A backslash does not escape another character if itself is prepended by an unescaped backslash character.

Unescaped quotation marks and backslashes do not appear in the final string.

An example for a valid string literal: `"This is a string!"`

An example for an invalid string literal: `"This is not a string`

## 1.7 Character literals

While a string represents several characters one after each other, a **char** is only a single character. A char literal is started and ended by a single quotation mark character `'`. As with a string literal, a char literal must be terminated on the line it was started. Additionally, it is a compile time error for a script to contain a character literal with more than one character. The literal has to be a single character or an escape sequence.

## 1.8 Escape sequences

Some characters cannot be represented in a string or char literal. One common example would be a new line. To be able to include one in the string literal, an escape sequence can be used. It is a backslash followed by a special character potentially followed by the Unicode representation of a character.

| Escape sequence        | Corresponding character        |
| ---------------------- | ------------------------------ |
| `\a`                   | Bell (alert)                   |
| `\b`                   | Backspace                      |
| `\f`                   | Form feed                      |
| `\n`                   | New line                       |
| `\r`                   | Carriage return                |
| `\t`                   | Tab                            |
| `\0`                   | Null character                 |
| `\\`                   | Backspace                      |
| `\"`                   | Double quotation mark          |
| `\'`                   | Single quotation mark          |
| `\u[n]`, e.g. `\u004F` | A Unicode character in base 16 |

The base 16 literal has exactly 4 digits, so it has to be "padded" by leading zeroes if the number would not have 4 digits.

It is the compile time error 004 for a string to contain an unescaped backslash without an escape sequence following.

## 1.9 Number literals

There are three types of number literals.

### 1.9.1 Integer literals

An integer literal represents a whole number. It obeys the following rules:

- It consists only of digits 0 to 9, except:
- If the first character is a `0`, then the second character can be a `b` or `x`. This means, that the integer literal is in base 2 (`b` for binary) or 16 (`x` for hexadecimal).
- If the integer literal is in base 16, it also allows the letters a through f. Both lower- and uppercase are allowed, however it is the compile time error 005 if the casing is not consistent (that means that both lowercase and uppercase letters are used).
- No matter the base, an integer literal can contain underscore characters `_`. If it is a binary or hexadecimal integer literal, these underscores are only permitted after the `b` or `x`. More than one underscore is allowed after each other, and trailing underscores are permitted. Integer literals with underscores are exactly equal to the corresponding integer literal without.

These are examples of valid integer literals:

````
1234
1_000_000
0xFF00FF
0b0101011101011010
0xFF_FF_FF
0b010_111_011
````

These are examples of invalid integer literals:

```
0a144552
0x011T91
0_b11010101
```

Invalid in this case does not necessarily mean that a compile time error is reported. The last example for example is split into two lexemes: the integer literal `0` and the identifier `_b11010101`.

### 1.9.2 Rational literals

A rational literal represents a real number. It obeys the following rules:

- It consists only of digits 0 to 9, except:
- It has to contain exactly one dot character `.` to represent the decimal point. If it lacks it, it is not a rational but an integer literal. The dot may not be the first or last character.
- It may also have the underscores as described under <u>1.9.1 Integer literals</u>. However, these underscores are not allowed directly before or directly after the decimal point.

These are examples of valid real literals:

```
3.14159265
3.141_592_65
```

These are examples of invalid real literals, that are also analyzed as something other than a real literal:

```
3_.14159
3.141.59
3.
.5
```

### 1.9.3 Imaginary literals

Imaginary literals are integer or rational literals, if they are immediately followed by an `i`.

For example, `4i` is an imaginary literal.

Only `i` without an integer or rational literal in front of it is an identifier.

## 1.10 Syntax characters

Syntax characters are the punctuation of the language and operators like `+`. They are always analyzed greedily. For example:

```
x += 5;
```

The `+=` is not analyzed as `+` and `=` but as the whole lexeme `+=`. Krypton is lexed greedily.

