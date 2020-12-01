using Krypton.Analysis.Lexical.Lexemes;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Krypton.Analysis.Lexical
{
    public sealed class LexemeCollection : Collection<Lexeme>
    {
        public LexemeCollection() : base() { }

        // This ensure during development, that after sealing the collection no changes are made.
        // At production this does not matter anymore as ideally it doesn't happen.
#if DEBUG
        private bool open = true;
#else
        private const bool open = true;
#endif

        public void Seal(int lineNumber)
        {
            Debug.Assert(open);
            Add(new EndOfFileLexeme(lineNumber));
#if DEBUG
            open = false;
#endif
        }

#if DEBUG
        protected override void ClearItems()
        {
            Debug.Assert(open);
            base.ClearItems();
        }

        protected override void InsertItem(int index, Lexeme item)
        {
            Debug.Assert(open);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Debug.Assert(open);
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Lexeme item)
        {
            Debug.Assert(open);
            base.SetItem(index, item);
        }
#endif
    }
}
