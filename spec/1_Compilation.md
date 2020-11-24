# Krypton compilation

The process of taking a single Krypton script file (a file whose content obeys the syntactical and semantical rules of the Krypton specification and normally but not necessarily the file extension `.krpt`) and outputting code in a different language with exactly the same semantics as the ones describe in the Krypton script is called **compilation**. Normally, a Krypton file is compiled to Javascript. The exact semantics of this translation is described in the section <u>Javascript output</u>.

Compilation happens in four mainly but not necessarily distinct steps:

1. **Lexical analysis**
2. **Grammatical analysis**
3. **Semantical analysis**
4. **Code generation**

## Lexical analysis

The input to the compiler is plain text. To make sense of it, it first needs to break this text down into atomic units of language, called **lexemes** (or tokens, though this specification and the source code of the compiler only use the word "lexeme"). More information on the exact process and the concrete lexical structure of a valid Krypton program is given in section 2: <u>Lexical structure</u>.

## Grammatical analysis

After having a list of all lexemes of the script, the compiler attempts to build an **Abstract Syntax Tree**. It concretely describes the grammar of the script.

## Semantical analysis

"Semantical analysis" describes the process of making sense of the structure of the source file. Examples of tasks done in this step are:

- **Identifier binding**, so finding out what *entity* the *identifier* refer to
- **Datatype checking**, so checking whether an *expression* is only doing *operations* on *values* of *datatypes* that support these operations, and checking whether a value that is said to be of a datatype actually is of that datatype

## Code generation

During this step the compiler uses all the knowledge about the script that it gained during the previous steps to generate code of a different language (for example Javascript) that does precisely what the user intended with the Krypton script. In the section <u>Javascript output</u> the exact way each syntax construct is compiled is explained.