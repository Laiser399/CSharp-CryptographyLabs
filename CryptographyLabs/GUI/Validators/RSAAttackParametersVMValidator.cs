using CryptographyLabs.GUI.AbstractViewModels;
using FluentValidation;

namespace CryptographyLabs.GUI.Validators;

public class RSAAttackParametersVMValidator : AbstractValidator<IRSAAttackParametersVM>
{
    public RSAAttackParametersVMValidator()
    {
        RuleFor(x => x.PublicExponent)
            .NotNull()
            .GreaterThan(2)
            .OverridePropertyName(nameof(IRSAAttackParametersVM.PublicExponentStr));
        RuleFor(x => x.Modulus)
            .NotNull()
            .GreaterThan(2)
            .OverridePropertyName(nameof(IRSAAttackParametersVM.ModulusStr));
    }
}