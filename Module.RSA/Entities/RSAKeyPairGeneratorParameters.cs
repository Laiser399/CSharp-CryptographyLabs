using Module.RSA.Entities.Abstract;

namespace Module.RSA.Entities;

public class RSAKeyPairGeneratorParameters : IRSAKeyPairGeneratorParameters
{
    public bool ForceWienerAttackVulnerability { get; }

    public RSAKeyPairGeneratorParameters(bool forceWienerAttackVulnerability)
    {
        ForceWienerAttackVulnerability = forceWienerAttackVulnerability;
    }
}