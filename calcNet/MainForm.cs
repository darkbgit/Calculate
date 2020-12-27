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

namespace calc
{
    public partial class MainForm : Form
    {
              
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cil_b_Click(object sender, EventArgs e)
        {
            //CilForm cf = (CilForm) Application.OpenForms["CilForm"]; // создаем
            //if (cf == null)
            //{
            CilForm cf = new CilForm();
            cf.Owner = this;
            cf.ShowDialog();
            //}
            //else
            //{
              //  cf.Owner = this;
                //cf.Activate();
            //}
        }

        private void открытьРасчетToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

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
                MakeWord mw = new MakeWord();

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
                                    mw.MakeWord_cil(DataWordOut.DataArr[i - 1].Data_In, DataWordOut.DataArr[i - 1].Data_Out, f);
                                    break;
                                case "kon":
                                    break;
                                case "ell":
                                    mw.MakeWord_ell(DataWordOut.DataArr[i - 1].Data_In, DataWordOut.DataArr[i - 1].Data_Out, f);
                                    break;
                                case "cilyk":
                                    break;
                                case "konyk":
                                    break;
                                case "ellyk":
                                    break;
                                case "saddle":
                                    break;
                                case "heat":
                                    break;
                            }
                        }
                    }
                    //mw.MakeWord_cil(DataWordOut.Data_In, DataWordOut.Data_Out, "1.docx");
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
    }
}
