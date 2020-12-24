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
            MakeWord mw = new MakeWord();
            try
            {
                foreach (DataWordOut.DataOutArrEl item in DataWordOut.DataArr)
                {
                    int i = item.id;
                    mw.MakeWord_cil(DataWordOut.DataArr[i].Data_In, DataWordOut.DataArr[i].Data_Out, "1.docx");
                }
                //mw.MakeWord_cil(DataWordOut.Data_In, DataWordOut.Data_Out, "1.docx");
                System.Windows.Forms.MessageBox.Show("OK");
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Error");
            }
            
        }
    }
}
