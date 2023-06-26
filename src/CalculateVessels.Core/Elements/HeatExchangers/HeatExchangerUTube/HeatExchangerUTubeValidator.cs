using FluentValidation;

namespace CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerUTube;

internal class HeatExchangerUTubeValidator : AbstractValidator<HeatExchangerUTubeInput>
{
    public HeatExchangerUTubeValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(d => d)
            .NotNull();

        RuleFor(d => d.Dcp)
            .GreaterThan(d => d.DB)
            //.WithMessage(d => $"{nameof(d.Dcp)} must be greater then {nameof(d.DB)}={d.DB}.");
            .WithMessage("{PropertyName} must be greater then {ComparisonProperty}={ComparisonValue}.");

        RuleFor(d => d.Steelp)
            .NotEmpty();
    }
}