# 8 Top level code

The top level code is the code that runs when the script starts. It consists of statements. Statements are allowed to be cut off by function declarations. They still belong together and their order of execution exactly resembles the order they appear in code.

## 8.1 Scope

The whole top level code is a scope that coexists with the scope of declared functions. Those functions are not able to access any variables declared in top level code.

## 8.2 `Return`

A `Return` statement in top level code must not have an expression. When it is executed, the execution of the whole script stops.