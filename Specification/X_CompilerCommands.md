# X Compiler commands

Keywords in commands are case insensitive, so `Compile` and `compile` are interchangeable.

## Compile

### Description

Generates code in the given language that is equivalent to the script and saves it to the given file.

### Syntax

```
Compile As {Language} To {File}
```

### Arguments

| Argument | Meaning                                                      |
| -------- | ------------------------------------------------------------ |
| Language | The language to compile to. At this point, you can only compile to Javascript (`Js`). |
| File     | A valid file name relative to the script file being compiled. |

### Example

```
Compile As Js To script.js
```

## Reanalyse

### Description

Does the analysis again. Changes to the file in the meantime are reflected.

### Syntax

```
Reanalyse
```

