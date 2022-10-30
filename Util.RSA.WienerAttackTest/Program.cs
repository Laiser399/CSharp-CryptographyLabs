using System.Text.Json;
using Autofac;
using Module.RSA.Entities;
using Module.RSA.Entities.Abstract;
using Util.RSA.WienerAttackTest;
using Util.RSA.WienerAttackTest.Services.Abstract;

const int attackCount = 1000;
const string saveDirectory = "out";

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

var wienerAttackTestService = scope.Resolve<IWienerAttackTestService>();

var byteCounts = Enumerable.Range(1, 2)
    .Select(x => x * 16)
    .ToList();

var tasks = byteCounts
    .Select(x => wienerAttackTestService.PerformTestAsync(x, attackCount))
    .ToList();

Console.WriteLine("Tasks started.");
Console.WriteLine($"Byte counts: {string.Join(", ", byteCounts)}.");

foreach (var task in tasks)
{
    var result = await task;
    var filePath = Path.Combine(saveDirectory, $"result_{result.ByteCount}_{result.AttackCount}.json");
    var serialized = JsonSerializer.Serialize(result);
    File.WriteAllText(filePath, serialized);

    Console.WriteLine($"Task with ByteCount={result.ByteCount} done. Result saved to \"{filePath}\".");
}