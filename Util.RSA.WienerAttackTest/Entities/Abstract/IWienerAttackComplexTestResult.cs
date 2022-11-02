namespace Util.RSA.WienerAttackTest.Entities.Abstract;

public interface IWienerAttackComplexTestResult
{
    IReadOnlyCollection<IWienerAttackTestResult> Results { get; }
}