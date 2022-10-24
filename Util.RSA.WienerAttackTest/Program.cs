using Autofac;
using Module.RSA.Entities;
using Module.RSA.Entities.Abstract;
using Util.RSA.WienerAttackTest;
using Util.RSA.WienerAttackTest.Entities;
using Util.RSA.WienerAttackTest.Entities.Abstract;
using Util.RSA.WienerAttackTest.Services.Abstract;

var container = AppContainer.Build();

var scope = container.BeginLifetimeScope(builder =>
{
    builder
        .RegisterInstance(new RandomProvider(new Random()))
        .As<IRandomProvider>();
    builder
        .RegisterInstance(new PrimalityTesterParameters(0.999))
        .As<IPrimalityTesterParameters>();
});

var parameters = new WienerAttackTestServiceParameters(
    Enumerable.Range(1, 8).Select(x => x * 16),
    10
);

var wienerAttackTestService = scope.Resolve<IWienerAttackTestService>(
    new TypedParameter(typeof(IWienerAttackTestServiceParameters), parameters)
);

var results = await wienerAttackTestService.PerformComplexTestAsync();