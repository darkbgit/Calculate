using CalculateVessels.Core.Elements.Shells.Base;
using System.Collections.Generic;

namespace CalculateVessels.Core.Elements.Shells.Cylindrical;

public class CylindricalShellCalculatedCommon : ShellCalculatedCommonData
{
    //public override string Type => nameof(CylindricalShellCalculatedCommon);
    public bool IsConditionUseFormulas { get; set; }
    public ICollection<string> ErrorList { get; set; } = new List<string>();
}