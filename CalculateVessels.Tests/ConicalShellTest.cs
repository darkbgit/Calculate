using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Conical;
using CalculateVessels.Core.Shells.CylindricalShell;
using CalculateVessels.Core.Shells.Enums;
using Xunit;

namespace CalculateVessels.Tests
{
    public class ConicalShellTest
    {
        [Fact]
        public void PressureIn()
        {
            IInputData inputData = new ConicalShellInputData()
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
                alfa1 = 30,
                s1 = 8,
                ConnectionType = ConicalConnectionType.WithoutConnection,
                s2 = 8,
                sT = 0,
                IsConnectionWithLittle = true,
                D1 = 600,
                r = 0,
                fi_k = 1,
                fi_t = 1
            };

            var cone = new ConicalShell(inputData);

            cone.Calculate();

            var result = cone.CalculatedData as ConicalShellCalculatedData;

            Assert.NotNull(result);

            Assert.Equal(3.18, result.s_p, 2);
            //Assert.Equal(0, result.s_p_1, 0);
            //Assert.Equal(0, result.s_p_2, 0);

            //Assert.Equal(0, result.s, 0);

            Assert.Equal(0.98, result.p_d, 2);
            //Assert.Equal(0, result.p_dp, 0);
            //Assert.Equal(0, result.p_de, 0);

            Assert.Equal(189000, result.E, 0);
            Assert.Equal(140.5, result.SigmaAllow, 1);

            //Assert.Equal(0, result.c, 0);

            Assert.True(result.IsConditionUseFormulas);

            Assert.Empty(result.ErrorList);

        }

        //[Fact]
        //public void PressureOut()
        //{
        //    IInputData inputData = new CylindricalShellInputData
        //    {
        //        Name = "Тестовая цилиндрическая обечайка",
        //        Steel = "20",
        //        c1 = 2.0,
        //        D = 1200,
        //        c2 = 0.8,
        //        c3 = 0,
        //        p = 0.6,
        //        t = 120,
        //        //E = 189000,
        //        s = 12,
        //        //SigmaAllow = 1,
        //        fi = 0.9,
        //        ny = 2.4,
        //        IsPressureIn = false,
        //        l = 1500,
        //    };

        //    var cylinder = new CylindricalShell(inputData);

        //    cylinder.Calculate();

        //    var result = cylinder.CalculatedData as CylindricalShellCalculatedData;

        //    Assert.NotNull(result);

        //    Assert.Equal(8.79, result.s_p, 2);
        //    //Assert.Equal(0, result.s_p_1, 0);
        //    //Assert.Equal(0, result.s_p_2, 0);

        //    //Assert.Equal(0, result.s, 0);

        //    Assert.Equal(0.64, result.p_d, 2);
        //    //Assert.Equal(0, result.p_dp, 0);
        //    //Assert.Equal(0, result.p_de, 0);
        //    //Assert.Equal(0, result.E, 0);
        //    //Assert.Equal(0, result.SigmaAllow, 0);
        //    //Assert.Equal(0, result.c, 0);
        //    Assert.True(result.IsConditionUseFormulas);

        //    Assert.Empty(result.ErrorList);

        //}

        //[Fact]
        //public void PressureOutWithHandleSigmaAndE()
        //{
        //    IInputData inputData = new CylindricalShellInputData
        //    {
        //        Name = "Тестовая цилиндрическая обечайка",
        //        Steel = "20",
        //        c1 = 2.0,
        //        D = 1200,
        //        c2 = 0.8,
        //        c3 = 0,
        //        p = 0.6,
        //        t = 120,
        //        E = 189000,
        //        s = 12,
        //        SigmaAllow = 140.5,
        //        fi = 0.9,
        //        ny = 2.4,
        //        F = 0,
        //        q = 0,
        //        M = 0,
        //        Q = 0,
        //        IsPressureIn = false,
        //        l = 1500,
        //        l3 = 0,
        //        fi_t = 0,
        //        ConditionForCalcF5341 = false,
        //        FCalcSchema = 1,
        //        f = 0,
        //        IsFTensile = false
        //    };

        //    var cylinder = new CylindricalShell(inputData);

        //    cylinder.Calculate();

        //    var result = cylinder.CalculatedData as CylindricalShellCalculatedData;

        //    Assert.NotNull(result);

        //    //Assert.Equal(0, result.b, 0);
        //    //Assert.Equal(0, result.b_2, 0);
        //    //Assert.Equal(0, result.B1, 0);
        //    //Assert.Equal(0, result.B1_2, 0);
        //    //Assert.Equal(0, result.ConditionStability, 0);
        //    //Assert.Equal(0, result.F, 0);
        //    //Assert.Equal(0, result.FAllow, 0);
        //    //Assert.Equal(0, result.F_de, 0);
        //    //Assert.Equal(0, result.F_de1, 0);
        //    //Assert.Equal(0, result.F_de2, 0);
        //    //Assert.Equal(0, result.F_dp, 0);
        //    //Assert.Equal(0, result.l, 0);
        //    //Assert.Equal(0, result.lambda, 0);
        //    //Assert.Equal(0, result.lpr, 0);
        //    //Assert.Equal(0, result.M_d, 0);
        //    //Assert.Equal(0, result.M_de, 0);
        //    //Assert.Equal(0, result.M_dp, 0);
        //    //Assert.Equal(0, result.Q_d, 0);
        //    //Assert.Equal(0, result.Q_de, 0);
        //    //Assert.Equal(0, result.Q_dp, 0);
        //    //Assert.Equal(0, result.s_f, 0);
        //    //Assert.Equal(0, result.s_pf, 0);
        //    Assert.Equal(8.79, result.s_p, 2);
        //    //Assert.Equal(0, result.s_p_1, 0);
        //    //Assert.Equal(0, result.s_p_2, 0);

        //    //Assert.Equal(0, result.s, 0);

        //    Assert.Equal(0.64, result.p_d, 2);
        //    //Assert.Equal(0, result.p_dp, 0);
        //    //Assert.Equal(0, result.p_de, 0);
        //    //Assert.Equal(0, result.E, 0);
        //    //Assert.Equal(0, result.SigmaAllow, 0);
        //    //Assert.Equal(0, result.c, 0);
        //    Assert.True(result.IsConditionUseFormulas);

        //    Assert.Empty(result.ErrorList);

        //}
    }
}