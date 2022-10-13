using System.Numerics;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class WienerAttackService : IRSAAttackService
{
    public Task<BigInteger> AttackAsync(
        BigInteger publicExponent,
        BigInteger modulus,
        CancellationToken? cancellationToken = null)
    {
        throw new NotImplementedException();
    }
}