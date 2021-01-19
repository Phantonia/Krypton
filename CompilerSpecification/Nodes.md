# Nodes

A node is a part of the abstract syntax tree. Each node has exactly one **parent** and a variable number of **children**. Children, children's children, etc. are called **ancestors**.

An example:

```
  X
 / \
Y   Z
   / \
  A   B
```

This is a very simple tree where X is the parent of both Y and Z, and Y and Z are X's children. Y, Z, A and B are all ancestors of X.

## The class `Node`

The class `Node` is the base class of every node, so it provides the functionality that each node is capable of.

- The public property `LineNumber`, which represents the line number of the first lexeme that makes up this node (for error reporting purposes)
- The public property `Parent`, which represents the parent of this node as specified above, or `null`, if this node is the root
- The public method `GetChildren`, which returns a list of children of this node
- The public method `GetAncestors`, which returns a list of ancestors
- The internal methods `PopulateChildren` and `PopulateAncestors`, which add children or ancestors respectively to a list of nodes.

A class can only be derived from `Node` if it is in the `Krypton.Analysis` assembly.

## The class `ProgramNode`

The class `ProgramNode` represents a whole Krypton program and the root of a syntax tree.