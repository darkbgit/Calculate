using FluentValidation;

namespace CalculateVessels.Core.Elements.Supports.Saddle;

internal class SaddleInputValidator : AbstractValidator<SaddleInput>
{
    public SaddleInputValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(d => d)
            .NotNull();

        RuleFor(d => d.Steel)
            .NotEmpty();
    }
}