using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Autofac.Features.Indexed;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.RSA.Enums;
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

    private readonly IIndex<RSAAttackType, IRSAAttackService> _rsaAttackServices;

    private CancellationTokenSource? _tokenSource;

    public RSAFactorizationAttackVM(
        IRSAFactorizationAttackParametersVM parameters,
        IRSAFactorizationAttackResultsVM results,
        IIndex<RSAAttackType, IRSAAttackService> rsaAttackServices)
    {
        Parameters = parameters;
        Results = results;
        _rsaAttackServices = rsaAttackServices;
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
            var attackService = _rsaAttackServices[RSAAttackType.Factorization];
            var privateExponent = await attackService.AttackAsync(
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