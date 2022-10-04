using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class PrimesGenerationParametersDTVM : IPrimesGenerationParametersVM
{
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
    public bool HasErrors => false;

    public int? Seed => 666;
    public string SeedStr { get; set; } = "666";

    public int? ByteCount => 256;
    public string ByteCountStr { get; set; } = "256";

    public double? Probability => 0.995;
    public string ProbabilityStr { get; set; } = "0.995";

    public bool IsSaveToFile { get; set; } = true;
    public string SaveDirectory { get; set; } = @"Path\To\Some\Directory";
    public ICommand ChangeSaveDirectory { get; } = new RelayCommand(_ => { });

    public IEnumerable GetErrors(string? propertyName)
    {
        return Array.Empty<object>();
    }
}