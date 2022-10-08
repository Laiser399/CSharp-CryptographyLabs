using System;
using System.Collections;
using System.ComponentModel;
using System.Numerics;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class RSAFactorizationAttackParametersDTVM : IRSAFactorizationAttackParametersVM
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => false;

    public BigInteger? Modulus { get; } = BigInteger.Parse("83465293845762345");
    public string ModulusStr { get; set; } = "83465293845762345";

    public IEnumerable GetErrors(string? propertyName)
    {
        return Array.Empty<object>();
    }
}