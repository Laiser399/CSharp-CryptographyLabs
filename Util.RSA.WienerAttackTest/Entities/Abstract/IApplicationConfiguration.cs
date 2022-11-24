namespace Util.RSA.WienerAttackTest.Entities.Abstract;

public interface IApplicationConfiguration
{
    string InputPath { get; }
    string OutputPath { get; }

    TimeSpan AttackTimeout { get; }

    bool RewriteExistingFiles { get; }
}