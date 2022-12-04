using System.ComponentModel;
using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IPrimesGenerationParametersVM : INotifyPropertyChanged, INotifyDataErrorInfo
{
    int? Seed { get; }
    string SeedStr { get; set; }

    int? ByteCount { get; }
    string ByteCountStr { get; set; }

    double? Probability { get; }
    string ProbabilityStr { get; set; }

    bool IsSaveToFile { get; set; }
    string SaveDirectory { get; set; }
    ICommand ChangeSaveDirectory { get; }
}