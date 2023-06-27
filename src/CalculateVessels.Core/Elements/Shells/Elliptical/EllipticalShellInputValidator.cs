using FluentValidation;

namespace CalculateVessels.Core.Elements.Shells.Elliptical;

internal class EllipticalShellInputValidator : AbstractValidator<EllipticalShellInput>
{
    public EllipticalShellInputValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(d => d.EllipseH)
            .NotEqual(d => default);

        RuleFor(d => d.Ellipseh1)
            .NotEqual(d => default);
    }
}
