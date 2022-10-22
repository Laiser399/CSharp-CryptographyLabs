using Module.DES.Services.Abstract;

namespace Module.DES.Services;

public class BitOperationsService : IBitOperationsService
{
    public byte XorBits(byte value)
    {
        var result = value;
        result = (byte)(result ^ (result >> 4));
        result = (byte)(result ^ (result >> 2));
        result = (byte)(result ^ (result >> 1));

        return (byte)(result & 1);
    }
}