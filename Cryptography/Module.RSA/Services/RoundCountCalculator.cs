using Module.RSA.Services.Abstract;

namespace Module.RSA.Services;

public class RoundCountCalculator : IRoundCountCalculator
{
    public int GetRoundCount(double probability, double singleRoundWrongResultProbability)
    {
        ThrowOnInvalidProbability(probability);

        var roundCount = 1;
        var cumulativeWrongResultProbability = singleRoundWrongResultProbability;
        while (1 - probability < cumulativeWrongResultProbability)
        {
            roundCount++;
            cumulativeWrongResultProbability *= singleRoundWrongResultProbability;
        }

        return roundCount;
    }

    private static void ThrowOnInvalidProbability(double probability)
    {
        if (probability is <= 0 or >= 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(probability),
                probability,
                "Probability must be in range (0; 1)."
            );
        }
    }
}