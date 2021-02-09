using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace calcNet
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

    public static class DataWordOut
    {
        //Dictionary<>
        //   object[] DataArr = new object();
        //    //public static string Value { get; set; }
        //    //public static Data_in Data_In { get; set; }
        //    //public static Data_out Data_Out { get; set; }

        public struct DataOutArrEl
        {
            internal int id;
            internal string Value;// { get; set; }
            internal Data_in Data_In; // { get; set; }
            internal Data_out Data_Out; // { get; set; }
            internal DataNozzle_in DataN_In;
            internal DataNozzle_out DataN_Out;
            internal CalculatedElementType calculatedElementType; // cil, ell, kon, cilyk, konyk, ellyk, saddle, heat
        }

        public static List<DataOutArrEl> DataArr { get; set; } = new List<DataOutArrEl>();

        public static List<IElement> Elements { get; set; } = new List<IElement>();
    }

    public class Set_steellist
    {
        public static void Set_llist(ComboBox cb)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data\data.xml"));
            var root = doc.DocumentElement;
            XmlNodeList steels = root.SelectNodes("sigma_list/steels/steel");
            foreach (XmlNode steel in steels)
            {
                cb.Items.Add(steel.Attributes["name"].Value);
            }
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

    public partial class FormShell : Form
    {
        private DataWordOut.DataOutArrEl dataArrEl;
        internal DataWordOut.DataOutArrEl DataInOutShell { get => dataArrEl; set => dataArrEl = value; }
    }
}
