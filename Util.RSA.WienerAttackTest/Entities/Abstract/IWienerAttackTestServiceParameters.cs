namespace Util.RSA.WienerAttackTest.Entities.Abstract;

public interface IWienerAttackTestServiceParameters
{
    IEnumerable<int> ByteCounts { get; }
    int AttackCount { get; }
}