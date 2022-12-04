namespace Module.Core.Services.Abstract;

public interface IXorService
{
    /// <summary>
    /// Выполняет операцию симметрической разности над first и second и сохраняет результат в result
    /// </summary>
    /// <exception cref="ArgumentException">Lengths of arguments are not equal</exception>
    void Xor(ReadOnlySpan<byte> first, ReadOnlySpan<byte> second, Span<byte> result);
}