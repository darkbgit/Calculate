using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Cylindrical;
using CalculateVessels.Core.Shells.Nozzle;
using CalculateVessels.Core.Shells.Nozzle.Enums;
using Xunit;

namespace CalculateVessels.Tests
{
    public class NozzleTest
    {
        [Fact]
        public void NozzleInCylindricalShell()
        {
            IInputData inputData = new CylindricalShellInputData
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
            };

            var cylinder = new CylindricalShell(inputData);

            cylinder.Calculate();

            var cylindricalShellCalculatedData = cylinder.CalculatedData;

            IInputData nozzleInputData = new NozzleInputData(cylindricalShellCalculatedData)
            {
                t = 150,
                steel1 = "12Х18Н10Т",
                SigmaAllow1 = 0,
                E1 = 0,
                E2 = 0,
                E3 = 0,
                E4 = 0,
                d = 77,
                s1 = 6,
                s2 = 0,
                s3 = 6,
                cs = 2.9,
                cs1 = 2.0,
                l = 0,
                l1 = 130,
                l2 = 0,
                l3 = 5,
                NozzleKind = NozzleKind.PassWithoutRing,
                fi = 1,
                fi1 = 1,
                delta = 6,
                delta1 = 6,
                delta2 = 6,
                steel2 = "",
                steel3 = "12Х18Н10Т",
                steel4 = "",
                Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_1,
                omega = 0,
                tTransversely = 0,
                ellx = 0,
                gamma = 0,
                Name = "Тестовый штуцер",
                IsOval = false,
                d1 = 0,
                d2 = 0,
                r = 0,
                s0 = 0,
                SigmaAllow2 = 0,
                SigmaAllow3 = 0,
                SigmaAllow4 = 0
            };


            var nozzle = new Nozzle(nozzleInputData);

            nozzle.Calculate();

            var result = nozzle.CalculatedData as NozzleCalculatedData;

            Assert.NotNull(result);

            Assert.Equal(22.34, result.d0p, 2);
            Assert.Equal(451.85, result.d0, 2);
            Assert.Equal(600, result.dmax, 0);
            Assert.Equal(2.89, result.p_d, 2);

            Assert.True(result.IsConditionUseFormulas);
            //Assert.Equal(0, result.s_p_1, 0);
        }
    }
}