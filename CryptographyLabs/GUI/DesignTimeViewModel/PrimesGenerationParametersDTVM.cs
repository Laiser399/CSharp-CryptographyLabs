using System.Windows.Input;
using CryptographyLabs.GUI.AbstractViewModels;

namespace CryptographyLabs.GUI.DesignTimeViewModel;

public class PrimesGenerationParametersDTVM : IPrimesGenerationParametersVM
{
    public int Seed { get; set; } = 666;
    public int ByteCount { get; set; } = 256;
    public double Probability { get; set; } = 0.995;

    public bool IsSaveToFile { get; set; } = true;
    public string SaveDirectory { get; set; } = @"Path\To\Some\Directory";
    public ICommand ChangeSaveDirectory { get; } = new RelayCommand(_ => { });
}