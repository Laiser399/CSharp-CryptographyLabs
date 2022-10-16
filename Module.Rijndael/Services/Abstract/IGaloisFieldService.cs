namespace Module.Rijndael.Services.Abstract;

public interface IGaloisFieldService
{
    /// <summary>
    /// Determines whether this value can be generating element for GF(2^8)
    /// </summary>
    bool IsGeneratingElement(ushort value);
}