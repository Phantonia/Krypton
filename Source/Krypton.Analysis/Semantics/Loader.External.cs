//using Krypton.CompilationData.Symbols;
//using Krypton.Core.CompilerServices;
//using Mono.Cecil;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;

//namespace Krypton.Analysis.Semantics
//{
//    partial class Loader
//    {
//        private void AddFunctionsFromAssembly(AssemblyDefinition assembly, List<KeyValuePair<string, Symbol>> symbols)
//        {
//            TypeReference namespaceSymbolAttribute = assembly.MainModule.ImportReference(typeof(KryptonNamespaceSymbolAttribute));

//            IEnumerable<IMemberDefinition> typesWithTopLevelFunctions = assembly.MainModule.Types
//                                                                                         .Where(IsNamespaceSymbolClass)
//                                                                                         .SelectMany(type => type.Methods.Concat<IMemberDefinition>(type.Fields)
//                                                                                                                         .Concat(type.Properties))
//                                                                                         .Where(memb => memb.)

//            static bool IsNamespaceSymbol(IMemberDefinition member)
//            {
//                return member.CustomAttributes.Any(attr => attr.AttributeType.)
//            }

//            static bool IsNamespaceSymbolClass(TypeDefinition type)
//            {
//                if (!type.IsClass)
//                {
//                    return false;
//                }

//                return type.CustomAttributes.Any(attr => attr.AttributeType.FullName == typeof(KryptonNamespaceSymbolsClassAttribute).FullName);
//            }
//        }

//        private void AddTypesFromAssembly(AssemblyDefinition assembly, List<KeyValuePair<string, Symbol>> symbols)
//        {
//            IEnumerable<TypeReference> applicableTypes = assembly.MainModule
//                                                                 .Types
//                                                                 .Where(TypeIsNotBanned);

//            foreach (TypeReference type in applicableTypes)
//            {
//                ExternalTypeSymbol symbol = CreateTypeSymbol(type);
//                symbols.Add(KeyValuePair.Create(symbol.Name, (Symbol)symbol));
//            }

//            static bool TypeIsNotBanned(TypeDefinition type)
//            {
//                return !type.CustomAttributes.Any(attr => attr.AttributeType.FullName == typeof(KryptonCompilerBanAttribute).FullName);
//            }
//        }

//        private ExternalImplicitConversionSymbol CreateImplicitConversionSymbol(MethodReference conversion)
//        {
//            Debug.Assert(IsImplicitConversion(conversion));

//            ExternalTypeSymbol targetType = externalTypes[conversion.ReturnType];

//            return new ExternalImplicitConversionSymbol(conversion, targetType);
//        }

//        private ExternalPropertySymbol CreatePropertySymbol(PropertyReference property)
//        {
//            ExternalTypeSymbol returnType = externalTypes[property.DeclaringType];

//            return new ExternalPropertySymbol(property, returnType);
//        }

//        private ExternalTypeSymbol CreateTypeSymbol(TypeReference type)
//        {
//            TypeDefinition definition = type.Resolve();

//            IEnumerable<ExternalImplicitConversionSymbol> conversions
//                = definition.Methods
//                            .Where(meth => IsImplicitConversion(meth))
//                            .Select(conv => CreateImplicitConversionSymbol(conv));

//            IEnumerable<ExternalPropertySymbol> properties
//                = definition.Properties
//                            .Select(prop => CreatePropertySymbol(prop));

//            ExternalTypeSymbol typeSymbol = new(type, conversions, properties);

//            externalTypes.Add(type, typeSymbol);

//            return typeSymbol;
//        }

//        private bool IsImplicitConversion(MethodReference method)
//        {
//            MethodDefinition definition = method.Resolve();

//            if (method.Name != SpecialNames.ImplicitConversion)
//            {
//                return false;
//            }

//            if ((definition.Attributes & MethodAttributes.Static) != MethodAttributes.Static)
//            {
//                return false;
//            }

//            if ((definition.Attributes & MethodAttributes.SpecialName) != MethodAttributes.SpecialName)
//            {
//                return false;
//            }

//            return method.Parameters.Count == 1;
//        }
//    }
//}
