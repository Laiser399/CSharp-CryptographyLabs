using Module.Rijndael.Enums;
using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class RijndaelRoundCountCalculator : IRijndaelRoundCountCalculator
{
    public int GetRoundCount(RijndaelSize blockSize, RijndaelSize keySize)
    {
        if (blockSize == RijndaelSize.S128 && keySize == RijndaelSize.S128)
        {
            return 10;
        }

        if (blockSize == RijndaelSize.S256 || keySize == RijndaelSize.S256)
        {
            return 14;
        }

        return 12;
    }
}