using Autofac;
using Module.Core;
using Module.Core.Factories.Abstract;
using Module.Core.UnitTests.Tests;
using Module.Rijndael.Entities;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;
using NUnit.Framework;

namespace Module.Rijndael.UnitTests.Tests;

[TestFixture]
public class RijndaelBlockCryptoTransformTests : BlockCryptoTransformBaseTests<IRijndaelParameters>
{
    private readonly IContainer _container;

    public RijndaelBlockCryptoTransformTests()
    {
        _container = BuildContainer();
    }

    [Test]
    [TestCaseSource(nameof(GetTransformTestCases))]
    public void Transform_Test(IRijndaelParameters parameters)
    {
        TestTransform(parameters);
    }

    private static IReadOnlyCollection<IRijndaelParameters> GetTransformTestCases()
    {
        var keysBytes = new[]
        {
            new byte[]
            {
                220, 98, 68, 222,
                197, 55, 127, 233,
                53, 242, 68, 32,
                91, 119, 175, 95
            },
            new byte[]
            {
                93, 230, 245, 204, 216, 129,
                66, 132, 92, 63, 19, 244,
                227, 157, 177, 22, 49, 56,
                50, 142, 121, 181, 247, 18
            },
            new byte[]
            {
                130, 237, 235, 12, 213, 170, 49, 57,
                87, 227, 107, 115, 51, 119, 87, 130,
                246, 189, 57, 255, 113, 65, 74, 72,
                244, 66, 191, 162, 9, 233, 234, 1
            }
        };

        var blockSizes = new[]
        {
            RijndaelSize.S128,
            RijndaelSize.S192,
            RijndaelSize.S256
        };

        var testCases =
            from keyBytes in keysBytes
            from blockSize in blockSizes
            select new RijndaelParameters(keyBytes, blockSize);

        return testCases.ToList();
    }

    protected override IBlockCryptoTransformFactory<IRijndaelParameters> GetBlockCryptoTransformFactory()
    {
        return _container.Resolve<IBlockCryptoTransformFactory<IRijndaelParameters>>();
    }

    protected override IRijndaelParameters GetDefaultParameters()
    {
        var keyBytes = new byte[]
        {
            220, 98, 68, 222,
            197, 55, 127, 233,
            53, 242, 68, 32,
            91, 119, 175, 95
        };
        var blockSize = RijndaelSize.S128;
        return new RijndaelParameters(keyBytes, blockSize);
    }

    private static IContainer BuildContainer()
    {
        var builder = new ContainerBuilder();

        builder.RegisterModule<CoreModule>();
        builder.RegisterModule(new RijndaelModule
        {
            UseDefaultGaloisFieldConfiguration = true
        });

        return builder.Build();
    }
}