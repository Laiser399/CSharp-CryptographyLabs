using Module.DES.Entities.Abstract;

namespace Module.DES.Entities;

public class DesParameters : IDesParameters
{
    public ulong Key56 { get; }

    public DesParameters(ulong key56)
    {
        Key56 = key56;
    }
}