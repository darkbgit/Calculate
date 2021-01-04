using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace calcNet
{
    public partial class MainForm : Form
    {
              
        public MainForm()
        {
            InitializeComponent();
        }

        public CilForm cf = null;
        public NozzleForm nf = null;
        public EllForm ef = null;
        // TODO: KonForm 
        



        private void MakeWord_b_Click(object sender, EventArgs e)
        {
            string f = file_tb.Text;
            if (f != "")
            {
                f += ".docx";
                if (System.IO.File.Exists(f))
                {
                    try
                    {
                        var f1 = System.IO.File.Open(f, System.IO.FileMode.Append);
                        f1.Close();
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("Закройте" + f + "и нажмите OK");
                    }
                    
                }
                else
                {
                    System.IO.File.Copy("temp.docx", f);
                }
                List<int> lit = new List<int>();
                try
                {
                    foreach (DataWordOut.DataOutArrEl item in DataWordOut.DataArr)
                    {
                        int i = item.id;
                        if (i != 0)
                        {
                            switch(DataWordOut.DataArr[i-1].Typ)
                            {
                                case "cil":
                                    MakeWord.MakeWord_cil(DataWordOut.DataArr[i - 1].Data_In, DataWordOut.DataArr[i - 1].Data_Out, f);
                                    lit.Add(2);
                                    break;
                                case "kon":
                                    lit.Add(2);
                                    break;
                                case "ell":
                                    MakeWord.MakeWord_ell(DataWordOut.DataArr[i - 1].Data_In, DataWordOut.DataArr[i - 1].Data_Out, f);
                                    lit.Add(2);
                                    break;
                                case "cilyk":
                                case "konyk":
                                case "ellyk":
                                    MakeWord.MakeWord_nozzle(DataWordOut.DataArr[i - 1].Data_In, DataWordOut.DataArr[i - 1].Data_Out,       DataWordOut.DataArr[i - 1].DataN_In, DataWordOut.DataArr[i - 1].DataN_Out, f);
                                    lit.Add(3);
                                    break;
                                case "saddle":
                                    lit.Add(5);
                                    break;
                                case "heat":
                                    lit.Add(7);
                                    break;
                            }
                        }
                    }
                    MakeWord.MakeLit(lit, f);
                    System.Windows.Forms.MessageBox.Show("OK");
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Error");
                }

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Введите имя файла");
            }
            
            
        }

        private void Cil_b_Click(object sender, EventArgs e)
        {
            //CilForm cf = (CilForm) Application.OpenForms["CilForm"]; // создаем
            if (cf == null)
            {
                //cf.Dispose();
                cf = new CilForm { Owner = this };
                cf.Show();
            }
            else
            {
                cf.Owner = this;
                cf.Show();
            }
        }

        private void Kon_b_Click(object sender, EventArgs e)
        {
            if (nf == null)
            {
                nf = new NozzleForm { Owner = this };
                nf.Show();
            }
            else
            {
                nf.Owner = this;
                nf.Show();
            }
        }

        private void Ell_b_Click(object sender, EventArgs e)
        {
            if (ef == null)
            {
                ef = new EllForm { Owner = this };
                ef.Show();
            }
            else
            {
                ef.Owner = this;
                ef.Show();
            }
        }
    }
}
