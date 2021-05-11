namespace Krypton.Core.CompilerServices
{
    public sealed class KryptonIntConstantAttribute : KryptonConstantAttribute
    {
        public KryptonIntConstantAttribute(long value) : base(ConstantType.Int)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public long Value { get; }
    }
}
