namespace Module.Core.Services.Abstract;

public interface IXorService
{
    /// <summary>
    /// Выполняет операцию симметрической разности над first и second и сохраняет результат в result
    /// </summary>
    void Xor(Span<byte> first, Span<byte> second, Span<byte> result);
}