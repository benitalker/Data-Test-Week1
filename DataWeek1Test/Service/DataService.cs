using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using DataWeek1Test.BST;
using DataWeek1Test.D_A;
using DataWeek1Test.DSNode;

namespace DataWeek1Test.Service
{
    public class DataService
    {
        public static async Task ProcessThreatsAsync(IEnumerable<Threat> threats, DefenceStrategyNode defenseTree)
        {
            await threats.Aggregate(Task.CompletedTask, async (previousTask, threat) =>
            {
                await previousTask;

                int severity = threat.CalculateSeverity();
                Console.WriteLine($"Processing Threat: {threat}");
                Console.WriteLine($"Calculated Severity: {severity}");
                if (severity < GetMinSeverity(defenseTree))
                {
                    Console.WriteLine("Attack severity is below the threshold. Attack is ignored.");
                }
                else
                {
                    bool executed = ExecuteDefenses(defenseTree, severity);
                    if (!executed)
                    {
                        Console.WriteLine("No suitable defense was found. Brace for impact.");
                    }
                }
                await Task.Delay(2000);
            });
        }

        private static int GetMinSeverity(DefenceStrategyNode? root)
        {
            if (root == null)
            {
                return int.MaxValue;
            }
            int minSeverity = root.MinSeverity;
            minSeverity = Math.Min(minSeverity, GetMinSeverity(root.Left));
            minSeverity = Math.Min(minSeverity, GetMinSeverity(root.Right));
            return minSeverity;
        }

        private static bool ExecuteDefenses(DefenceStrategyNode? root, int severity)
        {
            if (root == null)
            {
                return false;
            }
            bool executed = false;

            if (root.InRange(severity))
            {
                Console.WriteLine($"Executing defenses for severity {severity}: {string.Join(", ", root.Defenses)}");
                executed = true;
            }

            executed |= ExecuteDefenses(root.Left, severity);
            executed |= ExecuteDefenses(root.Right, severity);

            return executed;
        }

        public static IEnumerable<Threat> LoadThreatsFromJsonAsync(string filePath)
            => ReadFromJsonAsync<IEnumerable<Threat>>(filePath) ?? Enumerable.Empty<Threat>();

        public static DefenceStrategyNode? BuildDefenseTree(string filePath)
            => (ReadFromJsonAsync<IEnumerable<DefenceStrategyNode>>(filePath) ?? Enumerable.Empty<DefenceStrategyNode>())
                .Aggregate(new DefenceStrategyNode(), BinarySearchTree.Insert);

        private static T ReadFromJsonAsync<T>(string filePath)
        {
            try
            {
                var data = JsonSerializer.Deserialize<T>(
                    File.OpenRead(filePath),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        IncludeFields = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    }
                );
                if (data == null)
                {
                    return default!;
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from {filePath}: {ex.Message}");
                return default!;
            }
        }
    }
}
