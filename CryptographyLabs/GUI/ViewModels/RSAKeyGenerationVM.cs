using System.Windows;
using System.Windows.Input;
using Autofac.Features.Indexed;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.RSA;
using Module.RSA.Services.Abstract;

namespace CryptographyLabs.GUI.ViewModels;

public class RSAKeyGenerationVM : IRSAKeyGenerationVM
{
    public IRSAKeyGenerationParametersVM Parameters { get; }
    public IRSAKeyGenerationResultsVM Results { get; }

    public ICommand Generate => _generate ??= new RelayCommand(_ => Generate_Internal());
    private ICommand? _generate;

    private readonly IIndex<RSAKeyPairGenerationType, IRSAKeyPairGenerator> _rsaKeyPairGenerators;

    public RSAKeyGenerationVM(
        IRSAKeyGenerationParametersVM parameters,
        IRSAKeyGenerationResultsVM results,
        IIndex<RSAKeyPairGenerationType, IRSAKeyPairGenerator> rsaKeyPairGenerators)
    {
        Parameters = parameters;
        Results = results;
        _rsaKeyPairGenerators = rsaKeyPairGenerators;
    }

    private void Generate_Internal()
    {
        if (Parameters.HasErrors)
        {
            MessageBox.Show("Parameters is not configured correctly.");
            return;
        }

        Results.PublicExponent = 0;
        Results.PrivateExponent = 0;
        Results.Modulus = 0;

        var keyPairGenerator = Parameters.ForceWienerAttackVulnerability
            ? _rsaKeyPairGenerators[RSAKeyPairGenerationType.WithWienerAttackVulnerability]
            : _rsaKeyPairGenerators[RSAKeyPairGenerationType.Default];

        var keyPair = keyPairGenerator.Generate(Parameters.P!.Value, Parameters.Q!.Value);

        Results.PublicExponent = keyPair.Public.Exponent;
        Results.PrivateExponent = keyPair.Private.Exponent;
        Results.Modulus = keyPair.Private.Modulus;
    }
}