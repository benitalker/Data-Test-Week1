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
            // 1. Build the defense tree from the JSON file
            var defenseTree = DataService.BuildDefenseTreeAsync(DefenseStrategyPath) ?? new DefenceStrategyNode();

            // 2. Print the initial tree using In-Order Traversal
            Console.WriteLine("Initial Tree (In-Order):");
            BinarySearchTree.PrintInOrder(defenseTree);

            // 3. Balance the tree
            defenseTree = BinarySearchTree.BalanceTree(defenseTree) ?? new DefenceStrategyNode();

            // 4. Print the balanced tree using Pre-Order Traversal
            Console.WriteLine("Balanced Binary Searched Tree (Pre-Order):");
            BinarySearchTree.PrintPreOrder(defenseTree);

            // 5. Print the balanced tree using In-Order Traversal
            Console.WriteLine("Balanced Tree (In-Order):");
            BinarySearchTree.PrintInOrder(defenseTree);

            // 6. Load threats from the JSON file
            var threats = DataService.LoadThreatsFromJsonAsync(ThreatsPath);

            // 7. Process each threat with the balanced defense tree
            await DataService.ProcessThreatsAsync(threats, defenseTree);

            // 8. Serialize the balanced tree to JSON
            string jsonOutput = BinarySearchTree.SerializeTreeToJson(defenseTree);
            File.WriteAllText("../../../Data/balancedDefenceStrategies.json", jsonOutput);
            Console.WriteLine("Balanced tree serialized to JSON.");
        }
    }
}
