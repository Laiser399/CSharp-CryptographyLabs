using CryptographyLabs.GUI.AbstractViewModels;
using FluentValidation;

namespace CryptographyLabs.GUI.Validators;

public class PrimesGenerationParametersVMValidator : AbstractValidator<IPrimesGenerationParametersVM>
{
    public PrimesGenerationParametersVMValidator()
    {
        RuleFor(x => x.Seed)
            .NotNull()
            .OverridePropertyName(nameof(IPrimesGenerationParametersVM.SeedStr));

        RuleFor(x => x.ByteCount)
            .NotNull()
            .GreaterThanOrEqualTo(1)
            .LessThan(10000)
            .OverridePropertyName(nameof(IPrimesGenerationParametersVM.ByteCountStr));

        RuleFor(x => x.Probability)
            .NotNull()
            .GreaterThan(0)
            .LessThan(1)
            .OverridePropertyName(nameof(IPrimesGenerationParametersVM.ProbabilityStr));
    }
}