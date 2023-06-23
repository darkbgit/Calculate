using System.Collections.Generic;
using CalculateVessels.Core.Elements.Shells.Base;

namespace CalculateVessels.Core.Elements.Shells.Elliptical;

#pragma warning disable IDE1006
// ReSharper disable InconsistentNaming

public class EllipticalShellCalculatedCommon : ShellCalculatedCommonData
{
    //public override string Type => nameof(EllipticalShellCalculatedCommon);
    //public double c { get; set; }
    public double EllipseR { get; set; }
    public bool IsConditionUseFormulas { get; set; }
    public ICollection<string> ErrorList { get; set; } = new List<string>();
}