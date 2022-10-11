using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.RSA.Exceptions;
using Module.RSA.Services.Abstract;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class RSAFactorizationAttackVM : IRSAFactorizationAttackVM
{
    public IRSAFactorizationAttackParametersVM Parameters { get; }
    public IRSAFactorizationAttackResultsVM Results { get; }

    public bool IsInProgress { get; private set; }

    public ICommand Attack => _attack ??= new AsyncRelayCommand(_ => Attack_Internal());
    public ICommand Cancel => _cancel ??= new RelayCommand(_ => Cancel_Internal());

    private ICommand? _attack;
    private ICommand? _cancel;

    private readonly IRSAAttackService _rsaAttackService;

    private CancellationTokenSource? _tokenSource;

    public RSAFactorizationAttackVM(
        IRSAFactorizationAttackParametersVM parameters,
        IRSAFactorizationAttackResultsVM results,
        IRSAAttackService rsaAttackService)
    {
        Parameters = parameters;
        Results = results;
        _rsaAttackService = rsaAttackService;
    }

    private async Task Attack_Internal()
    {
        if (IsInProgress)
        {
            return;
        }

        if (Parameters.HasErrors)
        {
            MessageBox.Show("Parameters has invalid state.");
            return;
        }

        IsInProgress = true;
        Results.PrivateExponent = 0;

        _tokenSource = new CancellationTokenSource();
        try
        {
            var privateExponent = await _rsaAttackService.AttackAsync(
                Parameters.PublicExponent!.Value,
                Parameters.Modulus!.Value,
                _tokenSource.Token
            );

            Results.PrivateExponent = privateExponent;
        }
        catch (OperationCanceledException)
        {
        }
        catch (CryptographyAttackException e)
        {
            MessageBox.Show($"Attack error:\n\n{e}");
        }
        finally
        {
            _tokenSource.Dispose();
            _tokenSource = null;
            IsInProgress = false;
        }
    }

    private void Cancel_Internal()
    {
        _tokenSource?.Cancel();
    }
}