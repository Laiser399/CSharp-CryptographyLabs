using System;
using System.Numerics;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class PrimesGenerationResultsVM : IPrimesGenerationResultsVM
{
    public BigInteger P { get; set; } = 0;
    public BigInteger Q { get; set; } = 0;

    public ICommand CopyPToClipboard => _copyPToClipboard ??= new RelayCommand(_ => CopyPToClipboard_Internal());
    public ICommand CopyQToClipboard => _copyQToClipboard ??= new RelayCommand(_ => CopyQToClipboard_Internal());

    private ICommand? _copyPToClipboard;
    private ICommand? _copyQToClipboard;

    private void CopyPToClipboard_Internal()
    {
        throw new NotImplementedException();
    }

    private void CopyQToClipboard_Internal()
    {
        throw new NotImplementedException();
    }
}