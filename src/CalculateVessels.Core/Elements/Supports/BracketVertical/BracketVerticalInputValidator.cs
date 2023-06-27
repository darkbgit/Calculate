using FluentValidation;

namespace CalculateVessels.Core.Elements.Supports.BracketVertical;

internal class BracketVerticalInputValidator : AbstractValidator<BracketVerticalInput>
{
    public BracketVerticalInputValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(d => d)
            .NotNull();

        RuleFor(d => d.Steel)
            .NotEmpty();
    }
}