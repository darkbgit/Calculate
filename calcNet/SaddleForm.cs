using System;
using System.Windows.Forms;

namespace CalculateVessels
{
    public partial class SaddleForm : Form
    {
        public SaddleForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dataIn = new DataSaddle_in
            {
                G = 80000,
                L = 3490,
                Lob = 3410,
                H = 350,
                e = 930,
                p = 0.1,
                D = 1400,
                s = 8,
                s2 = 8,
                c = 2.8,
                fi = 1,
                sigma_d = 145,
                E = 196000,
                type = 2,
                b = 360,
                b2 = 400,
                delta1 = 116,
                delta2 = 140,
                //l0 = 
                steel = "Сталь 20К",
                isPressureIn = true,
                a = 740
            };

            var result = CalcClass.CalcSaddle(dataIn);
            //MakeWord.MakeWord_saddle(dataIn, result, "6724-3");
        }
    }
}
