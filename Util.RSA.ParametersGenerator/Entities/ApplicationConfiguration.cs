using Module.RSA;
using Util.RSA.ParametersGenerator.Entities.Abstract;

namespace Util.RSA.ParametersGenerator.Entities;

public record ApplicationConfiguration(
    string OutputPath,
    RSAKeyPairGenerationType RSAKeyPairGenerationType,
    int? RandomSeed,
    int AddingTriesCount,
    double PrimalityProbability,
    bool RewriteExistingFiles
) : IApplicationConfiguration;