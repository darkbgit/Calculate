﻿using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Enums;
using System.Linq;

namespace CalculateVessels.Core.Shells.Conical;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class ConicalShellInput : ShellInputData, IInputData
{
    public ConicalShellInput()
        : base(ShellType.Conical)
    {

    }
    public override string Type => nameof(ConicalShellInput);
    public double Ak { get; set; }
    public ConicalConnectionType ConnectionType { get; set; }
    public double D1 { get; set; }
    public double phi_k { get; set; }
    public double phi_t { get; set; }
    public bool IsConnectionWithLittle { get; set; }
    public double L { get; set; }
    public double ny { get; set; } = 2.4;
    public double r { get; set; }
    public double SigmaAllow { get; set; }
    public double SigmaAllow1Little { get; set; }
    public double SigmaAllow1Big { get; set; }
    public double SigmaAllow2Little { get; set; }
    public double SigmaAllow2Big { get; set; }
    public double SigmaAllowC { get; set; }
    public double SigmaAllowT { get; set; }
    public string Steel1Little { get; set; } = string.Empty;
    public string Steel1Big { get; set; } = string.Empty;
    public string Steel2Little { get; set; } = string.Empty;
    public string Steel2Big { get; set; } = string.Empty;
    public string SteelC { get; set; } = string.Empty;
    public string SteelT { get; set; } = string.Empty;
    public double s1Big { get; set; }
    public double s2Big { get; set; }
    public double s1Little { get; set; }
    public double s2Little { get; set; }
    public double sT { get; set; }

    public override bool IsDataGood
    {
        get
        {
            if (D <= D1)
            {
                ErrorList.Add("D должно быть больше чем D1.");
            }

            return !ErrorList.Any();
        }
    }
}