using Module.DES.Services.Abstract;

namespace Module.DES.Services;

public class FeistelFunctionService : IFeistelFunctionService
{
    private readonly IDesExpandFunction _desExpandFunction;
    private readonly IUInt32BitPermutationService _feistelPermutationService;
    private readonly IDesSubstitutionService _desSubstitutionService;

    public FeistelFunctionService(
        IDesExpandFunction desExpandFunction,
        IUInt32BitPermutationService feistelPermutationService,
        IDesSubstitutionService desSubstitutionService)
    {
        _desExpandFunction = desExpandFunction;
        _feistelPermutationService = feistelPermutationService;
        _desSubstitutionService = desSubstitutionService;
    }

    public uint Calculate(uint value, ulong roundKey48)
    {
        var value48 = _desExpandFunction.Calculate(value) ^ roundKey48;

        var substitutionResult = _desSubstitutionService.Substitute(value48);

        return _feistelPermutationService.Permute(substitutionResult);
    }
}