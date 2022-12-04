using Module.RSA.Entities.Abstract;

namespace Module.RSA.Entities;

public class RandomProvider : IRandomProvider
{
    public Random Random { get; }

    public RandomProvider(Random random)
    {
        Random = random;
    }
}