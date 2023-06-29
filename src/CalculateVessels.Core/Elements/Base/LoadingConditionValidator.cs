using FluentValidation;

namespace CalculateVessels.Core.Elements.Base;

internal class LoadingConditionValidator : AbstractValidator<LoadingCondition>
{
    public LoadingConditionValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(d => d)
            .NotNull();

        RuleFor(d => d.PressureType)
            .IsInEnum();

        RuleFor(d => d.p)
            .InclusiveBetween(0, 200);
    }
}