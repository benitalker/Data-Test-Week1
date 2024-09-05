using DataWeek1Test.DSNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace DataWeek1Test.BST
{
    public static class BinarySearchTree
    {
        public static DefenceStrategyNode Insert(DefenceStrategyNode? root, DefenceStrategyNode newNode) =>
            root == null ? newNode :
            newNode.MinSeverity < root.MinSeverity
                ? root with { Left = Insert(root.Left, newNode) }
                : root with { Right = Insert(root.Right, newNode) };

        public static void PrintTreeRec(DefenceStrategyNode? node, string indent, bool last, string position)
        {
            if (node == null)
            {
                return;
            }
            Console.Write(indent);
            Console.Write(last ? "└──" : "├──");
            Console.WriteLine($"{position}: [{node.MinSeverity}-{node.MaxSeverity}] Defenses: {string.Join(", ", node.Defenses)}");

            var newIndent = indent + (last ? "   " : "|  ");
            PrintTreeRec(node.Left, newIndent, false, "Left Child");
            PrintTreeRec(node.Right, newIndent, true, "Right Child");
        }

        public static IEnumerable<DefenceStrategyNode> InOrderTraversal(DefenceStrategyNode? root)
        {
            if (root == null)
            {
                yield break;
            }
            foreach (var node in InOrderTraversal(root.Left))
            {
                yield return node;
            }

            yield return root;

            foreach (var node in InOrderTraversal(root.Right))
            {
                yield return node;
            }
        }

        public static void PrintInOrder(DefenceStrategyNode? root)
        {
            PrintInOrderRec(root, "", true);
        }

        private static void PrintInOrderRec(DefenceStrategyNode? node, string indent, bool last)
        {
            if (node == null)
            {
                return;
            }
            var newIndent = indent + (last ? "   " : "|  ");

            PrintInOrderRec(node.Left, indent, false);

            Console.Write(indent);
            Console.Write(last ? "└──" : "├──");
            Console.WriteLine($"[{node.MinSeverity}-{node.MaxSeverity}] Defenses: {string.Join(", ", node.Defenses)}");

            PrintInOrderRec(node.Right, newIndent, true);
        }

        public static IEnumerable<DefenceStrategyNode> PreOrderTraversal(DefenceStrategyNode? root)
        {
            if (root == null)
            {
                yield break;
            }
            yield return root;

            foreach (var node in PreOrderTraversal(root.Left))
            {
                yield return node;
            }
            foreach (var node in PreOrderTraversal(root.Right))
            {
                yield return node;
            }
        }

        public static void PrintPreOrder(DefenceStrategyNode? root)
        {
            PrintPreOrderRec(root, "", true);
        }

        private static void PrintPreOrderRec(DefenceStrategyNode? node, string indent, bool last)
        {
            if (node == null)
            {
                return;
            }
            Console.Write(indent);
            Console.Write(last ? "└──" : "├──");
            Console.WriteLine($"[{node.MinSeverity}-{node.MaxSeverity}] Defenses: {string.Join(", ", node.Defenses)}");

            var newIndent = indent + (last ? "   " : "|  ");
            PrintPreOrderRec(node.Left, newIndent, false);
            PrintPreOrderRec(node.Right, newIndent, true);
        }

        public static void ExecuteDefenses(DefenceStrategyNode? root, int severity)
        {
            if (root == null)
            {
                return;
            }
            if (root.InRange(severity))
            {
                Console.WriteLine($"Executing defenses for severity {severity}: {string.Join(", ", root.Defenses)}");
            }
            ExecuteDefenses(root.Left, severity);
            ExecuteDefenses(root.Right, severity);
        }

        public static DefenceStrategyNode? BuildBalancedTree(IList<DefenceStrategyNode> nodes)
            => BuildBalancedTreeRec(nodes, 0, nodes.Count - 1);

        private static DefenceStrategyNode? BuildBalancedTreeRec(IList<DefenceStrategyNode> nodes, int start, int end)
        {
            if (start > end)
            {
                return null;
            }
            int mid = (start + end) / 2;
            var node = nodes[mid];

            return node with
            {
                Left = BuildBalancedTreeRec(nodes, start, mid - 1),
                Right = BuildBalancedTreeRec(nodes, mid + 1, end)
            };
        }

        public static DefenceStrategyNode? BalanceTree(DefenceStrategyNode root)
            => BuildBalancedTree(InOrderTraversal(root).ToList());

        public static string SerializeTreeToJson(DefenceStrategyNode root)
        {
            var nodes = PreOrderTraversal(root).ToList();
            return JsonSerializer.Serialize(nodes, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        public static DefenceStrategyNode? Remove(DefenceStrategyNode? root, int minSeverity)
        {
            if (root == null) return null;

            if (minSeverity < root.MinSeverity)
            {
                root = root with { Left = Remove(root.Left, minSeverity) };
            }
            else if (minSeverity > root.MinSeverity)
            {
                root = root with { Right = Remove(root.Right, minSeverity) };
            }
            else
            {
                // Case 1: Node has no children (leaf)
                if (root.Left == null && root.Right == null)
                {
                    return null;
                }

                // Case 2: Node has one child
                if (root.Left == null)
                {
                    return root.Right;
                }
                if (root.Right == null)
                {
                    return root.Left;
                }

                // Case 3: Node has two children
                var successor = FindMin(root.Right);
                root = root with
                {
                    MinSeverity = successor.MinSeverity,
                    MaxSeverity = successor.MaxSeverity,
                    Defenses = successor.Defenses,
                    Right = Remove(root.Right, successor.MinSeverity)
                };
            }

            return root;
        }

        private static DefenceStrategyNode FindMin(DefenceStrategyNode node)
        {
            while (node.Left != null)
            {
                node = node.Left;
            }
            return node;
        }
    }
}
