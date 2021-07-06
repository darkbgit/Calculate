using System;
using System.Windows.Forms;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.DataIn;

namespace CalculateVessels
{
    public partial class MessageBoxCheckBox : Form
    {
        public MessageBoxCheckBox(IElement element, IDataIn dataIn)
        {
            InitializeComponent();
            this.dataIn = dataIn;
            this.element = element;
        }

        private readonly IDataIn dataIn;
        private readonly IElement element;
        
        private void OK_b_Click(object sender, EventArgs e)
        {
            Owner.Hide();
            Close();
            if (nozzle_cb.Checked)
            {
                NozzleForm nf = new NozzleForm(element, (ShellDataIn)dataIn)
                {
                    Owner = this.Owner,
                };
               
                nf.Show();
            }
        }
    }
}
