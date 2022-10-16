using Module.Rijndael.Enums;

namespace Module.Rijndael.Services.Abstract;

public interface IRijndaelRoundCountCalculator
{
    int GetRoundCount(RijndaelSize blockSize, RijndaelSize keySize);
}