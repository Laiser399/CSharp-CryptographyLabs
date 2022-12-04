namespace Module.Rijndael.Services.Abstract;

public interface IRijndaelShiftRowsService
{
    void ShiftRows(Span<byte> state);

    void InverseShiftRows(Span<byte> state);
}