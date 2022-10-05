using System;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.ViewModels;

public class RSATransformationVM : IRSATransformationVM
{
    public IRSATransformationParametersVM Parameters { get; }

    public bool IsInProgress { get; private set; }
    public double Progress { get; private set; }

    public ICommand Transform => _transform ??= new RelayCommand(_ => Transform_Internal());
    private ICommand? _transform;

    public RSATransformationVM(IRSATransformationParametersVM parametersVM)
    {
        Parameters = parametersVM;
    }

    private void Transform_Internal()
    {
        throw new NotImplementedException();
    }
}