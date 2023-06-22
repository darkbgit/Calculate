using CalculateVessels.Core.Shells.Base;
using System.Collections.Generic;

namespace CalculateVessels.Core.Shells.Cylindrical;

public class CylindricalShellCalculatedCommon : ShellCalculatedCommonData
{
    //public override string Type => nameof(CylindricalShellCalculatedCommon);
    public bool IsConditionUseFormulas { get; set; }
    public ICollection<string> ErrorList { get; set; } = new List<string>();
}