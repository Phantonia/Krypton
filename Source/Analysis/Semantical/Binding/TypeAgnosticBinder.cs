using Krypton.Analysis.AST;
using Krypton.Analysis.AST.Statements;
using Krypton.Analysis.AST.Symbols;
using Krypton.Analysis.AST.TypeSpecs;
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
            if (!BindLocalVariables(syntaxTree.Root.TopLevelStatements, localVariableIdentifierMap, globalIdentifierMap))
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
                    TypeSymbolNode? typeSymbol = null;

                    if (declaration.Type != null)
                    {
                        typeSymbol = BindType(declaration.Type, localVariableIdentifierMap, globalIdentifierMap);

                        if (typeSymbol == null)
                        {
                            return false;
                        }
                    }

                    LocalVariableSymbolNode variable = declaration.CreateVariable(typeSymbol);
                    localVariableIdentifierMap.AddSymbol(declaration.Identifier, variable);
                }
            }

            return true;
        }

        private bool BindAllNestedNodes(Node statementNode, LocalVariableIdentifierMap localVariableIdentifierMap, GlobalIdentifierMap globalIdentifierMap)
        {
            return Walker.PerformForEach<IBindable>(statementNode, bindable =>
            {
                if (bindable is IdentifierTypeSpecNode itn)
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

                SymbolNode? symbol = FindSymbol(bindable.IdentifierNode.Identifier, localVariableIdentifierMap, globalIdentifierMap);

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

        private TypeSymbolNode? BindType(TypeSpecNode typeNode, LocalVariableIdentifierMap localVariableIdentifierMap, GlobalIdentifierMap globalIdentifierMap)
        {
            if (typeNode is not IdentifierTypeSpecNode idType)
            {
                throw new NotImplementedException("Not implemented: other type expressions");
            }

            SymbolNode? maybeTypeSymbol = FindSymbol(idType.Identifier, localVariableIdentifierMap, globalIdentifierMap);

            if (maybeTypeSymbol is not TypeSymbolNode typeSymbol)
            {
                throw new NotImplementedException("Error ???: Not type used as type");
            }

            idType.Bind(typeSymbol);

            return typeSymbol;
        }
        
        private SymbolNode? FindSymbol(string identifier, LocalVariableIdentifierMap localVariableIdentifierMap, GlobalIdentifierMap globalIdentifierMap)
        {
            return localVariableIdentifierMap[identifier]
                ?? globalIdentifierMap[identifier]
                ?? builtinIdentifierMap[identifier];
        }

        private GlobalIdentifierMap GatherGlobalSymbols()
        {
            GlobalIdentifierMap map = new();
            //... declared functions are not yet supported, so an empty map is returned for now
            return map;
        }
    }
}
