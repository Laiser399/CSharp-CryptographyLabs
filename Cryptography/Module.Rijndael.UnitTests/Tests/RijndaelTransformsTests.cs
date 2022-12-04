using Autofac;
using Module.Core;
using Module.Core.Enums;
using Module.Core.Factories.Abstract;
using Module.Core.UnitTests.Tests;
using Module.Rijndael.Entities;
using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;
using NUnit.Framework;

namespace Module.Rijndael.UnitTests.Tests;

[TestFixture]
public class RijndaelTransformsTests : CryptoTransformsBaseTests<IRijndaelParameters>
{
    private static readonly IRijndaelParameters RijndaelParameters = new RijndaelParameters(
        new byte[]
        {
            93, 230, 245, 204, 216, 129,
            66, 132, 92, 63, 19, 244,
            227, 157, 177, 22, 49, 56,
            50, 142, 121, 181, 247, 18
        },
        RijndaelSize.S192
    );

    private static readonly byte[] InitialVector =
    {
        249, 215, 150, 208, 191, 44,
        178, 243, 120, 245, 137, 203,
        164, 21, 225, 170, 222, 19,
        75, 28, 217, 208, 125, 192
    };

    private readonly IContainer _container;

    public RijndaelTransformsTests()
    {
        _container = BuildContainer();
    }

    protected override ICryptoTransformFactory<IRijndaelParameters> GetCryptoTransformFactory()
    {
        return _container.Resolve<ICryptoTransformFactory<IRijndaelParameters>>();
    }

    [Test]
    [TestCaseSource(nameof(GetEcbTestCases))]
    public void Transform_EcbTest(
        IRijndaelParameters parameters,
        bool withParallelism,
        int blockCount,
        int hangingByteCount)
    {
        TestEcbTransform(
            parameters,
            withParallelism,
            blockCount,
            hangingByteCount
        );
    }

    [Test]
    [TestCaseSource(nameof(GetTestCases))]
    public void Transform_Test(
        IRijndaelParameters parameters,
        BlockCipherMode mode,
        byte[] initialVector,
        int blockCount,
        int hangingByteCount)
    {
        TestTransform(
            parameters,
            mode,
            initialVector,
            blockCount,
            hangingByteCount
        );
    }

    private static IReadOnlyCollection<object[]> GetEcbTestCases()
    {
        var testCases =
            from blockCount in BlockCountCases
            from hangingByteCount in HangingByteCountCases
            from withParallelism in new[] { true, false }
            where blockCount != 0 || hangingByteCount >= 0
            select new object[]
            {
                RijndaelParameters,
                withParallelism,
                blockCount,
                hangingByteCount
            };

        return testCases.ToList();
    }

    private static IReadOnlyCollection<object[]> GetTestCases()
    {
        var testCases =
            from mode in ModeCases
            from blockCount in BlockCountCases
            from hangingByteCount in HangingByteCountCases
            where blockCount != 0 || hangingByteCount >= 0
            select new object[]
            {
                RijndaelParameters,
                mode,
                InitialVector,
                blockCount,
                hangingByteCount
            };

        return testCases.ToList();
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