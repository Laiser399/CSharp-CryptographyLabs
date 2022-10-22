using Autofac;
using Module.Core.Factories.Abstract;
using Module.Core.UnitTests.Tests;
using Module.DES.Entities;
using Module.DES.Entities.Abstract;
using Module.PermutationNetwork;
using NUnit.Framework;

namespace Module.DES.UnitTests.Tests;

[TestFixture]
public class DesBlockCryptoTransformTests : BlockCryptoTransformBaseTests<IDesParameters>
{
    private readonly IContainer _container;

    public DesBlockCryptoTransformTests()
    {
        _container = BuildContainer();
    }

    [Test]
    public void Transform_Test()
    {
        TestTransform(GetDefaultParameters());
    }

    protected override IBlockCryptoTransformFactory<IDesParameters> GetBlockCryptoTransformFactory()
    {
        return _container.Resolve<IBlockCryptoTransformFactory<IDesParameters>>();
    }

    protected override IDesParameters GetDefaultParameters()
    {
        return new DesParameters(0b00111100_11011101_10110010_01111001_01011011_00000110_10110110);
    }

    private static IContainer BuildContainer()
    {
        var builder = new ContainerBuilder();

        builder.RegisterModule<DesModule>();
        builder.RegisterModule<PermutationNetworkModule>();

        return builder.Build();
    }
}