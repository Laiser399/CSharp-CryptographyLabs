using System;
using System.Collections;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class RSATransformationParametersDTVM : IRSATransformationParametersVM
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => false;

    public bool IsEncryption { get; set; } = false;

    public BigInteger? Exponent { get; } = BigInteger.Parse("98798435734");
    public string ExponentStr { get; set; } = "98798435734";

    public BigInteger? Modulus { get; } = BigInteger.Parse("309847569283475923487");
    public string ModulusStr { get; set; } = "309847569283475923487";

    public string FilePath { get; set; } = @"Some\Path\To\File";

    public ICommand SetPublicKeyFromGenerationResults { get; } = new RelayCommand(_ => { });
    public ICommand SetPrivateKeyFromGenerationResults { get; } = new RelayCommand(_ => { });
    public ICommand ChangeFilePath { get; } = new RelayCommand(_ => { });

    public IEnumerable GetErrors(string? propertyName)
    {
        return Array.Empty<object>();
    }
}