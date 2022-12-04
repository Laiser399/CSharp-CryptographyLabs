namespace Module.DES.Services.Abstract;

public interface IDesRoundKeysGenerator
{
    /// <summary>
    /// Генерирует 16 48-ми битных ключей.
    /// </summary>
    ulong[] Generate(ulong key56);
}