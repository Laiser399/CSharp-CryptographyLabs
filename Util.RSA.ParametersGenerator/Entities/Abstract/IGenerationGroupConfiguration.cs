namespace Util.RSA.ParametersGenerator.Entities.Abstract;

public interface IGenerationGroupConfiguration
{
    int ByteSize { get; }
    int Count { get; }

    public void Deconstruct(out int byteSize, out int count)
    {
        byteSize = ByteSize;
        count = Count;
    }
}