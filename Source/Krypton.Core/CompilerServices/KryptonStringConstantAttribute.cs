namespace Krypton.Core.CompilerServices
{
    public sealed class KryptonStringConstantAttribute : KryptonConstantAttribute
    {
        public KryptonStringConstantAttribute(string value) : base(ConstantType.String)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public string Value { get; }
    }
}
