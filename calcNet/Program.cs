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

    class DataWordOut
    {
        //Dictionary<>
        //   object[] DataArr = new object();
        //    //public static string Value { get; set; }
        //    //public static Data_in Data_In { get; set; }
        //    //public static Data_out Data_Out { get; set; }

        internal struct DataOutArrEl
        {
            public int id;
            public string Value;// { get; set; }
            public Data_in Data_In;// { get; set; }
            public Data_out Data_Out;// { get; set; }
        }

        public static DataOutArrEl[] DataArr = new DataOutArrEl[];// { get; set; }
        //public static DataOutArrEl element = new DataOutArrEl();
    }
}
