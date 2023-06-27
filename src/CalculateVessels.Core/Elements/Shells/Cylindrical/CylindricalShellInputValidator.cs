using System.Linq;
using CalculateVessels.Core.Enums;
using FluentValidation;

namespace CalculateVessels.Core.Elements.Shells.Cylindrical;

internal class CylindricalShellInputValidator : AbstractValidator<CylindricalShellInput>
{
    public CylindricalShellInputValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(d => d)
            .NotNull();

        RuleFor(d => d.Steel)
            .NotEmpty();

        RuleFor(d => d.l)
            .Must((d, l) => d.LoadingConditions.Any(lc => lc.PressureType == PressureType.Outside) && l <= 0)
            .WithMessage("{PropertyName} должна быть задана для наружного давления.");

    }
}