namespace Module.RSA.Entities.Abstract;

public interface IRandomProvider
{
    Random Random { get; }
}