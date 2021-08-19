using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Data.PhysicalData;

namespace CalculateVessels
{
    public partial class SaddleForm : Form
    {
        public SaddleForm()
        {
            InitializeComponent();
        }

        private void SaddleForm_Load(object sender, EventArgs e)
        {
            var steels = Physical.Gost34233_1.GetSteelsList()?.ToArray<object>();
            if (steels != null)
            {
                steel_cb.Items.AddRange(steels);
                steel_cb.SelectedIndex = 0;
            }
            Gost_cb.SelectedIndex = 0;

            saddle_pb.Image = (Bitmap)new ImageConverter()
                .ConvertFrom(Data.Properties.Resources.SaddleNothingElem);
        }

        private void ShellReinforcement_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not RadioButton { Checked: true } rb) return;

            var name = rb.Name.First().ToString().ToUpper() + rb.Name[1..^3];

            if (name == "Ring")
            {
                name += in_rb.Checked ? "In" : "Out";
                ringLocation_gb.Enabled = true;
            }
            else
            {
                ringLocation_gb.Enabled = false;
            }

            saddle_pb.Image = (Bitmap)new ImageConverter()
                .ConvertFrom(Data.Properties.Resources.ResourceManager
                    .GetObject("Saddle" + name + "Elem"));
        }

        private void InOut_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not RadioButton { Checked: true } rb) return;

            saddle_pb.Image = (Bitmap)new ImageConverter()
                .ConvertFrom(Data.Properties.Resources.ResourceManager
                    .GetObject("SaddleRing" + (in_rb.Checked ? "In" : "Out") + "Elem"));
        }
    }
}
