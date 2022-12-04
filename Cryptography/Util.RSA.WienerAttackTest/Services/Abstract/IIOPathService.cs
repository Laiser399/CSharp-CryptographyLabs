using Util.RSA.WienerAttackTest.Entities;

namespace Util.RSA.WienerAttackTest.Services.Abstract;

public interface IIOPathService
{
    bool TryGetInputFileMetaInfo(string inputFilePath, out InputFileMetaInfo metaInfo);

    string GetOutputFilePath(int primesByteSize, int index);
}