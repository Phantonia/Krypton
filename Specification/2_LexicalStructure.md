# 2 Lexical structure

## 2.1 Case

Scripts are entirely case sensitive. That means, that an identifier `Something` and an identifier `something` (note the different casing of the "S") do not mean the same thing and can coexist. Casing also affects keywords. For example, the keyword `Var` is used for declaring a variable and due to being a reserved keyword, it cannot be used as and identifier, while `var` is a valid identifier.

## 2.2 Whitespace

For the most part, scripts are whitespace insensitive. That means, that no whitespace affects the meaning of a script. For example, code can have any number of blank lines, and can be indented with any number of tabs or spaces.

For example, all three of these scripts are legal and mean the same thing:

```
Out "What's your name?";
Var name = Input();
If name.Length > 10
{
	Out "Oh, you have a long name!";
}
Out "Hello " + name;
```

```
Out"What's your name?";Var name=Input();If name.Length>10{Out"Oh, you have a long name!";}Out"Hello "+name;
```

```
Out "What's your name?";
	Var name = Input();
		If name.Length > 10
			{
        		Out "Oh, you have a long name!";
					}
    
    
    
Out "Hello " + name;
```

The only situation in which a whitespace character changes the meaning of a program is between two identifiers, two reserved keywords or one identifier and one keyword. Consider the following situation:

```
Var x As String;
VarxAsString;
```

The first one declares a variable of datatype `String` whereas the second one is a compile time error because a single identifier can never be used as a whole *statement*.

## 2.3 Comments

A comment is a portion of code that is ignored after lexical analysis.

There are three kinds of comments:

### 2.3.1 Single line comments

A single line comment is opened using three dot characters `.` and does not have to be closed as it is considered closed at the end of the line.

```
Out "Hello world!"; ... this is a comment
```

###  2.3.2 Weak multiline comment

A weak multiline comment is opened using two consecutive greater than characters `>` and closed using two consecutive less than characters `<`. Because of this syntax, it is possible for a weak multiline comment to span multiple lines. However, there is no requirement for that.

A weak multiline comment works like a strict multiline comment with the exception of the balancing rule, which does not apply to weak multiline comments. Thus, this comment is properly closed:

```
>> This is a comment >> It is closed there: <<
```

As you can see, weak multiline comments only use two greater/less than signs.

### 2.3.3 Strict multiline comment

A strict multiline comment works like a weak multiline comment, except that it uses three consecutive characters for the starting and the ending lexeme.

```
Out "Hello world!"; >>> This is a comment <<<
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

It is discouraged to use this kind of comment with a text editor that doesn't support correct syntax highlighting for this kind of comment.

Both kinds of comments ignore the starting and ending lexemes of the other kind. That means, that this strict multiline comment is closed:

```
>>> Strict >> something <<<
```

And these comments aren't:

```
>>> Comment <<
>> Comment <<<
```

## 2.4 Identifiers

Identifiers are strings of characters that obey the following rules:

- The first character is a letter or an underscore
- The second to last character are all letters, underscores and/or numbers
- The identifier as a whole does not equal a reserved keyword (case sensitive)
- The identifier is not a single underscore

These are examples of valid identifiers: `PascalCase`, `camelCase`, `snake_case`, `SCREAMING_SNAKE_CASE`
These are examples of invalid identifiers: `3D`, `$abc`, `_`

## 2.5 Reserved keywords

Reserved keywords are strings of letters with a special meaning in the language. A full list of reserved keywords can be found in the file "ReservedKeywords.csv".

## 2.6 String literals

A **string** is a string of characters. A string literal is a hardcoded string in the script itself. It starts and ends with a double quotation mark character `"`. It is a compile time error to start a string literal but not end it on the same line.

The backslash character `\` however can stop the string literal from ending. If the user intended to add a quotation mark to their string literal, they can escape it by prepending it with the backslash. A backslash itself also has to be escaped if the intention is to add one to the string literal.

Unescaped quotation marks and backslashes do not appear in the final string.

An example for a valid string literal: `"This is a string!"`

An example for an invalid string literal: `"This is not a string`

## 2.7 Character literals

While a string represents several characters one after each other, a **char** is only a single character. A char literal is started and ended by a single quotation mark character `'`. As with a string literal, a char literal must be terminated on the line it was started. Additionally, it is a compile time error for a script to contain a character literal with more than one character. The literal has to be a single character or an escape sequence.

## 2.8 Escape sequences

Some characters cannot be represented in a string or char literal. One common example would be a new line. To be able to include one in the string literal, an escape sequence can be used. It is a backslash followed by a special character potentially followed by the Unicode representation of a character.

| Escape sequence             | Corresponding character        |
| --------------------------- | ------------------------------ |
| `\a`                        | Bell (alert)                   |
| `\f`                        | Form feed                      |
| `\n`                        | New line                       |
| `\r`                        | Carriage return                |
| `\s`                        | Backspace                      |
| `\t`                        | Tab                            |
| `\0`                        | Null character                 |
| `\\`                        | Backspace                      |
| `\"`                        | Double quotation mark          |
| `\'`                        | Single quotation mark          |
| `\d[n];`, e.g. `\d41;`      | A Unicode character in base 10 |
| `\x[n];`, e.g. `\x4F;`      | A Unicode character in base 16 |
| `\b[n];`, e.g. `\b0010110;` | A Unicode character in base 2  |

The last three represent any Unicode character. `[n]` is replaced with the number of the character in the base defined by the character `x`, `d` or `b`. The number is then followed by a semicolon.

It is a compile time error for a string to contain an unescaped backslash without an escape sequence following.

## 2.9 Number literals

There are three types of number literals.

### 2.9.1 Integer literals

An integer literal represents a whole number. It obeys the following rules:

- It consists only of digits 0 to 9, except:
- If the first character is a `0`, then the second character can be a `b` or `x`. This means, that the integer literal is in base 2 (`b` for binary) or 16 (`x` for hexadecimal).
- If the integer literal is in base 16, it also allows the letters a through f. Casing doesn't matter, however it is a compile time error if the casing is not consistent (that means that lowercase and uppercase letters are mixed).
- No matter the base, an integer literal can contain underscore characters `_`. If it is a binary or hexadecimal integer literal, these underscores are only permitted after the `b` or `x`. More than one underscore is allowed after each other, and trailing underscores are permitted. These underscores are ignored after lexical analysis and only aid the readability of large numbers.

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

### 2.9.2 Real literals

A real literal represents a real number. It obeys the following rules:

- It consists only of digits 0 to 9, except:
- It has to contain exactly one dot character `.` to represent the decimal point. If it lacks it, it is not a real but an integer literal. The dot may not be the first or last character.
- It may also have the underscores as described under Integer literals. However, these underscores are not allow directly before or directly after the decimal point.

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

### 2.9.3 Imaginary literals

Imaginary literals are any integer or real literals, if they are immediately followed by an `i`.

For example, `4i` is an imaginary literal.

To get the number i itself, the imaginary literal `1i` has to be used.

## 2.10 Syntax characters

Syntax characters are the punctuation of the language and operators like `+`. They are always analyzed greedily. For example, take this snippet of code:

```vb
x += 5;
```

The `+=` is not analyzed as `+` and `=` but as the whole lexeme `+=`. A general rule of lexical analysis in Krypton is, that the lexer always tries to form the longest possible lexeme.

