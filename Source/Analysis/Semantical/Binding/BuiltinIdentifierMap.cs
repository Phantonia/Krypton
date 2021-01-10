using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using Krypton.Analysis.Framework;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Krypton.Analysis.Semantical.Binding
{
    public sealed class BuiltinIdentifierMap : IIdentifierMap
    {
        public BuiltinIdentifierMap() : base() { }

        static BuiltinIdentifierMap()
        {
            var builder = ImmutableDictionary.CreateBuilder<string, SymbolNode>();
            AddFunc("Output", BuiltinFunction.Output, returnType: null, ("value", null!)); // TODO: Deal with type (String)
            builtinIdentifiers = builder.ToImmutable();

            void AddFunc(string name, BuiltinFunction builtinFunction, TypeNode? returnType, params (string id, TypeNode type)[] parameters)
            {
                builder.Add(name, new BuiltinFunctionSymbolNode(builtinFunction, name, parameters.Select(p => new ParameterNode(p.id, p.type, 0)), returnType, 0));
            }
        }

        public SymbolNode? this[string identifier] => builtinIdentifiers.TryGetValue(identifier, out SymbolNode? symbol) ? symbol : null;

        bool IIdentifierMap.AddSymbol(string identifier, SymbolNode symbol) => throw new NotSupportedException();

        private static readonly ImmutableDictionary<string, SymbolNode> builtinIdentifiers;
    }
}