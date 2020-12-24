using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calc
{
    public partial class FiForm : Form
    {
        public FiForm()
        {
            InitializeComponent();
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // приводим отправителя к элементу типа LinkLabel
            LinkLabel ll = sender as LinkLabel;
            //MessageBox.Show(ll.Text);
            CilForm main = this.Owner as CilForm;
            if (main !=null)
            {
                main.fi_tb.Text = ll.Text;
            }
            this.Close();
        }

        
    }
}
