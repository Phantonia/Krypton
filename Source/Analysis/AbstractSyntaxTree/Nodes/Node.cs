using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes
{
    /* A node is a part of the abstract syntax tree.
     * Each node also saves the exact line in which
     * it occurred in the original code. This is to
     * make error messages better. Most nodes logically
     * consist of multiple lexemes, so the line number
     * will be the line number of the first lexemes
     * that makes up the node (for example, the line
     * number of an IfStatementNode is the line number
     * of the "If" keyword). The first line is 1. If
     * the line number is 0, then the node is synthesised
     * by the compiler or it representes a builtin type,
     * function or constant.
     * As the node is part of a tree, it is recursive in
     * that every node may save references to other nodes.
     * These are called branches of the node.
     * The parent of a node is the single node that owns
     * a reference to this node in the tree. Some nodes
     * like BoundIdentifierNode point across the tree
     * to another node. These nodes are not considered
     * parent of the other node. The textual equivalence
     * of the parent is seen close to the textual equivalence
     * of the node in the original code. This does not
     * apply to identifier that point across the code.
     *   A node is able to clone itself. This means that
     * a new instance of the node type is created. Every
     * branch of the node is cloned as well and put in
     * place of the old reference as the branch of the
     * cloned node. This rule also applies to the cloning
     * of the branches.
     */
    [DebuggerDisplay("{GetType().Name} -- {GetHashCode()}")]
    public abstract class Node : INode
    {
        protected private Node(int lineNumber)
        {
            LineNumber = lineNumber;
        }

        public int LineNumber { get; }

        public Node? Parent { get; internal set; }

        //public abstract Node Clone();

        public List<Node> GetBranches()
        {
            List<Node> branches = new();
            PopulateBranches(branches);
            return branches;
        }

        public abstract void PopulateBranches(List<Node> list);
    }
}
