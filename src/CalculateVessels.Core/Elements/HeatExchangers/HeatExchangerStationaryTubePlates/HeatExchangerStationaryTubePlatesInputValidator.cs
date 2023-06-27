using FluentValidation;

namespace CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerStationaryTubePlates;

internal class HeatExchangerStationaryTubePlatesInputValidator : AbstractValidator<HeatExchangerStationaryTubePlatesInput>
{
    public HeatExchangerStationaryTubePlatesInputValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(d => d)
            .NotNull();
    }
}