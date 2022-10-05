using System;
using System.Collections;
using System.ComponentModel;
using System.Numerics;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class RSAKeyGenerationParametersDTVM : IRSAKeyGenerationParametersVM
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => false;

    public BigInteger? P { get; } = BigInteger.Parse("23094587234");
    public string PStr { get; set; } = "23094587234";

    public BigInteger? Q { get; } = BigInteger.Parse("9807");
    public string QStr { get; set; } = "9807";

    public IEnumerable GetErrors(string? propertyName)
    {
        return Array.Empty<object>();
    }
}