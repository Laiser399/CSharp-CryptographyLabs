using Autofac;
using Module.PermutationNetwork.Services;
using Module.PermutationNetwork.Services.Abstract;

namespace Module.PermutationNetwork;

public class PermutationNetworkModule : Autofac.Module
{
    public bool RegisterPermutationMasksCalculator { get; set; }

    protected override void Load(ContainerBuilder builder)
    {
        if (RegisterPermutationMasksCalculator)
        {
            builder
                .RegisterType<PermutationMasksCalculator>()
                .As<IPermutationMasksCalculator>()
                .SingleInstance();
        }

        builder
            .RegisterType<BitPermutationService>()
            .As<IBitPermutationService>()
            .SingleInstance();
    }
}