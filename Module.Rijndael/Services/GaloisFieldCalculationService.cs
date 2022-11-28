using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Factories;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class GaloisFieldCalculationService : IGaloisFieldCalculationService
{
    private readonly IGaloisFieldConfiguration _configuration;

    public GaloisFieldCalculationService(IGaloisFieldConfiguration? configuration = null)
    {
        _configuration = configuration ?? GaloisFieldConfigurationFactory.DefaultConfiguration;
    }

    public byte Multiply(byte a, byte b)
    {
        ushort result = 0;
        ushort bExt = b;
        while (a > 0)
        {
            if ((a & 1) == 1)
            {
                result ^= bExt;
            }

            a >>= 1;
            bExt <<= 1;
            if ((bExt & 0x100) != 0)
            {
                bExt ^= _configuration.IrreduciblePolynomial;
            }
        }

        return (byte)result;
    }

    public byte Inverse(byte a)
    {
        return Pow(a, 254);
    }

    private byte Pow(byte a, byte exponent)
    {
        var result = (byte)1;
        var degree = a;
        while (exponent > 0)
        {
            if ((exponent & 1) == 1)
            {
                result = Multiply(result, degree);
            }

            degree = Multiply(degree, degree);
            exponent >>= 1;
        }

        return result;
    }
}