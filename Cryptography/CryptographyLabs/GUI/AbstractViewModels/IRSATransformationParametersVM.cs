using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSATransformationParametersVM : INotifyPropertyChanged, INotifyDataErrorInfo
{
    bool IsEncryption { get; set; }

    BigInteger? Exponent { get; }
    string ExponentStr { get; set; }

    BigInteger? Modulus { get; }
    string ModulusStr { get; set; }

    string FilePath { get; set; }

    ICommand SetPublicKeyFromGenerationResults { get; }
    ICommand SetPrivateKeyFromGenerationResults { get; }
    ICommand ChangeFilePath { get; }
}