using System;
using System.Collections.Generic;

namespace CalculateVessels.Core.Elements.HeatExchangers;

public class HeatExchangerAdditionalRequirementsCalculate
{
    public static (double spp, double sp) CheckThicknessAccordingTo5_5_1(double DE, double pp, double sigmap, double c, double spIn, ICollection<string> errorList)
    {
        var spp = 0.5 * DE * Math.Sqrt(pp / sigmap);
        var sp = spp + c;

        if (sp > spIn)
        {
            errorList.Add($"Проверка по п. 5.5.1. Толщина трубной решетки {spIn} мм меньше расчетной {sp} мм.");
        }

        return (spp, sp);
    }

    public static (double sprp1, double sprp2, double sprp, double spr) CheckThicknessAccordingTo5_5_2(double pp, double Dcp, double DB, double sigmap, double c, double sprIn, ICollection<string> errorList)
    {
        var sprp1 = 0.71 * Math.Sqrt(pp * Dcp * (Dcp - DB) / sigmap);
        var sprp2 = 0.5 * Dcp * pp / sigmap;
        var sprp = Math.Max(sprp1, sprp2);
        var spr = sprp + c;

        if (spr > sprIn)
        {
            errorList.Add($"Проверка по п. 5.5.2. Толщина трубной решетки в месте уплотнения под кольцевую прокладку {sprIn} мм меньше расчетной {spr} мм.");
        }

        return (sprp1, sprp2, sprp, spr);
    }

    public static (double snp1, double snp2, double snp, double sn) CheckThicknessAccordingTo5_5_3(double d0, double BP, double tP, double tp, double phip, double sp, double c, double snIn, ICollection<string> errorList)
    {
        var snp1 = 1 - Math.Sqrt(d0 / BP * (tP / tp - 1));
        var snp2 = Math.Sqrt(phip);
        var snp = (sp - c) * Math.Max(snp1, snp2);
        var sn = snp + c;

        if (sn > snIn)
        {
            errorList.Add($"Проверка по п. 5.5.3. Толщина трубной решетки в сечении канавки {snIn} мм меньше расчетной {sn} мм.");
        }

        return (snp1, snp2, snp, sn);
    }
}