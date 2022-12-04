namespace Module.DES.Entities.Abstract;

public interface IDesKey
{
    IReadOnlyList<ulong> RoundKeys48 { get; }
}