using System.Numerics;

namespace Util.RSA.ParametersGenerator.Entities;

public record RsaParameters(
    BigInteger P,
    BigInteger Q,
    BigInteger PublicExponent,
    BigInteger PrivateExponent,
    BigInteger Modulus
);