using FluentValidation;

namespace CalculateVessels.Core.Elements.Shells.Nozzle;

public class NozzleInputValidator : AbstractValidator<NozzleInput>
{
    public NozzleInputValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(d => d)
            .NotNull();

        RuleFor(d => d.ShellCalculatedData)
            .NotNull();

        RuleFor(d => d.s3)
            .GreaterThan(d => d.cs + d.cs1)
            .WithMessage("{PropertyName} must be greater then cs + cs1={ComparisonValue}.");
    }
}