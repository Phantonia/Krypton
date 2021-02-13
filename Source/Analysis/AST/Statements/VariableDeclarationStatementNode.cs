using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Statements
{
    [DebuggerDisplay("{GetType().Name}; VariableIdentifier = {VariableIdentifier}")]
    public sealed class VariableDeclarationStatementNode : StatementNode
    {
        internal VariableDeclarationStatementNode(IdentifierNode identifier,
                                                  TypeSpecNode? type,
                                                  ExpressionNode? value,
                                                  bool isReadOnly,
                                                  int lineNumber,
                                                  int index) : base(lineNumber, index)
        {
            VariableIdentifierNode = identifier;
            VariableIdentifierNode.ParentNode = this;

            TypeSpecNode = type;
            if (TypeSpecNode != null)
            {
                TypeSpecNode.ParentNode = this;
            }

            AssignedExpressionNode = value;
            if (AssignedExpressionNode != null)
            {
                AssignedExpressionNode.ParentNode = this;
            }


            IsReadOnly = isReadOnly;
        }

        public ExpressionNode? AssignedExpressionNode { get; }

        public bool IsReadOnly { get; }

        public TypeSpecNode? TypeSpecNode { get; }

        public string VariableIdentifier => VariableIdentifierNode.Identifier;

        public IdentifierNode VariableIdentifierNode { get; private set; }

        public VariableSymbolNode? VariableNode => (VariableIdentifierNode as BoundIdentifierNode)?.Symbol as VariableSymbolNode;

        public VariableSymbolNode CreateVariable(TypeSymbolNode? typeSymbol)
        {
            VariableSymbolNode variable = new VariableSymbolNode(VariableIdentifier,
                                                                 typeSymbol,
                                                                 IsReadOnly,
                                                                 VariableIdentifierNode.LineNumber,
                                                                 VariableIdentifierNode.Index);
            VariableIdentifierNode = new BoundIdentifierNode(VariableIdentifier,
                                                             variable,
                                                             VariableIdentifierNode.LineNumber,
                                                             VariableIdentifierNode.Index)
            {
                ParentNode = this
            };
            return variable;
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            VariableIdentifierNode.PopulateBranches(list);
            TypeSpecNode?.PopulateBranches(list);
            AssignedExpressionNode?.PopulateBranches(list);
        }
    }
}
