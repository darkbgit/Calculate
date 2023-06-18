using CalculateVessels.Core.Shells.Base;
using System.Collections.Generic;

namespace CalculateVessels.Core.Shells.Cylindrical;

#pragma warning disable IDE1006
// ReSharper disable InconsistentNaming

public class CylindricalShellCalculatedCommon : ShellCalculatedCommonData
{
    //public override string Type => nameof(CylindricalShellCalculatedCommon);
    public bool IsConditionUseFormulas { get; set; }
    public ICollection<string> ErrorList { get; set; } = new List<string>();
}