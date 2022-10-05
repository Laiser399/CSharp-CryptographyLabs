using System.Numerics;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class RSATransformationParametersVM : IRSATransformationParametersVM
{
    public bool IsEncryption { get; set; }

    #region Exponent

    public BigInteger? Exponent { get; private set; }

    public string ExponentStr
    {
        get => _exponentStr;
        set
        {
            _exponentStr = value;
            Exponent = BigInteger.TryParse(value, out var exponent)
                ? exponent
                : null;
        }
    }

    private string _exponentStr = string.Empty;

    #endregion

    #region Modulus

    public BigInteger? Modulus { get; private set; }

    public string ModulusStr
    {
        get => _modulusStr;
        set
        {
            _modulusStr = value;
            Modulus = BigInteger.TryParse(value, out var modulus)
                ? modulus
                : null;
        }
    }

    private string _modulusStr = string.Empty;

    #endregion

    public string FilePath { get; set; } = string.Empty;

    public ICommand SetPublicKeyFromGenerationResults => _setPublicKeyFromGenerationResults ??=
        new RelayCommand(_ => SetPublicKeyFromGenerationResults_Internal());

    public ICommand SetPrivateKeyFromGenerationResults => _setPrivateKeyFromGenerationResults ??=
        new RelayCommand(_ => SetPrivateKeyFromGenerationResults_Internal());

    public ICommand ChangeFilePath => _changeFilePath ??= new RelayCommand(_ => ChangeFilePath_Internal());

    private ICommand? _setPublicKeyFromGenerationResults;
    private ICommand? _setPrivateKeyFromGenerationResults;
    private ICommand? _changeFilePath;

    private readonly IRSAKeyGenerationResultsVM _rsaKeyGenerationResultsVM;

    public RSATransformationParametersVM(IRSAKeyGenerationResultsVM rsaKeyGenerationResultsVM)
    {
        _rsaKeyGenerationResultsVM = rsaKeyGenerationResultsVM;
    }

    private void SetPublicKeyFromGenerationResults_Internal()
    {
        ExponentStr = _rsaKeyGenerationResultsVM.PublicExponent.ToString();
        ModulusStr = _rsaKeyGenerationResultsVM.Modulus.ToString();
    }

    private void SetPrivateKeyFromGenerationResults_Internal()
    {
        ExponentStr = _rsaKeyGenerationResultsVM.PrivateExponent.ToString();
        ModulusStr = _rsaKeyGenerationResultsVM.Modulus.ToString();
    }

    private void ChangeFilePath_Internal()
    {
        var dialog = new CommonOpenFileDialog
        {
            EnsureFileExists = true,
            Filters =
            {
                IsEncryption
                    ? new CommonFileDialogFilter("Any file", "*.*")
                    : new CommonFileDialogFilter("Binary file", "*.bin")
            }
        };

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            FilePath = dialog.FileName;
        }
    }
}