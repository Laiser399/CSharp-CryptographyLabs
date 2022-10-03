using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class PrimesGenerationParametersVM : IPrimesGenerationParametersVM
{
    public int Seed { get; set; }
    public int ByteCount { get; set; }
    public double Probability { get; set; }

    public bool IsSaveToFile { get; set; }
    public string SaveDirectory { get; set; } = string.Empty;

    public ICommand ChangeSaveDirectory =>
        _changeSaveDirectory ??= new RelayCommand(_ => ChangeSaveDirectory_Internal());

    private ICommand? _changeSaveDirectory;

    private void ChangeSaveDirectory_Internal()
    {
        var dialog = new CommonOpenFileDialog
        {
            IsFolderPicker = true,
            EnsurePathExists = true
        };

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            SaveDirectory = dialog.FileName;
        }
    }
}