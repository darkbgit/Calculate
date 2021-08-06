using CalculateVessels.Core.HeatExchanger;
using CalculateVessels.Core.Shells;
using CalculateVessels.Core.Shells.DataIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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

        private HeatExchangerDataIn heatExchengerDataIn;

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

        private void button1_Click(object sender, EventArgs e)
        {


            List<string> dataInErr = new();

            //name
            heatExchengerDataIn.Name = Name_tb.Text;

            //SteelK
            heatExchengerDataIn.SteelK = steelK_cb.Text;

            //D
            {
                if (double.TryParse(D_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double D))
                {
                    heatExchengerDataIn.D = D;
                }
                else
                {
                    dataInErr.Add("D неверный ввод");
                }
            }

            //sK
            {
                if (double.TryParse(sK_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double sK))
                {
                    heatExchengerDataIn.sK = sK;
                }
                else
                {
                    dataInErr.Add("sK неверный ввод");
                }
            }

            //cK
            {
                if (double.TryParse(cK_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double cK))
                {
                    heatExchengerDataIn.cK = cK;
                }
                else
                {
                    dataInErr.Add("cK неверный ввод");
                }
            }

            //pM
            {
                if (double.TryParse(pM_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double pM))
                {
                    heatExchengerDataIn.pM = pM;
                }
                else
                {
                    dataInErr.Add("pM неверный ввод");
                }
            }

            //tK
            {
                if (double.TryParse(tK_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double tK))
                {
                    heatExchengerDataIn.tK = tK;
                }
                else
                {
                    dataInErr.Add("tK неверный ввод");
                }
            }

            //TK
            {
                if (double.TryParse(TCalculateK_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double TK))
                {
                    heatExchengerDataIn.TK = TK;
                }
                else
                {
                    dataInErr.Add("TK неверный ввод");
                }
            }

            //pT
            {
                if (double.TryParse(pT_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double pT))
                {
                    heatExchengerDataIn.pT = pT;
                }
                else
                {
                    dataInErr.Add("pT неверный ввод");
                }
            }

            //tT
            {
                if (double.TryParse(tT_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double tT))
                {
                    heatExchengerDataIn.tT = tT;
                }
                else
                {
                    dataInErr.Add("tT неверный ввод");
                }
            }

            //TT
            {
                if (double.TryParse(TCalculateT_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double TT))
                {
                    heatExchengerDataIn.TT = TT;
                }
                else
                {
                    dataInErr.Add("TK неверный ввод");
                }
            }

            heatExchengerDataIn.IsWorkCondition = isWorkCondition_cb.Checked;

            heatExchengerDataIn.IsOneGo = !isNotOneGo_cb.Checked;

            if (stressHand_rb.Checked)
            {
                //Q
                {
                    if (double.TryParse(Q_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out double Q))
                    {
                        heatExchengerDataIn.Q = Q;
                    }
                    else
                    {
                        dataInErr.Add("Q неверный ввод");
                    }
                }

                //M
                {
                    if (double.TryParse(M_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out double M))
                    {
                        heatExchengerDataIn.M = M;
                    }
                    else
                    {
                        dataInErr.Add("M неверный ввод");
                    }
                }

                heatExchengerDataIn.IsFTensile = forceStretch_rb.Checked;

                //F
                {
                    if (double.TryParse(F_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out double F))
                    {
                        heatExchengerDataIn.F = F;
                    }
                    else
                    {
                        dataInErr.Add("F неверный ввод");
                    }
                }

                if (!heatExchengerDataIn.IsFTensile)
                {
                    var idx = force_gb.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked)?.Text;
                    if (int.TryParse(idx, NumberStyles.Integer, CultureInfo.InvariantCulture, out int i))
                    {
                        heatExchengerDataIn.FCalcSchema = i;

                        switch (i)
                        {
                            case 5:
                                if (double.TryParse(F_tb.Text, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out double q))
                                {
                                    heatExchengerDataIn.q = q;
                                }
                                else
                                {
                                    dataInErr.Add("q неверный ввод");
                                }
                                break;
                            case 6:
                            case 7:
                                if (double.TryParse(F_tb.Text, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out double f))
                                {
                                    heatExchengerDataIn.f = f;
                                }
                                else
                                {
                                    dataInErr.Add("f неверный ввод");
                                }
                                break;
                        }
                    }
                    else
                    {
                        dataInErr.Add("Не возможно определить тип сжимающего усилия");
                    }
                }
            }

            bool isNotError = dataInErr.Count == 0 && heatExchengerDataIn.IsDataGood;

            if (isNotError)
            {
                HeatExchanger heatExchanger = new(heatExchengerDataIn);
                heatExchanger.Calculate();
                if (!heatExchanger.IsCriticalError)
                {
                    if (this.Owner is MainForm main)
                    {
                        main.Word_lv.Items.Add(heatExchanger.ToString());
                        Elements.ElementsList.Add(heatExchanger);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("MainForm Error");
                    }

                    if (heatExchanger.IsError)
                    {
                        MessageBox.Show(string.Join<string>(Environment.NewLine, heatExchanger.ErrorList));
                    }

                    MessageBox.Show("Calculation complete");
                }
                else
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, heatExchanger.ErrorList));
                }
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(heatExchengerDataIn.ErrorList)));
            }
        }
    }
}
