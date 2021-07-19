using CalculateVessels.Data.PhysicalData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculateVessels
{
    public partial class FlatBottomWithAdditionalMomentForm : Form
    {
        public FlatBottomWithAdditionalMomentForm()
        {
            InitializeComponent();
        }



        private void Cancel_b_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void FlatBottomWithAdditionalMoment_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (sender is not FlatBottomWithAdditionalMomentForm) return;

            if (this.Owner is MainForm { flatBottomWithAdditionalMomentForm: { } } main)
            {
                main.flatBottomWithAdditionalMomentForm = null;
            }
            
        }

        private void FlatCover_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                grooveCover_cb.Enabled = true;
                grooveCover_cb.Checked = false;
                cover_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.FlatBottomWithMoment);
            }
            else
            {
                grooveCover_cb.Checked = false;
                grooveCover_cb.Enabled = false;
                //cover_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.pldn15);
            }
        }

        private void FlatBottomWithAdditionalMomentForm_Load(object sender, EventArgs e)
        {
            var steels = Physical.Gost34233_1.GetSteelsList()?.ToArray();
            if (steels != null)
            {
                steel_cb.Items.AddRange(steels);
                steel_cb.SelectedIndex = 0;
            }
            Gost_cb.SelectedIndex = 0;
            cover_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.FlatBottomWithMoment);
            //f_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.PC1);
        }

        private void GrooveCover_cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            
            cover_pb.Image = cb.Checked
                ? (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.FlatBottomWithMomentWithGroove)
                : (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.FlatBottomWithMoment);
            
            s4_1_l.Visible = cb.Checked;
            s4_2_l.Visible = cb.Checked;
            s4_tb.Visible = cb.Checked;
        }

        private void Hole_cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            hole_gb.Enabled = cb.Checked;
        }

        private void Flange_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                string i = rb.Name[0].ToString();
                string type = scirtFlange_cb.Checked ? "fl1_" : "fl2_";
                flange_pb.Image =
                    (Bitmap)new ImageConverter()
                    .ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject(type + i));
            }
        }

        private void ScirtFlange_cb_CheckedChanged(object sender, EventArgs e)
        {
            if (d_flange_rb.Checked)
            {
                a_flange_rb.Checked = true;
            }
            //UNDONE
        }
    }
}
