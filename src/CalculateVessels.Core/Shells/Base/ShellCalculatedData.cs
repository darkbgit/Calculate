using CalculateVessels.Core.Base;
using System.Collections.Generic;

namespace CalculateVessels.Core.Shells.Base;

public abstract class ShellCalculatedData : CalculatedElement
{
    public double c { get; set; }
    public double s_p { get; set; }
    public double s { get; set; }
    public double p_de { get; set; }
    public double p_d { get; set; }
    public double SigmaAllow { get; set; }


    public ICollection<string> ErrorList => _errorList;

    public bool IsConditionUseFormulas { get; set; }



    protected List<string> _errorList = new();

}
