namespace Util.RSA.WienerAttackTest.Entities.Abstract;

public interface IWienerAttackTestResult
{
    int ByteCount { get; }
    int AttackCount { get; }
    double AverageExponentsCheckCount { get; }
    int MinExponentsCheckCount { get; }
    int MaxExponentsCheckCount { get; }
    
    int MissCount { get; }
    int ErrorCount { get; }
}