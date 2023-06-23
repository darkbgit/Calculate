namespace CalculateVessels.Core.Elements.Shells.Base;

#pragma warning disable IDE1006
// ReSharper disable InconsistentNaming

//[JsonConverter(typeof(InputDataConverter<CylindricalShellCalculatedCommon, ShellCalculatedCommonData>))]
public abstract class ShellCalculatedCommonData
{
    //public abstract string Type { get; }
    public double c { get; set; }
}