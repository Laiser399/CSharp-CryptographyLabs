using System.Windows;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Module.RSA.Services.Abstract;

namespace CryptographyLabs.GUI.ViewModels;

public class RSAKeyGenerationVM : IRSAKeyGenerationVM
{
    public IRSAKeyGenerationParametersVM Parameters { get; }
    public IRSAKeyGenerationResultsVM Results { get; }

    public ICommand Generate => _generate ??= new RelayCommand(_ => Generate_Internal());
    private ICommand? _generate;

    private readonly IRSAKeyPairGenerator _rsaKeyPairGenerator;

    public RSAKeyGenerationVM(
        IRSAKeyGenerationParametersVM parameters,
        IRSAKeyGenerationResultsVM results,
        IRSAKeyPairGenerator rsaKeyPairGenerator)
    {
        Parameters = parameters;
        Results = results;
        _rsaKeyPairGenerator = rsaKeyPairGenerator;
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

        var keyPair = _rsaKeyPairGenerator.Generate(Parameters.P!.Value, Parameters.Q!.Value);

        Results.PublicExponent = keyPair.Public.Exponent;
        Results.PrivateExponent = keyPair.Private.Exponent;
        Results.Modulus = keyPair.Private.Modulus;
    }
}