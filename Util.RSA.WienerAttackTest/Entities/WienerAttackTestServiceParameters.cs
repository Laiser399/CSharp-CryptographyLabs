using Util.RSA.WienerAttackTest.Entities.Abstract;

namespace Util.RSA.WienerAttackTest.Entities;

public class WienerAttackTestServiceParameters : IWienerAttackTestServiceParameters
{
    public IEnumerable<int> ByteCounts { get; }
    public int AttackCount { get; }

    public WienerAttackTestServiceParameters(IEnumerable<int> byteCounts, int attackCount)
    {
        ByteCounts = byteCounts;
        AttackCount = attackCount;
    }
}