namespace CalculateVessels.Core.Base;

public class LoadingConditionSaddle : LoadingCondition
{
    public int N { get; set; } = 1000;
    public double G { get; set; }
    public bool IsAssembly { get; set; }
}