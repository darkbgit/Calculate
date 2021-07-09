using System;
using System.Windows.Forms;
using System.Xml;
using CalculateVessels.Data.PhysicalData;

namespace CalculateVessels
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }


    enum CalculatedElementType
    {
        Cylindrical,
        CylindricalWhithNozzle,
        Elliptical,
        EllipticalWhithNozzle,
        Conical,
        ConicalWhithNozzle,
        Saddle,
        FlatBottom,
        Heatexchenge
    }
}
