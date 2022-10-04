using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class PrimesGenerationParametersVM : IPrimesGenerationParametersVM
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool HasErrors => _validationTemplate.HasErrors;

    #region Seed

    public int? Seed { get; private set; }

    public string SeedStr
    {
        get => _seedStr;
        set
        {
            _seedStr = value;
            Seed = int.TryParse(value, out var seed)
                ? seed
                : null;
        }
    }

    private string _seedStr = "0";

    #endregion

    #region ByteCount

    public int? ByteCount { get; private set; }

    public string ByteCountStr
    {
        get => _byteCountStr;
        set
        {
            _byteCountStr = value;
            ByteCount = int.TryParse(value, out var byteCount)
                ? byteCount
                : null;
        }
    }

    private string _byteCountStr = "16";

    #endregion

    #region Probability

    public double? Probability { get; private set; }

    public string ProbabilityStr
    {
        get => _probabilityStr;
        set
        {
            _probabilityStr = value;
            Probability = double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var probability)
                ? probability
                : null;
        }
    }

    private string _probabilityStr = "0.995";

    #endregion

    public bool IsSaveToFile { get; set; }
    public string SaveDirectory { get; set; } = string.Empty;

    public ICommand ChangeSaveDirectory =>
        _changeSaveDirectory ??= new RelayCommand(_ => ChangeSaveDirectory_Internal());

    private ICommand? _changeSaveDirectory;

    private readonly INotifyDataErrorInfo _validationTemplate;

    public PrimesGenerationParametersVM(
        ValidationTemplateFactory<IPrimesGenerationParametersVM> validationTemplateFactory)
    {
        _validationTemplate = validationTemplateFactory(this);
        _validationTemplate.ErrorsChanged += ErrorsChanged;
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        return _validationTemplate.GetErrors(propertyName);
    }

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