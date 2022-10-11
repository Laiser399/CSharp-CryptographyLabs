using System.ComponentModel;
using System.Numerics;

namespace CryptographyLabs.GUI.AbstractViewModels;

public interface IRSAFactorizationAttackParametersVM : INotifyPropertyChanged, INotifyDataErrorInfo
{
    BigInteger? PublicExponent { get; }
    string PublicExponentStr { get; set; }

    BigInteger? Modulus { get; }
    string ModulusStr { get; set; }
}