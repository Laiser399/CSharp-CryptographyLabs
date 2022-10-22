using Module.DES.Entities.Abstract;
using Module.DES.Factories.Abstract;
using Module.DES.Services.Abstract;

namespace Module.DES.Factories;

public class DesKeyFactory : IDesKeyFactory
{
    private readonly IDesRoundKeysGenerator _desRoundKeysGenerator;

    public DesKeyFactory(IDesRoundKeysGenerator desRoundKeysGenerator)
    {
        _desRoundKeysGenerator = desRoundKeysGenerator;
    }

    public IDesKey Create(ulong key56)
    {
        var roundKeys48 = _desRoundKeysGenerator.Generate(key56);

        return new DesKey(roundKeys48);
    }

    private class DesKey : IDesKey
    {
        public IReadOnlyList<ulong> RoundKeys48 { get; }

        public DesKey(IReadOnlyList<ulong> roundKeys48)
        {
            RoundKeys48 = roundKeys48;
        }
    }
}