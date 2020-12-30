﻿using System;
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
                NozzleForm nf = new NozzleForm { Owner = this.Owner };
                if (this.Owner is CilForm cf)
                {
                    nf.dataArrEl.Data_In = cf.dataArrEl.Data_In;
                    nf.dataArrEl.Data_Out = cf.dataArrEl.Data_Out;
                }
                else if (this.Owner is EllForm ef)
                {
                    nf.dataArrEl.Data_In = ef.dataArrEl.Data_In;
                    nf.dataArrEl.Data_Out = ef.dataArrEl.Data_Out;
                }
                
                
                
                nf.Show();
            }
            

        }
    }
}
