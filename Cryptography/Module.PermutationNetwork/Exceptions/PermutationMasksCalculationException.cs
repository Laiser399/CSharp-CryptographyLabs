namespace Module.PermutationNetwork.Exceptions;

public class PermutationMasksCalculationException : Exception
{
    public PermutationMasksCalculationException(string? message) : base(message)
    {
    }

    public PermutationMasksCalculationException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}