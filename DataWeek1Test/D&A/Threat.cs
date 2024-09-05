using System;

namespace DataWeek1Test.D_A
{
    public class Threat
    {
        public string ThreatType { get; init; } = string.Empty;
        public int Volume { get; init; }
        public int Sophistication { get; init; }
        public string Target { get; init; } = string.Empty;

        public int CalculateSeverity() =>
            (Volume * Sophistication) + GetTargetValue(Target);

        private static int GetTargetValue(string target) => target switch
        {
            "Web Server" => 10,
            "Database" => 15,
            "User Credentials" => 20,
            _ => 5
        };

        public override string ToString() =>
            $"ThreatType: {ThreatType}, Volume: {Volume}, Sophistication: {Sophistication}, Target: {Target}";
    }
}
