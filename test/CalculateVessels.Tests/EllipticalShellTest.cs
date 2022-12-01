using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Enums;
using Xunit;

namespace CalculateVessels.Tests
{
    public class EllipticalShellTest
    {
        [Fact]
        public void PressureIn()
        {
            IInputData inputData = new EllipticalShellInputData
            {
                Name = "Тестовая эллиптическая обечайка",
                Steel = "Ст3",
                c1 = 2.0,
                D = 1000,
                c2 = 0.8,
                c3 = 1.2,
                p = 0.6,
                t = 120,
                s = 8,
                fi = 1.0,
                ny = 2.4,
                EllipseH = 250,
                Ellipseh1 = 25,
                EllipticalBottomType = EllipticalBottomType.Elliptical,
                IsPressureIn = true
            };

            var ellipse = new EllipticalShell(inputData);

            ellipse.Calculate();

            var result = ellipse.CalculatedData as EllipticalShellCalculatedData;

            Assert.NotNull(result);

            Assert.Equal(2.04, result.s_p, 2);
            //Assert.Equal(0, result.s_p_1, 0);
            //Assert.Equal(0, result.s_p_2, 0);

            //Assert.Equal(0, result.s, 0);

            Assert.Equal(1.17, result.p_d, 2);
            //Assert.Equal(0, result.p_dp, 0);
            //Assert.Equal(0, result.p_de, 0);

            Assert.Equal(189000, result.E, 0);
            Assert.Equal(147, result.SigmaAllow, 1);

            //Assert.Equal(0, result.c, 0);

            Assert.True(result.IsConditionUseFormulas);

            Assert.Empty(result.ErrorList);

        }

        [Fact]
        public void PressureOut()
        {
            IInputData inputData = new EllipticalShellInputData
            {
                Name = "Тестовая эллиптическая обечайка",
                Steel = "Ст3",
                c1 = 2.0,
                D = 1000,
                c2 = 0.8,
                c3 = 1.2,
                p = 0.6,
                t = 120,
                //E = 189000,
                s = 10,
                //SigmaAllow = 1,
                fi = 1.0,
                ny = 2.4,
                IsPressureIn = false,
                EllipseH = 250,
                Ellipseh1 = 25,
                EllipticalBottomType = EllipticalBottomType.Elliptical
            };

            var ellipse = new EllipticalShell(inputData);

            ellipse.Calculate();

            var result = ellipse.CalculatedData as EllipticalShellCalculatedData;

            Assert.NotNull(result);

            Assert.Equal(4.88, result.s_p, 2);
            //Assert.Equal(0, result.s_p_1, 0);
            //Assert.Equal(0, result.s_p_2, 0);

            //Assert.Equal(0, result.s, 0);

            Assert.Equal(0.74, result.p_d, 2);
            //Assert.Equal(0, result.p_dp, 0);
            //Assert.Equal(0, result.p_de, 0);
            //Assert.Equal(0, result.E, 0);
            //Assert.Equal(0, result.SigmaAllow, 0);
            //Assert.Equal(0, result.c, 0);
            Assert.True(result.IsConditionUseFormulas);

            Assert.Empty(result.ErrorList);

        }

        [Fact]
        public void PressureOutWithHandleSigmaAndE()
        {
            IInputData inputData = new EllipticalShellInputData()
            {
                Name = "Тестовая эллиптическая обечайка",
                Steel = "Ст3",
                c1 = 2.0,
                D = 1000,
                c2 = 0.8,
                c3 = 1.2,
                p = 0.6,
                t = 120,
                E = 189000,
                s = 10,
                SigmaAllow = 147,
                fi = 1.0,
                ny = 2.4,
                IsPressureIn = false,
                EllipseH = 250,
                Ellipseh1 = 25,
                EllipticalBottomType = EllipticalBottomType.Elliptical
            };

            var ellipse = new EllipticalShell(inputData);

            ellipse.Calculate();

            var result = ellipse.CalculatedData as EllipticalShellCalculatedData;

            Assert.NotNull(result);

            //Assert.Equal(0, result.b, 0);
            //Assert.Equal(0, result.b_2, 0);
            //Assert.Equal(0, result.B1, 0);
            //Assert.Equal(0, result.B1_2, 0);
            //Assert.Equal(0, result.ConditionStability, 0);
            //Assert.Equal(0, result.F, 0);
            //Assert.Equal(0, result.FAllow, 0);
            //Assert.Equal(0, result.F_de, 0);
            //Assert.Equal(0, result.F_de1, 0);
            //Assert.Equal(0, result.F_de2, 0);
            //Assert.Equal(0, result.F_dp, 0);
            //Assert.Equal(0, result.l, 0);
            //Assert.Equal(0, result.lambda, 0);
            //Assert.Equal(0, result.lpr, 0);
            //Assert.Equal(0, result.M_d, 0);
            //Assert.Equal(0, result.M_de, 0);
            //Assert.Equal(0, result.M_dp, 0);
            //Assert.Equal(0, result.Q_d, 0);
            //Assert.Equal(0, result.Q_de, 0);
            //Assert.Equal(0, result.Q_dp, 0);
            //Assert.Equal(0, result.s_f, 0);
            //Assert.Equal(0, result.s_pf, 0);
            Assert.Equal(4.88, result.s_p, 2);
            //Assert.Equal(0, result.s_p_1, 0);
            //Assert.Equal(0, result.s_p_2, 0);

            //Assert.Equal(0, result.s, 0);

            Assert.Equal(0.74, result.p_d, 2);
            //Assert.Equal(0, result.p_dp, 0);
            //Assert.Equal(0, result.p_de, 0);
            //Assert.Equal(0, result.E, 0);
            //Assert.Equal(0, result.SigmaAllow, 0);
            //Assert.Equal(0, result.c, 0);
            Assert.True(result.IsConditionUseFormulas);

            Assert.Empty(result.ErrorList);

        }
    }
}