using Microsoft.Extensions.Configuration;
using Util.RSA.ParametersGenerator.Entities.Abstract;
using Util.RSA.ParametersGenerator.Exceptions;

namespace Util.RSA.ParametersGenerator.Entities;

public class GenerationGroupsConfiguration : IGenerationGroupsConfiguration
{
    private const string SectionName = "GenerationGroups";

    public IReadOnlyCollection<IGenerationGroupConfiguration> Groups { get; }

    public GenerationGroupsConfiguration(IConfiguration configuration)
    {
        var section = configuration.GetSection(SectionName);

        Groups = section.Get<GenerationGroupConfiguration[]>()
                 ?? throw new ApplicationStartupException(
                     $"Could not read \"{SectionName}\" section from configuration."
                 );
    }
}