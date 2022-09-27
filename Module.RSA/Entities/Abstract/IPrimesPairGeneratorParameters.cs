namespace Module.RSA.Entities.Abstract;

public interface IPrimesPairGeneratorParameters
{
    int ByteCount { get; }
    int PQDifferenceMinBitCount { get; }
    int AddingTriesCount { get; }
}