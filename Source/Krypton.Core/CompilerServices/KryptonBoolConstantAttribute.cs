namespace Krypton.Core.CompilerServices
{
    public sealed class KryptonBoolConstantAttribute : KryptonConstantAttribute
    {
        public KryptonBoolConstantAttribute(bool value) : base(ConstantType.Bool)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public bool Value { get; }
    }
}
