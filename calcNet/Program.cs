using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace calc
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
            internal Data_in Data_In;// { get; set; }
            internal Data_out Data_Out;// { get; set; }
            internal string Typ; // cil, ell, kon, cilyk, konyk, ellyk, saddle, heat
        }
        //public static DataOutArrEl[]
        public static DataOutArrEl[] DataArr = new DataOutArrEl[10];

        public static int[] Num = new int[10];

        //public static int[] Num { get => num; set => num = value; }
        //internal static DataOutArrEl[] DataArr { get => dataArr; set => dataArr = value; }

        //public List<DataOutArrEl> DataArr = new List<DataOutArrEl>();// { get; set; }
        //public static DataOutArrEl element = new DataOutArrEl();
    }
}
