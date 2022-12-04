using CryptographyLabs.GUI.AbstractViewModels;
using FluentValidation;

namespace CryptographyLabs.GUI.Validators;

public class RSAKeyGenerationParametersVMValidator : AbstractValidator<IRSAKeyGenerationParametersVM>
{
    public RSAKeyGenerationParametersVMValidator()
    {
        RuleFor(x => x.P)
            .NotNull()
            .GreaterThanOrEqualTo(2)
            .OverridePropertyName(nameof(IRSAKeyGenerationParametersVM.PStr));
        RuleFor(x => x.Q)
            .NotNull()
            .GreaterThanOrEqualTo(2)
            .OverridePropertyName(nameof(IRSAKeyGenerationParametersVM.QStr));
    }
}