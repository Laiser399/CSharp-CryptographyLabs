using Autofac;
using Util.RSA.WienerAttackTest;
using Util.RSA.WienerAttackTest.Services.Abstract;

var container = AppContainer.Build();

var attackService = container.Resolve<IAttackService>();

await attackService.PerformAttacksAsync();