using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Nozzle.Enums;
using System.Collections.Generic;
using System.Linq;

namespace CalculateVessels.Core.Shells.Nozzle;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming
public class NozzleInput : InputData, IInputData
{
    private List<string> _errorList = new();

    private readonly ICalculatedElement _shellCalculatedData;

    public NozzleInput(ICalculatedElement shellCalculatedData)
    {
        _shellCalculatedData = shellCalculatedData;
    }

    public override string Type => nameof(NozzleInput);
    public double cs { get; set; }
    public double cs1 { get; set; }
    public double d { get; set; }
    public double d1 { get; set; }
    public double d2 { get; set; }
    public double delta { get; set; }
    public double delta1 { get; set; }
    public double delta2 { get; set; }
    //public IEnumerable<string> ErrorList => _errorList;
    public double E1 { get; set; }
    public double E2 { get; set; }
    public double E3 { get; set; }
    public double E4 { get; set; }
    public double ellx { get; set; }
    public double phi { get; set; }
    public double phi1 { get; set; }
    public double gamma { get; set; }
    public bool IsOval { get; set; }
    public NozzleLocation Location { get; set; }
    public double l { get; set; }
    public double l1 { get; set; }
    public double l2 { get; set; }
    public double l3 { get; set; }
    public NozzleKind NozzleKind { get; set; }
    public double omega { get; set; }
    public double r { get; set; }
    public ICalculatedElement ShellCalculatedData => _shellCalculatedData;
    public double SigmaAllow1 { get; set; }
    public double SigmaAllow2 { get; set; }
    public double SigmaAllow3 { get; set; }
    public double SigmaAllow4 { get; set; }
    public double s0 { get; set; }
    public double s1 { get; set; }
    public double s2 { get; set; }
    public double s3 { get; set; }
    public string steel1 { get; set; } = string.Empty;
    public string steel2 { get; set; } = string.Empty;
    public string steel3 { get; set; } = string.Empty;
    public string steel4 { get; set; } = string.Empty;
    //public double t { get; set; }
    public double tTransversely { get; set; }

    //public IElement Element { get; set; }
    //public ShellDataIn ShellDataIn => (ShellDataIn) _shellCalculatedData.InputData;

    public override bool IsDataGood
    {
        get
        {
            if (s3 > 0 && cs + cs1 > s3)
            {
                ErrorList.Add("cs+cs1 должно быть меньше s3.");
            }

            return !ErrorList.Any();
        }
    }
}