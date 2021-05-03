namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class SyntaxCharacterToken : Token
    {
        public SyntaxCharacterToken(SyntaxCharacter syntaxCharacter, int lineNumber, Trivia leadingTrivia)
            : base(lineNumber, leadingTrivia)
        {
            SyntaxCharacter = syntaxCharacter;
        }

        public SyntaxCharacter SyntaxCharacter { get; }

        public override string Text => SyntaxCharacter.ToText();
    }
}
