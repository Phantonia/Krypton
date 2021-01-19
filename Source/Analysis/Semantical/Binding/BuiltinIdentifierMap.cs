using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Framework;
using Krypton.Analysis.Utilities;
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
            
            // Placeholder, because type symbols do not yet hold any state like members
            foreach (var type in EnumUtils.GetValues<BuiltinType>())
            {
                AddType(type.ToString(), type);
            }

            AddFunc("Output", BuiltinFunction.Output, returnType: null, ("value", (TypeSymbolNode)builder["String"]));

            builtinIdentifiers = builder.ToImmutable();

            void AddFunc(string name, BuiltinFunction builtinFunction, TypeSpecNode? returnType, params (string id, TypeSymbolNode type)[] parameters)
            {
                builder.Add(name, new BuiltinFunctionSymbolNode(builtinFunction, name, parameters.Select(p => new ParameterNode(p.id, p.type, 0)), returnType, 0));
            }

            void AddType(string name, BuiltinType builtinType)
            {
                builder.Add(name, new BuiltinTypeSymbolNode(builtinType, name, 0));
            }
        }

        public SymbolNode? this[string identifier] => builtinIdentifiers.TryGetValue(identifier, out SymbolNode? symbol) ? symbol : null;

        bool IIdentifierMap.AddSymbol(string identifier, SymbolNode symbol) => throw new NotSupportedException();

        private static readonly ImmutableDictionary<string, SymbolNode> builtinIdentifiers;
    }
}