namespace Module.Rijndael.Services.Abstract;

public interface IRijndaelMixColumnsService
{
    void MixColumns(Span<byte> state);

    void ReverseColumnsMixing(Span<byte> state);
}