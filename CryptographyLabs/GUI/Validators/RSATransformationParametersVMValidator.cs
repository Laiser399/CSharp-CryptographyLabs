using CryptographyLabs.GUI.AbstractViewModels;
using FluentValidation;

namespace CryptographyLabs.GUI.Validators;

public class RSATransformationParametersVMValidator : AbstractValidator<IRSATransformationParametersVM>
{
    public RSATransformationParametersVMValidator()
    {
        RuleFor(x => x.Exponent)
            .NotNull()
            .GreaterThanOrEqualTo(1)
            .OverridePropertyName(nameof(IRSATransformationParametersVM.ExponentStr));
        RuleFor(x => x.Modulus)
            .NotNull()
            .GreaterThanOrEqualTo(1)
            .OverridePropertyName(nameof(IRSATransformationParametersVM.ModulusStr));
    }
}