using System.Linq;
using CalculateVessels.Core.Elements.Base;
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

        RuleFor(d => d.LoadingConditions)
            .NotEmpty();

        RuleForEach(d => d.LoadingConditions)
            .SetValidator(new LoadingConditionValidator());

        RuleFor(d => d.ShellType)
            .IsInEnum();

        RuleFor(d => d.phi)
            .InclusiveBetween(0, 1);
        RuleFor(d => d.D)
            .GreaterThan(0);
        RuleFor(d => d.c1)
            .GreaterThanOrEqualTo(0);

        RuleFor(d => d.c2)
            .GreaterThanOrEqualTo(0);

        RuleFor(d => d.c3)
            .GreaterThanOrEqualTo(0);

        RuleFor(d => d.s)
            .Must((d, s) => s == 0 || s > d.c1 + d.c2 + d.c3)
            .WithMessage(d => $"Сумма прибавок c1+c2+c3={d.c1 + d.c2 + d.c3} " + "должна быть меньше {PropertyName}={PropertyValue}.");

        RuleFor(d => d.l)
            .Must((d, l) => d.LoadingConditions.All(lc => lc.PressureType == PressureType.Inside) || l > 0)
            .WithMessage("{PropertyName} должна быть задана для наружного давления.");

        RuleFor(d => d.FCalcSchema)
            .Must((d, f) => !d.IsFTensile || f is >= 0 and <= 7);

        RuleFor(d => d.q)
            .Must((d, q) => d.F == 0 || d.IsFTensile || d.FCalcSchema is not 5 || q > 0)
            .WithMessage("{PropertyName} должно быть задано для 5 схемы нагружение осевой сжимающей силой.");

        RuleFor(d => d.f)
            .Must((d, f) => d.F == 0 || d.IsFTensile || d.FCalcSchema is not (6 or 7) || f > 0)
            .WithMessage("{PropertyName} должно быть задано для 6(7) схемы нагружение осевой сжимающей силой.");

    }
}