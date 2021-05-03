using System.IO;

namespace Krypton.CompilationData
{
    public interface IWritable
    {
        public abstract void WriteCode(TextWriter output);
    }
}
