using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Statements
{
    public sealed class VariableDeclarationStatementNode : StatementNode
    {
        public VariableDeclarationStatementNode(IdentifierNode identifier, TypeSpecNode? type, ExpressionNode? value, int lineNumber) : base(lineNumber)
        {
            VariableIdentifierNode = identifier;
            VariableIdentifierNode.Parent = this;

            Type = type;
            if (Type != null)
            {
                Type.Parent = this;
            }

            Value = value;
            if (Value != null)
            {
                Value.Parent = this;
            }
        }

        public string Identifier => VariableIdentifierNode.Identifier;

        public IdentifierNode VariableIdentifierNode { get; private set; }

        public TypeSpecNode? Type { get; }

        public ExpressionNode? Value { get; }

        public LocalVariableSymbolNode CreateVariable(TypeSymbolNode? typeSymbol)
        {
            LocalVariableSymbolNode var = new LocalVariableSymbolNode(Identifier, typeSymbol, VariableIdentifierNode.LineNumber);
            VariableIdentifierNode = new BoundIdentifierNode(Identifier, var, VariableIdentifierNode.LineNumber) { Parent = this };
            return var;
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            VariableIdentifierNode.PopulateBranches(list);
            Type?.PopulateBranches(list);
            Value?.PopulateBranches(list);
        }
    }
}
