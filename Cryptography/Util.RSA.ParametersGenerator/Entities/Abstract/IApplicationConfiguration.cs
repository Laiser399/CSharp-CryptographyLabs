using Module.RSA;

namespace Util.RSA.ParametersGenerator.Entities.Abstract;

public interface IApplicationConfiguration
{
    string OutputPath { get; }

    RSAKeyPairGenerationType RSAKeyPairGenerationType { get; }
    int? RandomSeed { get; }
    int AddingTriesCount { get; }
    double PrimalityProbability { get; }

    bool RewriteExistingFiles { get; }
}