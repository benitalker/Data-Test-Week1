using System.Collections.Generic;
using System.Collections.Immutable;

namespace DataWeek1Test.DSNode
{
    public record DefenceStrategyNode
    {
        public int MinSeverity { get; init; }
        public int MaxSeverity { get; init; }
        public ImmutableList<string> Defenses { get; init; } = ImmutableList<string>.Empty;
        public DefenceStrategyNode? Left { get; init; }
        public DefenceStrategyNode? Right { get; init; }

        public bool InRange(int severity) =>
            severity >= MinSeverity && severity <= MaxSeverity;
    }
}
