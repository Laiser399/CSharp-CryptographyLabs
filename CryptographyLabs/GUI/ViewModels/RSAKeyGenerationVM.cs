using System;
using System.Windows;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.RSA.Entities;
using Module.RSA.Entities.Abstract;
using Module.RSA.Services.Abstract;

namespace CryptographyLabs.GUI.ViewModels;

public class RSAKeyGenerationVM : IRSAKeyGenerationVM
{
    public IRSAKeyGenerationParametersVM Parameters { get; }
    public IRSAKeyGenerationResultsVM Results { get; }

    public ICommand Generate => _generate ??= new RelayCommand(_ => Generate_Internal());
    private ICommand? _generate;

    private readonly Func<IRSAKeyPairGeneratorParameters, IRSAKeyPairGenerator> _rsaKeyPairGeneratorFactory;

    public RSAKeyGenerationVM(
        IRSAKeyGenerationParametersVM parameters,
        IRSAKeyGenerationResultsVM results,
        Func<IRSAKeyPairGeneratorParameters, IRSAKeyPairGenerator> rsaKeyPairGeneratorFactory)
    {
        Parameters = parameters;
        Results = results;
        _rsaKeyPairGeneratorFactory = rsaKeyPairGeneratorFactory;
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

        var generator = _rsaKeyPairGeneratorFactory(
            new RSAKeyPairGeneratorParameters(Parameters.ForceWienerAttackVulnerability)
        );
        var keyPair = generator.Generate(Parameters.P!.Value, Parameters.Q!.Value);

        Results.PublicExponent = keyPair.Public.Exponent;
        Results.PrivateExponent = keyPair.Private.Exponent;
        Results.Modulus = keyPair.Private.Modulus;
    }
}