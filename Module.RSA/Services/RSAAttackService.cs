using System.Numerics;
using Module.RSA.Entities;
using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class RSAAttackService : IRSAAttackService
{
    public Task<FactorizationResult> FactorizeModulusAsync(BigInteger modulus, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }
}