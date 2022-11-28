namespace Module.Rijndael.Services.Abstract;

public interface IGaloisFieldService
{
    /// <summary>
    /// Calculates generating elements for GF(2^8)
    /// </summary>
    /// <returns></returns>
    IReadOnlyCollection<ushort> CalculateGeneratingElements();

    /// <summary>
    /// Determines whether this value can be generating element for GF(2^8)
    /// </summary>
    bool IsGeneratingElement(ushort value);
}