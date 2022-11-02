using Util.RSA.WienerAttackTest.Entities.Abstract;

namespace Util.RSA.WienerAttackTest.Entities;

public class WienerAttackTestResult : IWienerAttackTestResult
{
    public int ByteCount { get; init; }
    public int AttackCount { get; init; }

    public double AverageExponentsCheckCount { get; set; }
    public int MinExponentsCheckCount { get; set; }
    public int MaxExponentsCheckCount { get; set; }

    public int MissCount { get; set; }
    public int ErrorCount { get; set; }
}