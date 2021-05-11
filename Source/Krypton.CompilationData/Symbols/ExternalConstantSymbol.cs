using Mono.Cecil;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Krypton.CompilationData.Symbols
{
    public sealed class ExternalConstantSymbol<TValue> : ConstantSymbol<TValue>, IExternalSymbol
        where TValue : notnull
    {
        public ExternalConstantSymbol(FieldDefinition constant,
                                      ExternalTypeSymbol type,
                                      TValue value)
            : base(constant.Name, value)
        {
            // field must be constant
            AssertValidConst(constant);

            ConstantReference = constant;
            TypeSymbol = type;

            [Conditional("DEBUG")]
            static void AssertValidConst(FieldReference field)
            {
                if ((field.Resolve().Attributes & FieldAttributes.Literal) == FieldAttributes.Literal)
                {
                    // constant
                    return;
                }

                if ((field.Resolve().Attributes & FieldAttributes.InitOnly) == FieldAttributes.InitOnly
                 && (field.Resolve().Attributes & FieldAttributes.Static) == FieldAttributes.Static)
                {
                    // static readonly field

                    foreach (CustomAttribute attr in field.Resolve().CustomAttributes)
                    {
                        string attrFullName = attr.AttributeType.FullName;

                        if (Regex.IsMatch(attrFullName, "Krypton(Int|Rational(Complex)?|Bool|Char|String)ConstantAttribute"))
                        {
                            return;
                        }
                    }
                }

                Debug.Fail(message: null);
            }
        }

        public FieldDefinition ConstantReference { get; }

        public override ExternalTypeSymbol TypeSymbol { get; }

        MemberReference IExternalSymbol.Reference => ConstantReference;
    }
}
