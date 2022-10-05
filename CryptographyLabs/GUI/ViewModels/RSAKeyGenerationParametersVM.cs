using System;
using System.Collections;
using System.ComponentModel;
using System.Numerics;
using CryptographyLabs.GUI.AbstractViewModels;
using PropertyChanged;

namespace CryptographyLabs.GUI.ViewModels;

[AddINotifyPropertyChangedInterface]
public class RSAKeyGenerationParametersVM : IRSAKeyGenerationParametersVM
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
    {
        add => _validationTemplate.ErrorsChanged += value;
        remove => _validationTemplate.ErrorsChanged -= value;
    }

    public bool HasErrors => _validationTemplate.HasErrors;

    #region P

    public BigInteger? P { get; private set; }


    public string PStr
    {
        get => _pStr;
        set
        {
            _pStr = value;
            if (BigInteger.TryParse(value, out var p))
            {
                P = p;
            }
        }
    }

    private string _pStr = string.Empty;

    #endregion

    #region Q

    public BigInteger? Q { get; private set; }


    public string QStr
    {
        get => _qStr;
        set
        {
            _qStr = value;
            if (BigInteger.TryParse(value, out var q))
            {
                Q = q;
            }
        }
    }

    private string _qStr = string.Empty;

    #endregion

    private readonly INotifyDataErrorInfo _validationTemplate;

    public RSAKeyGenerationParametersVM(
        ValidationTemplateFactory<IRSAKeyGenerationParametersVM> validationTemplateFactory)
    {
        _validationTemplate = validationTemplateFactory(this);
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        return _validationTemplate.GetErrors(propertyName);
    }
}