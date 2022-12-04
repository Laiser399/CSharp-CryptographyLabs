using Module.RSA.Entities.Abstract;
using Module.RSA.Services.Abstract;

namespace Util.RSA.WienerAttackTest.Entities;

public class WienerAttackStatistics : IWienerAttackStatistics, IWienerAttackStatisticsCollector
{
    public int ExponentsCheckedCount { get; private set; }

    public void IncreaseExponentsCheckedCount()
    {
        ExponentsCheckedCount++;
    }
}