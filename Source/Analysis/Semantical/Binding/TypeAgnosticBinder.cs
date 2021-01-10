using Krypton.Analysis.AbstractSyntaxTree;
using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using System;

namespace Krypton.Analysis.Semantical.Binding
{
    public sealed class TypeAgnosticBinder : IBinder
    {
        public TypeAgnosticBinder(SyntaxTree syntaxTree, BuiltinIdentifierMap builtinIdentifierMap)
        {
            this.syntaxTree = syntaxTree;
            this.builtinIdentifierMap = builtinIdentifierMap;
        }

        private readonly BuiltinIdentifierMap builtinIdentifierMap;
        private readonly SyntaxTree syntaxTree;

        public bool PerformBinding()
        {
            GlobalIdentifierMap globalIdentifierMap = GatherGlobalSymbols();

            // Bind top level statements
            LocalVariableIdentifierMap localVariableIdentifierMap = new();
            if (!BindLocalVariables(syntaxTree.Root.Statements, localVariableIdentifierMap, globalIdentifierMap))
            {
                return false;
            }

            return true;
        }

        private bool BindLocalVariables(StatementCollectionNode statements, LocalVariableIdentifierMap localVariableIdentifierMap, GlobalIdentifierMap globalIdentifierMap)
        {
            foreach (StatementNode statement in statements)
            {
                if (statement is IParentStatementNode psn)
                {
                    switch (statement)
                    {
                        case WhileStatementNode wsn:
                            BindAllNestedNodes(wsn.Condition, localVariableIdentifierMap, globalIdentifierMap);
                            break;
                    }

                    localVariableIdentifierMap.EnterBlock();

                    if (!BindLocalVariables(psn.Statements, localVariableIdentifierMap, globalIdentifierMap))
                    {
                        return false;
                    }

                    localVariableIdentifierMap.LeaveBlock();
                }

                if (!BindAllNestedNodes(statement, localVariableIdentifierMap, globalIdentifierMap))
                {
                    return false;
                }

                if (statement is VariableDeclarationStatementNode declaration)
                {
                    LocalVariableSymbolNode variable = declaration.CreateVariable();
                    localVariableIdentifierMap.AddSymbol(declaration.Identifier, variable);
                }
            }

            return true;
        }

        private bool BindAllNestedNodes(Node statementNode, LocalVariableIdentifierMap localVariableIdentifierMap, GlobalIdentifierMap globalIdentifierMap)
        {
            return Walker.PerformForEach<IBindable>(statementNode, bindable =>
            {
                if (bindable is IdentifierTypeNode itn)
                {
                    // No declared types supported yet
                    SymbolNode? type = builtinIdentifierMap[bindable.IdentifierNode.Identifier];

                    if (type is not TypeSymbolNode tsn)
                    {
                        throw new NotImplementedException("Error ???: Type not found");
                        // return false;
                    }

                    itn.Bind(tsn);
                }

                SymbolNode? symbol = localVariableIdentifierMap[bindable.IdentifierNode.Identifier]
                                  ?? globalIdentifierMap[bindable.IdentifierNode.Identifier]
                                  ?? builtinIdentifierMap[bindable.IdentifierNode.Identifier];

                if (symbol != null)
                {
                    bindable.Bind(symbol);
                }
                else
                {
                    throw new NotImplementedException("Error ???: Symbol not found");
                    // return false;
                }

                return true;
            });
        }

        private GlobalIdentifierMap GatherGlobalSymbols()
        {
            GlobalIdentifierMap map = new();
            //... declared functions are not yet supported, so an empty map is returned for now
            return map;
        }
    }
}
