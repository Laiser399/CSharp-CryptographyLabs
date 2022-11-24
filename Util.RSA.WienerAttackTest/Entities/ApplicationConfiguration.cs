using Util.RSA.WienerAttackTest.Entities.Abstract;

namespace Util.RSA.WienerAttackTest.Entities;

public record ApplicationConfiguration(
    string InputPath,
    string OutputPath,
    TimeSpan AttackTimeout,
    bool RewriteExistingFiles
) : IApplicationConfiguration;