using Util.RSA.WienerAttackTest.Entities.Abstract;

namespace Util.RSA.WienerAttackTest.Entities;

public class WienerAttackComplexTestResult : IWienerAttackComplexTestResult
{
    public IReadOnlyCollection<IWienerAttackTestResult> Results { get; }

    public WienerAttackComplexTestResult(IReadOnlyCollection<IWienerAttackTestResult> results)
    {
        Results = results;
    }
}