using Autofac;
using Module.Core;
using Module.Core.Enums;
using Module.Core.Factories.Abstract;
using Module.Core.UnitTests.Tests;
using Module.DES.Entities;
using Module.DES.Entities.Abstract;
using NUnit.Framework;

namespace Module.DES.UnitTests.Tests;

[TestFixture]
public class DesTransformsTests : CryptoTransformsBaseTests<IDesParameters>
{
    private static readonly IDesParameters DesParameters = new DesParameters(
        0b00111100_11011101_10110010_01111001_01011011_00000110_10110110
    );

    private static readonly byte[] InitialVector =
    {
        75, 213, 73, 223, 173, 250, 18, 53,
    };

    private readonly IContainer _container;

    public DesTransformsTests()
    {
        _container = BuildContainer();
    }

    protected override ICryptoTransformFactory<IDesParameters> GetCryptoTransformFactory()
    {
        return _container.Resolve<ICryptoTransformFactory<IDesParameters>>();
    }

    [Test]
    [TestCaseSource(nameof(GetEcbTestCases))]
    public void Transform_EcbTest(
        IDesParameters parameters,
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
        IDesParameters parameters,
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
                DesParameters,
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
                DesParameters,
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
        builder.RegisterModule<DesModule>();

        return builder.Build();
    }
}