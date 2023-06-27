using FluentValidation;

namespace CalculateVessels.Core.Elements.Bottoms.FlatBottom;

internal class FlatBottomInputValidator : AbstractValidator<FlatBottomInput>
{
    public FlatBottomInputValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(d => d)
            .NotNull();

        RuleFor(d => d.Steel)
            .NotEmpty();
    }
}