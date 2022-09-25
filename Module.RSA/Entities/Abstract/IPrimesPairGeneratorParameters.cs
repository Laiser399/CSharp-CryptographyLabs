namespace Module.RSA.Entities.Abstract;

public interface IPrimesPairGeneratorParameters
{
    int ByteCount { get; }
    double PrimalityProbability { get; }
    int PQDifferenceMinBitCount { get; }
    int StepTriesCount { get; }
}