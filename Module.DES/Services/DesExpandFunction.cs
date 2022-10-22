using Module.DES.Services.Abstract;

namespace Module.DES.Services;

public class DesExpandFunction : IDesExpandFunction
{
    public ulong Calculate(uint value)
    {
        ulong result = 0;

        value = (value << 5) | (value >> 27);
        for (var i = 0; i < 8; ++i)
        {
            result = (result << 6) | (value & 0b111111);

            value = (value << 4) | (value >> 28);
        }

        return result;
    }
}