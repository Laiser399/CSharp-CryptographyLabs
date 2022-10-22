namespace Module.Rijndael.Services.Abstract;

public interface IGaloisFieldCalculationService
{
    byte Multiply(byte a, byte b);

    byte Inverse(byte a);
}