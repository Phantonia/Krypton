using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Symbols;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Statements
{
    [DebuggerDisplay("{GetType().Name}; VariableIdentifier = {VariableIdentifier}")]
    public sealed class VariableAssignmentStatementNode : StatementNode
    {
        internal VariableAssignmentStatementNode(IdentifierNode identifier, ExpressionNode assignedValue, int lineNumber, int index) : base(lineNumber, index)
        {
            VariableIdentifierNode = identifier;
            VariableIdentifierNode.ParentNode = this;
            AssignedExpressionNode = assignedValue;
            AssignedExpressionNode.ParentNode = this;
        }

        public ExpressionNode AssignedExpressionNode { get; }

        public VariableSymbolNode VariableNode
        {
            get
            {
                VariableSymbolNode? variable = (VariableIdentifierNode as BoundIdentifierNode)?.Symbol as VariableSymbolNode;
                Debug.Assert(variable != null);
                return variable;
            }
        }

        public string VariableIdentifier => VariableIdentifierNode.Identifier;

        public IdentifierNode VariableIdentifierNode { get; private set; }

        public void Bind(VariableSymbolNode symbol)
        {
            VariableIdentifierNode = new BoundIdentifierNode(VariableIdentifier, symbol, VariableIdentifierNode.LineNumber, VariableIdentifierNode.LineNumber)
            {
                ParentNode = this
            };
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            VariableIdentifierNode.PopulateBranches(list);
            AssignedExpressionNode.PopulateBranches(list);
        }
    }
}
