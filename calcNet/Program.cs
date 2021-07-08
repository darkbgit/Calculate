using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using CalculateVessels.Core.Interfaces;
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

    public static class Elements
    {
        public static List<IElement> ElementsList { get; set; } = new List<IElement>();
    }



    internal static class SetSteelList
    {
        internal static void SetList(ComboBox cb)
        {
            var steels = Physical.GetSteelsList().ToArray();
            //XmlDocument doc = new XmlDocument();
            //doc.Load(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data\data.xml"));
            //var root = doc.DocumentElement;
            //XmlNodeList steels = root.SelectNodes("sigma_list/steels/steel");
            //foreach (XmlNode steel in steels)
            //{
            //    cb.Items.Add(steel.Attributes["name"].Value);
            //}
            cb.Items.AddRange(steels);
            cb.SelectedIndex = 0;
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
