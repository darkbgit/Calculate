using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Enums;
using System.Collections.Generic;

namespace CalculateVessels.Core.Shells.Cylindrical;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class CylindricalShellInput : IInputData
{
    private List<string> _errorList = new();

    public bool ConditionForCalcF5341 { get; set; }
    public double c1 { get; set; }
    public double c2 { get; set; }
    public double c3 { get; set; }
    public double D { get; set; }
    public double E { get; set; }
    public IEnumerable<string> ErrorList => _errorList;
    public double F { get; set; }
    public int FCalcSchema { get; set; } //1-7
    public double f { get; set; }
    public double fi { get; set; }
    public double fi_t { get; set; }
    public bool IsPressureIn { get; set; }
    public bool IsFTensile { get; set; }
    public double l { get; set; }
    public double l3 { get; set; }
    public double M { get; set; }
    public string Name { get; set; } = string.Empty;
    public double ny { get; set; } = 2.4;
    public double p { get; set; }
    public double Q { get; set; }
    public double q { get; set; }
    public double SigmaAllow { get; set; }
    public string Steel { get; set; } = string.Empty;
    public ShellType ShellType { get; }
    public double s { get; set; }
    public double t { get; set; }
}