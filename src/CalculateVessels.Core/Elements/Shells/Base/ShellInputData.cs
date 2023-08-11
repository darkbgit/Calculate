using System.Collections.Generic;
using System.Linq;
using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Elements.Shells.Enums;
using CalculateVessels.Data.Public.Models;

namespace CalculateVessels.Core.Elements.Shells.Base;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public abstract class ShellInputData //: InputData
{
    protected ShellInputData(ShellType shellType)
    {
        ShellType = shellType;
    }

    public IEnumerable<LoadingCondition> LoadingConditions { get; set; } = Enumerable.Empty<LoadingCondition>();

    public string Name { get; set; } = string.Empty;
    public double c1 { get; set; }
    public double c2 { get; set; }
    public double c3 { get; set; }
    public double D { get; init; }
    public double phi { get; set; }
    public double s { get; set; }
    public ShellType ShellType { get; }
    public SteelWithIdsDto Steel { get; set; }
}