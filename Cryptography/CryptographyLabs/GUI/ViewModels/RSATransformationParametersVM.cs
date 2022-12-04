using System;
using System.Collections;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class RSATransformationParametersVM : IRSATransformationParametersVM
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
    {
        add => _validationTemplate.ErrorsChanged += value;
        remove => _validationTemplate.ErrorsChanged -= value;
    }

    public bool HasErrors => _validationTemplate.HasErrors;

    public bool IsEncryption { get; set; } = true;

    #region Exponent

    public BigInteger? Exponent { get; private set; }

    public string ExponentStr { get; set; }

    public void OnExponentStrChanged()
    {
        Exponent = BigInteger.TryParse(ExponentStr, out var exponent)
            ? exponent
            : null;
    }

    #endregion

    #region Modulus

    public BigInteger? Modulus { get; private set; }

    public string ModulusStr { get; set; }

    public void OnModulusStrChanged()
    {
        Modulus = BigInteger.TryParse(ModulusStr, out var modulus)
            ? modulus
            : null;
    }

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

    private readonly INotifyDataErrorInfo _validationTemplate;
    private readonly IRSAKeyGenerationResultsVM _rsaKeyGenerationResultsVM;

    public RSATransformationParametersVM(
        ValidationTemplateFactory<IRSATransformationParametersVM> validationTemplateFactory,
        IRSAKeyGenerationResultsVM rsaKeyGenerationResultsVM)
    {
        _validationTemplate = validationTemplateFactory(this);
        _rsaKeyGenerationResultsVM = rsaKeyGenerationResultsVM;

        ExponentStr = "2";
        ModulusStr = "137";
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        return _validationTemplate.GetErrors(propertyName);
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