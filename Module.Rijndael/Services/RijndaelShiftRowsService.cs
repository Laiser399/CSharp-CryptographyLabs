using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class RijndaelShiftRowsService : IRijndaelShiftRowsService
{
    public void ShiftRows(Span<byte> state)
    {
        ValidateState(state);

        var columnsCount = state.Length / 4;
        switch (columnsCount)
        {
            case 4:
                ShiftRowsWith4Columns(state);
                break;
            case 6:
                ShiftRowsWith6Columns(state);
                break;
            case 8:
                ShiftRowsWith8Columns(state);
                break;
            default:
                throw new ArgumentException("Invalid state size.", nameof(state));
        }
    }

    public void InverseShiftRows(Span<byte> state)
    {
        ValidateState(state);

        var columnsCount = state.Length / 4;
        switch (columnsCount)
        {
            case 4:
                InverseShiftRowsWith4Columns(state);
                break;
            case 6:
                InverseShiftRowsWith6Columns(state);
                break;
            case 8:
                InverseShiftRowsWith8Columns(state);
                break;
            default:
                throw new ArgumentException("Invalid state size.", nameof(state));
        }
    }

    private static void ValidateState(Span<byte> state)
    {
        if (state.Length % 4 != 0)
        {
            throw new ArgumentException("State size is not multiple of 4.", nameof(state));
        }
    }

    private static unsafe void ShiftRowsWith4Columns(Span<byte> state)
    {
        fixed (byte* statePtr = state)
        {
            var stateUIntPtr = (uint*)statePtr;

            for (var i = 1; i < 4; i++)
            {
                stateUIntPtr[i] = (stateUIntPtr[i] >> (i * 8)) | (stateUIntPtr[i] << ((sizeof(uint) - i) * 8));
            }
        }
    }

    private static unsafe void InverseShiftRowsWith4Columns(Span<byte> state)
    {
        fixed (byte* statePtr = state)
        {
            var stateUIntPtr = (uint*)statePtr;

            for (var i = 1; i < 4; i++)
            {
                stateUIntPtr[i] = (stateUIntPtr[i] << (i * 8)) | (stateUIntPtr[i] >> ((sizeof(uint) - i) * 8));
            }
        }
    }

    private static unsafe void ShiftRowsWith6Columns(Span<byte> state)
    {
        for (var i = 1; i < 3; i++)
        {
            var row = state.Slice(i * 6, 6);
            fixed (byte* rowPtr = row)
            {
                var rowULongPtr = (ulong*)rowPtr;
                var shiftedRow =
                    ((*rowULongPtr & 0xffff_ffffffff) >> (i * 8))
                    | (*rowULongPtr << ((6 - i) * 8)) & 0xffff_ffffffff;
                *rowULongPtr &= 0xffff0000_00000000;
                *rowULongPtr |= shiftedRow;
            }
        }

        var lastRow = state.Slice(3 * 6, 6);

        // Сдвиг последней строки вынесен отдельно, т.к. в случае многопоточности заход на чужую территорию приводит к приколам.
        // А на чужую территорию мы заходим, т.к. в последней строке 6 байт, а длина ulong - 8 байт.
        (lastRow[0], lastRow[1], lastRow[2], lastRow[3], lastRow[4], lastRow[5]) =
            (lastRow[3], lastRow[4], lastRow[5], lastRow[0], lastRow[1], lastRow[2]);
    }

    private static unsafe void InverseShiftRowsWith6Columns(Span<byte> state)
    {
        for (var i = 1; i < 3; i++)
        {
            var row = state.Slice(i * 6, 6);
            fixed (byte* rowPtr = row)
            {
                var rowULongPtr = (ulong*)rowPtr;
                var shiftedRow =
                    (*rowULongPtr << (i * 8)) & 0xffff_ffffffff
                    | ((*rowULongPtr & 0xffff_ffffffff) >> ((6 - i) * 8));
                *rowULongPtr &= 0xffff0000_00000000;
                *rowULongPtr |= shiftedRow;
            }
        }

        var lastRow = state.Slice(3 * 6, 6);

        // см. выше
        (lastRow[0], lastRow[1], lastRow[2], lastRow[3], lastRow[4], lastRow[5]) =
            (lastRow[3], lastRow[4], lastRow[5], lastRow[0], lastRow[1], lastRow[2]);
    }

    private static unsafe void ShiftRowsWith8Columns(Span<byte> state)
    {
        fixed (byte* statePtr = state)
        {
            var stateULongPtr = (ulong*)statePtr;

            for (var i = 1; i < 4; i++)
            {
                stateULongPtr[i] = (stateULongPtr[i] >> (i * 8)) | (stateULongPtr[i] << ((sizeof(ulong) - i) * 8));
            }
        }
    }

    private static unsafe void InverseShiftRowsWith8Columns(Span<byte> state)
    {
        fixed (byte* statePtr = state)
        {
            var stateULongPtr = (ulong*)statePtr;

            for (var i = 1; i < 4; i++)
            {
                stateULongPtr[i] = (stateULongPtr[i] << (i * 8)) | (stateULongPtr[i] >> ((sizeof(ulong) - i) * 8));
            }
        }
    }
}