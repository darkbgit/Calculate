﻿using CalculateVessels.Core.Base;
using CalculateVessels.Core.Shells.Enums;
using System.Collections.Generic;
using System.Linq;

namespace CalculateVessels.Core.Shells.Base;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public abstract class ShellInputData : InputData
{
    protected ShellInputData(ShellType shellType)
    {
        ShellType = shellType;
    }

    public IEnumerable<LoadingCondition> LoadingConditions { get; set; } = Enumerable.Empty<LoadingCondition>();

    public double c1 { get; set; }
    public double c2 { get; set; }
    public double c3 { get; set; }
    public double D { get; init; }
    public double phi { get; set; }
    public double s { get; set; }
    public ShellType ShellType { get; }
    public string Steel { get; set; } = string.Empty;

    //public double SigmaAllow { get; set; }
    //public double ny { get; set; } = 2.4;
    //public double F { get; set; }
    //public double q { get; set; }
    //public double M { get; set; }
    //public double Q { get; set; }
    //public abstract override bool IsDataGood { get; }
}