using System;
using System.Collections.Generic;
using System.Linq;

namespace Krypton.Analysis.AST
{
    public static class Walker
    {
        public static IEnumerable<TNode> Every<TNode>(SyntaxTree syntaxTree, Func<TNode, bool> predicate)
        {
            return syntaxTree.OfType<TNode>().Where(predicate);
        }

        public static IEnumerable<TNode> Every<TNode>(Node node, Func<TNode, bool> predicate)
        {
            return node.GetBranches().OfType<TNode>().Where(predicate);
        }

        public static bool PerformForEach<TNode>(SyntaxTree syntaxTree, Func<TNode, bool> func)
        {
            return PerformForEachWhere(syntaxTree, _ => true, func);
        }

        public static bool PerformForEach<TNode>(Node root, Func<TNode, bool> func)
        {
            return PerformForEachWhere(root, _ => true, func);
        }

        public static bool PerformForEachWhere<TNode>(SyntaxTree syntaxTree, Func<TNode, bool> predicate, Func<TNode, bool> func)
        {
            return PerformForEachWhere(syntaxTree.Root, predicate, func);
        }

        public static bool PerformForEachWhere<TNode>(Node root, Func<TNode, bool> predicate, Func<TNode, bool> func)
        {
            foreach (Node node in root.GetBranches())
            {
                if (node is not TNode specificNode)
                {
                    continue;
                }

                if (!predicate(specificNode))
                {
                    continue;
                }

                bool success = func(specificNode);

                if (!success)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
