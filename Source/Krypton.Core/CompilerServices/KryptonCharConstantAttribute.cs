namespace Krypton.Core.CompilerServices
{
    public sealed class KryptonCharConstantAttribute : KryptonConstantAttribute
    {
        public KryptonCharConstantAttribute(char value) : base(ConstantType.Char)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public char Value { get; }
    }
}
