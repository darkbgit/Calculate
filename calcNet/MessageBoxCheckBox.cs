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
                NozzleForm nf = new NozzleForm { Owner = this.Owner };
                if (this.Owner is CilForm cf)
                {
                    DataWordOut.DataOutArrEl data_InOut = new DataWordOut.DataOutArrEl
                    {
                        Data_In = cf.DataArrEl.Data_In,
                        Data_Out = cf.DataArrEl.Data_Out
                    };
                    nf.DataArrEl = data_InOut;
                }
                else if (this.Owner is EllForm ef)
                {
                    DataWordOut.DataOutArrEl data_InOut = new DataWordOut.DataOutArrEl
                    {
                        Data_In = ef.DataArrEl.Data_In,
                        Data_Out = ef.DataArrEl.Data_Out
                    };
                    nf.DataArrEl = data_InOut;
                }
                
                
                
                nf.Show();
            }
            

        }
    }
}
