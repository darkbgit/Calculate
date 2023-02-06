using CalculateVessels.Core.Shells.Conical;
using CalculateVessels.Core.Shells.Enums;
using System.Collections.Generic;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class ConicalShellTestData
{
    public static IEnumerable<object[]> GetData()
    {
        var inputData1 = new ConicalShellInput()
        {
            Name = "Тестовая коническая обечайка",
            Steel = "20",
            c1 = 2.0,
            D = 1200,
            c2 = 0.8,
            c3 = 0,
            p = 0.6,
            t = 120,
            s = 8,
            fi = 0.9,
            ny = 2.4,
            IsPressureIn = true,
            alpha1 = 30,
            ConnectionType = ConicalConnectionType.WithoutConnection,
            sT = 0,
            IsConnectionWithLittle = false,
            //D1 = 600,
            r = 0,
            //fi_k = 1,
            //fi_t = 1
        };

        var calculatedData1 = new ConicalShellCalculated()
        {
            InputData = inputData1,
            Ak = 0,
            a1p = 59.419,
            a2p = 0,
            a1p_l = 0,
            a2p_l = 0,
            B1 = 0,
            B1_1 = 0,
            B2 = 0,
            B3 = 0,
            beta = 0,
            beta_0 = 0,
            beta_1 = 0,
            beta_2 = 0,
            beta_3 = 0,
            beta_4 = 0,
            beta_a = 0,
            beta_n = 0,
            beta_t = 0,
            ConditionForBetan = 0,
            c = 2.8,
            chi_1Little = 0,
            cosAlpha1 = 0.866,
            DE = 0,
            DE_1 = 0,
            DE_2 = 0,
            Dk = 1158.407,
            E = 189000,
            IsConditionUseFormulas = true,
            lE = 0,
            SigmaAllow = 140.5,
            SigmaAllowC = 0,
            s = 5.981,
            s_tp = 0,
            s_p = 3.181,
            s_p_1 = 0,
            s_p_2 = 0,
            s_2pLittle = 0,
            sinAlpha1 = 0.5,
            p_d = 0.979,
            p_dp = 0,
            p_dBig = 0,
            p_dLittle = 0,
            tgAlpha1 = 0.577,
        };

        yield return new object[] { inputData1, calculatedData1 };
        // Assert.Equal(3.18, result.s_p, 2);
        // Assert.Equal(0.98, result.p_d, 2);
        // Assert.Equal(189000, result.E, 0);
        // Assert.Equal(140.5, result.SigmaAllow, 1);
    }
}