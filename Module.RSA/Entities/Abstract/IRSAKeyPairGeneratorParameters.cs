namespace Module.RSA.Entities.Abstract;

public interface IRSAKeyPairGeneratorParameters
{
    bool ForceWienerAttackVulnerability { get; }
}