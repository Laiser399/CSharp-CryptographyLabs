namespace Util.RSA.WienerAttackTest.Entities;

public record AttackResult(
    TimeSpan Elapsed,
    int ExponentsCheckCount,
    string? ErrorMessage,
    bool IsUnexpectedResult
);