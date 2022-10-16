namespace Module.Rijndael.Services.Abstract;

public interface IRijndaelSubstitutionService
{
    void SubstituteBytes(Span<byte> state);

    void SubstituteBytesInversed(Span<byte> state);
}