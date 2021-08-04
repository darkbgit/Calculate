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
    public partial class HeatExchengerWithFixedTubePlatesForm : Form
    {
        public HeatExchengerWithFixedTubePlatesForm()
        {
            InitializeComponent();
        }

        private void HeatExchengerWithFixedTubePlatesForm_Load(object sender, EventArgs e)
        {
            firstTubePlate_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.ConnToFlange1);
            secondTubePlate_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.ConnToFlange1);
            shell_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.Fixed_2_2);
            chamberFlange_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.ConnToFlange1_Flat1);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void IsWithPartitions_cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            partitions_gb.Enabled = cb.Checked;
        }

        private void FirstTubePlate_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb.Checked == true)
            {
                var a = rb.Name.Last().ToString();

                firstTubePlate_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject("ConnToFlange" + a));

                string one = a switch
                {
                    "0" => "1",
                    _ => "2"
                };

                var b = secondTubePlate_gb.Controls.OfType<RadioButton>()
                    .FirstOrDefault(r => r.Checked)
                    .Name.Last().ToString();

                string two = b switch
                {
                    "0" => "1",
                    _ => "2"
                };

                shell_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject("Fixed_" + one + "_" + two));

            }

            

        }

        private void SecondTubePlate_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb.Checked == true)
            {
                var b = rb.Name.Last().ToString();

                secondTubePlate_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject("ConnToFlange" + b));

                string two = b switch
                {
                    "0" => "1",
                    _ => "2"
                };

                var a = firstTubePlate_gb.Controls.OfType<RadioButton>()
                    .FirstOrDefault(r => r.Checked)
                    .Name.Last().ToString();

                string one = a switch
                {
                    "0" => "1",
                    _ => "2"
                };

                shell_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject("Fixed_" + one + "_" + two));
            }

        }

        private void IsChamberFlangeScirt_cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            string type = cb.Checked ? "_Butt" : "_Flat";

            chamberFlange_rb4.Enabled = cb.Checked;

            var a = firstTubePlate_gb.Controls
                .OfType<RadioButton>()
                .FirstOrDefault(r => r.Checked).Name.Last().ToString();

            if (a == "0") a = "1";

            

            var name = flange_gb.Controls
                .OfType<RadioButton>()
                .FirstOrDefault(r => r.Checked)?.Name.Last().ToString();

            chamberFlange_pb.Image = (Bitmap)new ImageConverter()
                    .ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject("ConnToFlange" + a + type + name));


        }

        private void HeatExchengerWithFixedTubePlatesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is not HeatExchengerWithFixedTubePlatesForm) return;

            if (this.Owner is MainForm { heatExchengerWithFixedTubePlatesForm: { } } main)
            {
                main.heatExchengerWithFixedTubePlatesForm = null;
            }
        }

        private void ChamberFlange_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb.Checked)
            {
                var a = firstTubePlate_gb.Controls
                    .OfType<RadioButton>()
                    .FirstOrDefault(r => r.Checked).Name.Last().ToString();

                if (a == "0") a = "1";

                string type = isChamberFlangeScirt_cb.Checked ? "_Butt" : "_Flat";

                var name = rb.Name.Last().ToString();

                chamberFlange_pb.Image = (Bitmap)new ImageConverter()
                        .ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject("ConnToFlange" + a + type + name));
            }
        }
    }
}
