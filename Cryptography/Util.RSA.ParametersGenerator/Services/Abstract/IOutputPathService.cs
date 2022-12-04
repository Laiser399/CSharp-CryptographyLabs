namespace Util.RSA.ParametersGenerator.Services.Abstract;

public interface IOutputPathService
{
    string GetOutputFilePath(int byteSize, int index);
}