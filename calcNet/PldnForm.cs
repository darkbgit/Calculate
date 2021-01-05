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
    public partial class PldnForm : Form
    {
        public PldnForm()
        {
            InitializeComponent();
        }

        private void Rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                type_pb.Image = (Bitmap)calcNet.Properties.Resources.ResourceManager.GetObject("pldn" + rb.Text);
            }
        }

        private void PldnForm_Load(object sender, EventArgs e)
        {
            Set_steellist.Set_llist(steel_cb);
            steel_cb.SelectedIndex = 0;
            Gost_cb.SelectedIndex = 0;
        }
    }
}
