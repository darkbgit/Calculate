using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;
using System.Collections.Generic;

namespace CalculateVessels.Core.ColumnSkirt;

public class ColumnSkirtInput : InputData, IInputData
{

    public double D0 { get; set; }


    public double l { get; set; }
    public double l3 { get; set; }


    private double _l;
    private double _l3;


    public double fi_t { get; set; }

    public bool ConditionForCalcF5341 { get; set; }

    private int _FCalcSchema;
    public int FCalcSchema { get; set; }

    public double f { get; set; }

    public bool IsFTensile { get; set; }


    public string Name => throw new System.NotImplementedException();

    public IEnumerable<string> ErrorList => throw new System.NotImplementedException();
}