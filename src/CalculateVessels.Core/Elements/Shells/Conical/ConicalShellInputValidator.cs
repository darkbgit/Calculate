using System.Linq;
using FluentValidation;

namespace CalculateVessels.Core.Elements.Shells.Conical;

internal class ConicalShellInputValidator : AbstractValidator<ConicalShellInput>
{
    public ConicalShellInputValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(d => d.LoadingConditions)
            .Must(lc =>
            {
                var loadingConditions = lc.ToList();
                return loadingConditions.Count == loadingConditions.DistinctBy(c => c.Id).Count();
            });

        RuleFor(d => d)
            .NotNull();

        RuleFor(d => d.Steel)
            .NotEmpty();

        RuleFor(d => d.D)
            .GreaterThan(d => d.D1)
            .WithMessage("{PropertyName} must be greater then {ComparisonProperty}={ComparisonValue}.");
    }
}
