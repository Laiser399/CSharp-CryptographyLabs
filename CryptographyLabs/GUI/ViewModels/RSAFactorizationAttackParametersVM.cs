using System;
using System.Collections;
using System.ComponentModel;
using System.Numerics;
using CryptographyLabs.GUI.AbstractViewModels;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class RSAFactorizationAttackParametersVM : IRSAFactorizationAttackParametersVM
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
    {
        add => _validationTemplate.ErrorsChanged += value;
        remove => _validationTemplate.ErrorsChanged -= value;
    }

    public bool HasErrors => _validationTemplate.HasErrors;

    #region PublicExponent

    public BigInteger? PublicExponent { get; private set; }
    public string PublicExponentStr { get; set; }

    public void OnPublicExponentStrChanged()
    {
        PublicExponent = BigInteger.TryParse(PublicExponentStr, out var publicExponent)
            ? publicExponent
            : null;
    }

    #endregion

    #region Modulus

    public BigInteger? Modulus { get; private set; }
    public string ModulusStr { get; set; }

    public void OnModulusStrChanged()
    {
        Modulus = BigInteger.TryParse(ModulusStr, out var modulus)
            ? modulus
            : null;
    }

    #endregion

    private readonly INotifyDataErrorInfo _validationTemplate;

    public RSAFactorizationAttackParametersVM(
        ValidationTemplateFactory<IRSAFactorizationAttackParametersVM> validationTemplateFactory)
    {
        _validationTemplate = validationTemplateFactory(this);

        PublicExponentStr = "3";
        ModulusStr = "15";
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        return _validationTemplate.GetErrors(propertyName);
    }
}