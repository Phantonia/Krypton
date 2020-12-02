# Branch naming conventions

This is how a name of a branch has to be composed:

```
{Project}[{Part}]{DescribeFeature}
```

Branch names have to be written in PascalCase.

Stuff in `{ }` has to be replaced.

Stuff in `[ ]` may be omitted.

## Project

A prefix to specify the project the new feature is done on.

- Analysis: `Anl`
- CodeGeneration: `Cdg`
- Compiler: `Cmp`
- JsCompilationTemplate: `Jtm`
- Specification: `Spc`

## Part

The part of the project that is modified.

| Analysis                                                     | CodeGeneration               | Compiler                  | JsCompilationTemplate        | Specification                |
| ------------------------------------------------------------ | ---------------------------- | ------------------------- | ---------------------------- | ---------------------------- |
| Lexical analysis: `Lx`<br />Grammatical analysis: `Gr`<br />Semantical analysis: `Sm` | This part has to be omitted. | This part has to omitted. | This part has to be omitted. | This part has to be omitted. |

## Describe feature

Something like "AddFunctions" or "BindIdentifiers".

## Examples

- `AnlLxNewArrowLexeme`
- `AnlGrParseFunctionCalls`
- `AnlSmFixBindingIdentifiersBug`
- `GdgAddIndentation`
- `CmpAddMiniSwitch`
- `JtmModifiedRealType`