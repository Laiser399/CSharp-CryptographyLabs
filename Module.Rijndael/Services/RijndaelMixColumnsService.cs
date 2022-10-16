using Module.Rijndael.Services.Abstract;

namespace Module.Rijndael.Services;

public class RijndaelMixColumnsService : IRijndaelMixColumnsService
{
    private static readonly byte[,] MixColumnsMatrix =
    {
        { 0x02, 0x03, 0x01, 0x01 },
        { 0x01, 0x02, 0x03, 0x01 },
        { 0x01, 0x01, 0x02, 0x03 },
        { 0x03, 0x01, 0x01, 0x02 }
    };

    private static readonly byte[,] ReverseMixColumnsMatrix =
    {
        { 0x0E, 0x0B, 0x0D, 0x09 },
        { 0x09, 0x0E, 0x0B, 0x0D },
        { 0x0D, 0x09, 0x0E, 0x0B },
        { 0x0B, 0x0D, 0x09, 0x0E }
    };

    private readonly IGaloisFieldCalculationService _galoisFieldCalculationService;

    public RijndaelMixColumnsService(IGaloisFieldCalculationService galoisFieldCalculationService)
    {
        _galoisFieldCalculationService = galoisFieldCalculationService;
    }

    public void MixColumns(Span<byte> state)
    {
        ValidateArgument(state);
        MixColumns(state, MixColumnsMatrix);
    }

    public void ReverseColumnsMixing(Span<byte> state)
    {
        ValidateArgument(state);
        MixColumns(state, ReverseMixColumnsMatrix);
    }

    private static void ValidateArgument(Span<byte> state)
    {
        if (state.Length == 0)
        {
            throw new ArgumentException("State is empty.", nameof(state));
        }

        if (state.Length % 4 != 0)
        {
            throw new ArgumentException("Invalid state size.", nameof(state));
        }
    }

    private void MixColumns(Span<byte> state, byte[,] matrix)
    {
        var columnsCount = state.Length / 4;
        var buffer = new byte[4];
        for (var column = 0; column < columnsCount; column++)
        {
            for (var targetRow = 0; targetRow < 4; targetRow++)
            {
                buffer[targetRow] = 0;
                for (var sourceRow = 0; sourceRow < 4; sourceRow++)
                {
                    buffer[targetRow] ^= _galoisFieldCalculationService.Multiply(
                        matrix[targetRow, sourceRow],
                        state[sourceRow * columnsCount + column]
                    );
                }
            }

            for (var row = 0; row < 4; row++)
            {
                state[row * columnsCount + column] = buffer[row];
            }
        }
    }
}