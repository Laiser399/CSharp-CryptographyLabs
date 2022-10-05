using System;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Autofac;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.RSA.Entities;
using Module.RSA.Entities.Abstract;
using Module.RSA.Services.Abstract;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class PrimesGenerationVM : IPrimesGenerationVM
{
    public IPrimesGenerationParametersVM Parameters { get; }
    public IPrimesGenerationResultsVM Results { get; }

    public bool IsInProgress { get; private set; }
    public string GenerationTextAnimation { get; private set; } = string.Empty;
    public ICommand Generate => _generate ??= new AsyncRelayCommand(_ => GenerateAsync());
    private ICommand? _generate;

    private readonly ILifetimeScope _lifetimeScope;

    public PrimesGenerationVM(
        ILifetimeScope lifetimeScope,
        IPrimesGenerationParametersVM parametersVM,
        IPrimesGenerationResultsVM resultsVM)
    {
        _lifetimeScope = lifetimeScope;
        Parameters = parametersVM;
        Results = resultsVM;
    }

    private async Task GenerateAsync()
    {
        if (IsInProgress)
        {
            return;
        }

        if (Parameters.HasErrors)
        {
            MessageBox.Show("Parameters is not configured correctly.");
            return;
        }

        if (Parameters.IsSaveToFile && !Directory.Exists(Parameters.SaveDirectory))
        {
            MessageBox.Show("Specified saving directory does not exists.");
            return;
        }

        IsInProgress = true;

        var cancellationTokenSource = new CancellationTokenSource();
        var animeTask = StartGenerationAnimation(cancellationTokenSource.Token);

        try
        {
            await GenerateAsync_Internal();
            cancellationTokenSource.Cancel();
            await animeTask;
        }
        finally
        {
            IsInProgress = false;
        }
    }

    private async Task StartGenerationAnimation(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (GenerationTextAnimation.Length < 10)
            {
                GenerationTextAnimation += "-";
            }
            else
            {
                GenerationTextAnimation = string.Empty;
            }

            try
            {
                await Task.Delay(100, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }

        GenerationTextAnimation = string.Empty;
    }

    private async Task GenerateAsync_Internal()
    {
        Results.P = 0;
        Results.Q = 0;

        var parameters = new PrimesPairGeneratorCombinedParameters(
            new Random(Parameters.Seed!.Value),
            Parameters.ByteCount!.Value,
            Parameters.ByteCount!.Value * 8 - 1,
            100,
            Parameters.Probability!.Value
        );

        var registeredParametersLifetimeScope = _lifetimeScope.BeginLifetimeScope(x =>
        {
            x
                .Register(_ => parameters)
                .As<IRandomProvider>()
                .As<IPrimesPairGeneratorParameters>()
                .As<IPrimalityTesterParameters>()
                .SingleInstance();
        });

        var generator = registeredParametersLifetimeScope.Resolve<IPrimesPairGenerator>();
        var (p, q) = await generator.GenerateAsync();

        Results.P = p;
        Results.Q = q;

        SaveToFileIfConfigured(p, q);
    }

    private void SaveToFileIfConfigured(BigInteger p, BigInteger q)
    {
        if (!Parameters.IsSaveToFile)
        {
            return;
        }

        if (!Directory.Exists(Parameters.SaveDirectory))
        {
            MessageBox.Show(
                $"Directory \"{Parameters.SaveDirectory}\" does not exists.",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            return;
        }

        var timeStr = DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss.fff");
        var pFilePath = Path.Combine(Parameters.SaveDirectory, $"{timeStr} p.txt");
        var qFilePath = Path.Combine(Parameters.SaveDirectory, $"{timeStr} q.txt");

        try
        {
            File.WriteAllText(pFilePath, p.ToString());
            File.WriteAllText(qFilePath, q.ToString());
        }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Error save results to files: {e.Message}\n\n" +
                $"{e.StackTrace}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
    }
}