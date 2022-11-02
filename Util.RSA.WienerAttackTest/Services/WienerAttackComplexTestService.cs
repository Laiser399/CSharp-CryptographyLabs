using Util.RSA.WienerAttackTest.Entities;
using Util.RSA.WienerAttackTest.Entities.Abstract;
using Util.RSA.WienerAttackTest.Services.Abstract;

namespace Util.RSA.WienerAttackTest.Services;

public class WienerAttackComplexTestService : IWienerAttackComplexTestService
{
    private readonly IWienerAttackTestServiceParameters _parameters;
    private readonly IWienerAttackTestService _wienerAttackTestService;

    public WienerAttackComplexTestService(
        IWienerAttackTestServiceParameters parameters,
        IWienerAttackTestService wienerAttackTestService)
    {
        _parameters = parameters;
        _wienerAttackTestService = wienerAttackTestService;
    }

    public async Task<IWienerAttackComplexTestResult> PerformComplexTestAsync()
    {
        var results = new List<IWienerAttackTestResult>();
        foreach (var byteCount in _parameters.ByteCounts)
        {
            var result = await _wienerAttackTestService.PerformTestAsync(byteCount, _parameters.AttackCount);
            results.Add(result);
        }

        return new WienerAttackComplexTestResult(results);
    }
}