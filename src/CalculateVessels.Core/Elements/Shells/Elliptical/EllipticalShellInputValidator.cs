using System.Linq;
using FluentValidation;

namespace CalculateVessels.Core.Elements.Shells.Elliptical;

internal class EllipticalShellInputValidator : AbstractValidator<EllipticalShellInput>
{
    public EllipticalShellInputValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(d => d.LoadingConditions)
            .Must(lc =>
            {
                var loadingConditions = lc.ToList();
                return loadingConditions.Count == loadingConditions.DistinctBy(c => c.Id).Count();
            });

        RuleFor(d => d.EllipseH)
            .NotEqual(d => default);

        RuleFor(d => d.Ellipseh1)
            .NotEqual(d => default);
    }
}
