using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calcNet
{
    public partial class MessageBoxCheckBox : Form
    {
        public MessageBoxCheckBox()
        {
            InitializeComponent();
        }

        private void OK_b_Click(object sender, EventArgs e)
        {

            //this.Close();
            this.Owner.Hide();
            this.Close();
            if (nozzle_cb.Checked)
            {
                NozzleForm nf = new NozzleForm { Owner = this.Owner,
                                                steel1_cb = };
                nf.Show();
            }
            

        }
    }
}
