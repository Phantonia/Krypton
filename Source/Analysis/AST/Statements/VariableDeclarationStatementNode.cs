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
        internal VariableDeclarationStatementNode(IdentifierNode identifier, TypeSpecNode? type, ExpressionNode? value, int lineNumber, int index) : base(lineNumber, index)
        {
            VariableIdentifierNode = identifier;
            VariableIdentifierNode.ParentNode = this;

            Type = type;
            if (Type != null)
            {
                Type.ParentNode = this;
            }

            AssignedValueNode = value;
            if (AssignedValueNode != null)
            {
                AssignedValueNode.ParentNode = this;
            }
        }

        public ExpressionNode? AssignedValueNode { get; }

        public LocalVariableSymbolNode VariableNode
        {
            get
            {
                LocalVariableSymbolNode? localVariable = (VariableIdentifierNode as BoundIdentifierNode)?.Symbol as LocalVariableSymbolNode;
                Debug.Assert(localVariable != null);
                return localVariable;
            }
        }

        public string VariableIdentifier => VariableIdentifierNode.Identifier;

        public IdentifierNode VariableIdentifierNode { get; private set; }

        public TypeSpecNode? Type { get; }

        public LocalVariableSymbolNode CreateVariable(TypeSymbolNode? typeSymbol)
        {
            LocalVariableSymbolNode variable = new LocalVariableSymbolNode(VariableIdentifier,
                                                                           typeSymbol,
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
            Type?.PopulateBranches(list);
            AssignedValueNode?.PopulateBranches(list);
        }
    }
}
