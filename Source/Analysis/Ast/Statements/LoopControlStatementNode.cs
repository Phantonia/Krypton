using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Statements
{
    public sealed class LoopControlStatementNode : StatementNode
    {
        internal LoopControlStatementNode(ushort level, LoopControlStatementKind kind, int lineNumber, int index) : base(lineNumber, index)
        {
            Level = level;
            Kind = kind;
        }

        public ILoopStatementNode? ControlledLoopNode { get; private set; }

        public LoopControlStatementKind Kind { get; }

        public ushort Level { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }

        public void SetControlledLoop(ILoopStatementNode? loop)
        {
            Debug.Assert(ControlledLoopNode == null);
            ControlledLoopNode = loop;
        }
    }
}
