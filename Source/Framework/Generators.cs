using System.Collections.Generic;

namespace Krypton.Framework
{
    public delegate string BinaryGenerator(string expressionX, string expressionY);
    public delegate string FunctionCallGenerator(IList<string> expressions);
    public delegate string LiteralGenerator<T>(T literal);
    public delegate string UnaryGenerator(string expression);
}
