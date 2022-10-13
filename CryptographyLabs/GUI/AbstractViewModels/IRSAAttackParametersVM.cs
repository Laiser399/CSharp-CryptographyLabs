using System.ComponentModel;
using System.Numerics;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSAAttackParametersVM : INotifyPropertyChanged, INotifyDataErrorInfo
{
    BigInteger? PublicExponent { get; }
    string PublicExponentStr { get; set; }

    BigInteger? Modulus { get; }
    string ModulusStr { get; set; }
}