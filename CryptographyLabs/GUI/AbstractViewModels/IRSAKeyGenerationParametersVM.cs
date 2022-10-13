using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSAKeyGenerationParametersVM : INotifyPropertyChanged, INotifyDataErrorInfo
{
    BigInteger? P { get; }
    string PStr { get; set; }

    BigInteger? Q { get; }
    string QStr { get; set; }

    bool ForceWienerAttackVulnerability { get; set; }

    ICommand SetFromPrimesGenerationResults { get; }
}