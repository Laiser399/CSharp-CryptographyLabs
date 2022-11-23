namespace Util.RSA.ParametersGenerator.Entities.Abstract;

public interface IGenerationGroupsConfiguration
{
    IReadOnlyCollection<IGenerationGroupConfiguration> Groups { get; }
}