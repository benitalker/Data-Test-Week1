using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DataWeek1Test.BST;
using DataWeek1Test.DSNode;
using DataWeek1Test.Service;

namespace DataWeek1Test
{
    public static class Program
    {
        private const string DefenseStrategyPath = "../../../Data/defenceStrategies.json";
        private const string ThreatsPath = "../../../Data/threats.json";

        public static async Task Main(string[] args)
        {
            try
            {
                // 1. Build the defense tree from the JSON file
                var defenseTree = DataService.BuildDefenseTree(DefenseStrategyPath);
                if (defenseTree == null)
                {
                    Console.WriteLine("Failed to build the defense tree from the JSON file.");
                    return;
                }

                // 2. Print the initial tree using In-Order Traversal
                Console.WriteLine("Initial Tree (In-Order):");
                BinarySearchTree.PrintInOrder(defenseTree);

                // 3. Balance the tree
                defenseTree = BinarySearchTree.BalanceTree(defenseTree) ?? new DefenceStrategyNode();

                // 4. Print the balanced tree using Pre-Order Traversal
                Console.WriteLine("Balanced Binary Search Tree (Pre-Order):");
                BinarySearchTree.PrintPreOrder(defenseTree);

                // 5. Print the balanced tree using In-Order Traversal
                Console.WriteLine("Balanced Tree (In-Order):");
                BinarySearchTree.PrintInOrder(defenseTree);

                // 6. Test Remove functionality
                Console.WriteLine("\nTesting Remove Functionality:");

                // Remove a node with specific MinSeverity (e.g., 27) from the tree
                defenseTree = BinarySearchTree.Remove(defenseTree, 27) ?? new DefenceStrategyNode();
                Console.WriteLine("\nTree after removing node with MinSeverity 27 (In-Order):");
                BinarySearchTree.PrintPreOrder(defenseTree);

                // Remove another node with specific MinSeverity (e.g., 5)
                defenseTree = BinarySearchTree.Remove(defenseTree, 5) ?? new DefenceStrategyNode();
                Console.WriteLine("\nTree after removing node with MinSeverity 5 (In-Order):");
                BinarySearchTree.PrintPreOrder(defenseTree);

                // 7. Load threats from the JSON file
                var threats = DataService.LoadThreatsFromJsonAsync(ThreatsPath);
                if (threats == null)
                {
                    Console.WriteLine("Failed to load threats from the JSON file.");
                    return;
                }

                // 8. Process each threat with the balanced defense tree
                await DataService.ProcessThreatsAsync(threats, defenseTree);

                // 9. Serialize the balanced tree to JSON
                string jsonOutput = BinarySearchTree.SerializeTreeToJson(defenseTree);
                await File.WriteAllTextAsync("../../../Data/balancedDefenceStrategies.json", jsonOutput);
                Console.WriteLine("Balanced tree serialized to JSON.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
