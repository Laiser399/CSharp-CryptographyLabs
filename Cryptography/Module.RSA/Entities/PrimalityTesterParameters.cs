using Module.RSA.Entities.Abstract;

namespace Module.RSA.Entities;

public class PrimalityTesterParameters : IPrimalityTesterParameters
{
    public double PrimalityProbability { get; }

    public PrimalityTesterParameters(double primalityProbability)
    {
        PrimalityProbability = primalityProbability;
    }
}