using Module.DES.Entities.Abstract;

namespace Module.DES.Factories.Abstract;

public interface IDesKeyFactory
{
    IDesKey Create(ulong key56);
}