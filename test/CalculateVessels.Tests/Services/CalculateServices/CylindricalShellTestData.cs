using System.Collections;
using System.Collections.Generic;
using CalculateVessels.Core.Shells.Cylindrical;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class CylindricalShellTestData
{
    public static IEnumerable<object[]> GetData()
    {
        var inputData1 = new CylindricalShellInput
        {
            Name = "Тестовая цилиндрическая обечайка",
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
            IsPressureIn = true
        };

        var calculatedData1 = new CylindricalShellCalculated()
        {
            InputData = inputData1,
            c = 2.8,
            s_p = 2.854,
            s = 5.654,
            p_de = 0,
            p_d = 1.091,
            SigmaAllow = 140.5,
            //ErrorList => _errorList;
            IsConditionUseFormulas = true,
            //_errorList = new();
            b = 0,
            b_2 = 0,
            B1 = 0,
            B1_2 = 0,
            ConditionStability = 0.55,
            F = 0,
            FAllow = 0,
            F_de = 0,
            F_de1 = 0,
            F_de2 = 0,
            F_dp = 0,
            l = 0,
            lambda = 0,
            lpr = 0,
            M_d = 0,
            M_de = 0,
            M_dp = 0,
            Q_d = 0,
            Q_de = 0,
            Q_dp = 0,
            s_f = 0,
            s_pf = 0,
            s_p_1 = 0,
            s_p_2 = 0,
            p_dp = 0,
            E = 189000
        };

        var inputData2 = new CylindricalShellInput
        {
            Name = "Тестовая цилиндрическая обечайка",
            Steel = "20",
            c1 = 2.0,
            D = 1200,
            c2 = 0.8,
            c3 = 0,
            p = 0.6,
            t = 120,
            //E = 189000,
            s = 12,
            //SigmaAllow = 1,
            fi = 0.9,
            ny = 2.4,
            IsPressureIn = false,
            l = 1500,
        };
        var calculatedData2 = new CylindricalShellCalculated()
        {
            InputData = inputData2,
            c = 2.8,
            s_p = 8.789,
            s = 11.589,
            p_de = 0.674,
            p_d = 0.643,
            SigmaAllow = 140.5,
            //ErrorList => _errorList;
            IsConditionUseFormulas = true,
            //_errorList = new();
            b = 1,
            b_2 = 0.476,
            B1 = 1,
            B1_2 = 8.634,
            ConditionStability = 0.933,
            F = 0,
            FAllow = 0,
            F_de = 0,
            F_de1 = 0,
            F_de2 = 0,
            F_dp = 0,
            l = 1500,
            lambda = 0,
            lpr = 0,
            M_d = 0,
            M_de = 0,
            M_dp = 0,
            Q_d = 0,
            Q_de = 0,
            Q_dp = 0,
            s_f = 0,
            s_pf = 0,
            s_p_1 = 8.789,
            s_p_2 = 3.081,
            p_dp = 2.138,
            E = 189000
        };

        var inputData3 = new CylindricalShellInput
        {
            Name = "Тестовая цилиндрическая обечайка",
            Steel = "20",
            c1 = 2.0,
            D = 1200,
            c2 = 0.8,
            c3 = 0,
            p = 0.6,
            t = 120,
            E = 189000,
            s = 12,
            SigmaAllow = 140.5,
            fi = 0.9,
            ny = 2.4,
            F = 0,
            q = 0,
            M = 0,
            Q = 0,
            IsPressureIn = false,
            l = 1500,
            l3 = 0,
            fi_t = 0,
            ConditionForCalcF5341 = false,
            FCalcSchema = 1,
            f = 0,
            IsFTensile = false
        };
        var calculatedData3 = new CylindricalShellCalculated()
        {
            InputData = inputData3,
            c = 2.8,
            s_p = 8.789,
            s = 11.589,
            p_de = 0.674,
            p_d = 0.643,
            SigmaAllow = 140.5,
            IsConditionUseFormulas = true,
            b = 1,
            b_2 = 0.476,
            B1 = 1,
            B1_2 = 8.634,
            ConditionStability = 0.933,
            F = 0,
            FAllow = 0,
            F_de = 0,
            F_de1 = 0,
            F_de2 = 0,
            F_dp = 0,
            l = 1500,
            lambda = 0,
            lpr = 0,
            M_d = 0,
            M_de = 0,
            M_dp = 0,
            Q_d = 0,
            Q_de = 0,
            Q_dp = 0,
            s_f = 0,
            s_pf = 0,
            s_p_1 = 8.789,
            s_p_2 = 3.081,
            p_dp = 2.138,
            E = 189000
        };

        var inputData4 = new CylindricalShellInput
        {
            Name = "Тестовая цилиндрическая обечайка",
            Steel = "12Х18Н10Т",
            c1 = 2.0,
            D = 600,
            c2 = 0.8,
            c3 = 0,
            p = 0.6,
            t = 150,
            s = 8,
            fi = 1,
            ny = 2.4,
            IsPressureIn = true
            //Name = "Тестовая цилиндрическая обечайка",
            //Steel = "20",
            //c1 = 2.0,
            //D = 1200,
            //c2 = 0.8,
            //c3 = 0,
            //p = 0.6,
            //t = 120,
            //E = 189000,
            //s = 12,
            //SigmaAllow = 140.5,
            //fi = 0.9,
            //ny = 2.4,
            //F = 0,
            //q = 0,
            //M = 0,
            //Q = 0,
            //IsPressureIn = false,
            //l = 1500,
            //l3 = 0,
            //fi_t = 0,
            //ConditionForCalcF5341 = false,
            //FCalcSchema = 1,
            //f = 0,
            //IsFTensile = false
        };
        var calculatedData4 = new CylindricalShellCalculated()
        {
            InputData = inputData3,
            c = 2.8,
            s_p = 1.073,
            s = 3.873,
            p_de = 0,
            p_d = 2.887,
            SigmaAllow = 168,
            IsConditionUseFormulas = true,
            b = 0,
            b_2 = 0,
            B1 = 0,
            B1_2 = 0,
            ConditionStability = 0.208,
            F = 0,
            FAllow = 0,
            F_de = 0,
            F_de1 = 0,
            F_de2 = 0,
            F_dp = 0,
            l = 0,
            lambda = 0,
            lpr = 0,
            M_d = 0,
            M_de = 0,
            M_dp = 0,
            Q_d = 0,
            Q_de = 0,
            Q_dp = 0,
            s_f = 0,
            s_pf = 0,
            s_p_1 = 0,
            s_p_2 = 0,
            p_dp = 0,
            E = 199000
        };

        yield return new object[] { inputData1, calculatedData1 };
        yield return new object[] { inputData2, calculatedData2 };
        yield return new object[] { inputData3, calculatedData3 };
        yield return new object[] { inputData4, calculatedData4 };
    }
}