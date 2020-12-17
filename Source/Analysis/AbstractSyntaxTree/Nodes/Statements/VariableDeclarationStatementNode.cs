using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class VariableDeclarationStatementNode : StatementNode
    {
        public VariableDeclarationStatementNode(string variableName, TypeNode? type, ExpressionNode? value, int lineNumber) : base(lineNumber)
        {
            VariableName = variableName;
            Type = type;
            Value = value;
        }

        public TypeNode? Type { get; }

        public ExpressionNode? Value { get; }

        public string VariableName { get; }

        public override StatementNode Clone()
        {
            throw new System.NotImplementedException();
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            throw new System.NotImplementedException();
        }
    }
}
