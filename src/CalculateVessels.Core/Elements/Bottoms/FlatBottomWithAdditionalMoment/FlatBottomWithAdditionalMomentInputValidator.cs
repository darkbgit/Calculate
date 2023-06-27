using FluentValidation;

namespace CalculateVessels.Core.Elements.Bottoms.FlatBottomWithAdditionalMoment;

internal class FlatBottomWithAdditionalMomentInputValidator : AbstractValidator<FlatBottomWithAdditionalMomentInput>
{
    public FlatBottomWithAdditionalMomentInputValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(d => d)
            .NotNull();
    }
}