using System.Collections.Generic;
using CalculateVessels.Core.Shells.Base;

namespace CalculateVessels.Core.Shells.Elliptical;

#pragma warning disable IDE1006
// ReSharper disable InconsistentNaming

public class EllipticalShellCalculatedCommon : ShellCalculatedCommonData
{
    //public double c { get; set; }
    public double EllipseR { get; set; }
    public bool IsConditionUseFormulas { get; set; }
    public ICollection<string> ErrorList { get; set; } = new List<string>();
}