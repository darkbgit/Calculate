using CalculateVessels.Core.Bottoms.Enums;
using CalculateVessels.Core.HeatExchanger;
using CalculateVessels.Core.HeatExchanger.Enums;
using CalculateVessels.Core.Shells;
using CalculateVessels.Core.Shells.DataIn;
using CalculateVessels.Data.PhysicalData;
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

            var steels = Physical.Gost34233_1.GetSteelsList()?.ToArray();
            if (steels != null)
            {
                steel1_cb.Items.AddRange(steels);
                steel2_cb.Items.AddRange(steels);
                steelK_cb.Items.AddRange(steels);
                steelT_cb.Items.AddRange(steels);
                steelD_cb.Items.AddRange(steels);
                steelp_cb.Items.AddRange(steels);
                steel1_cb.SelectedIndex = 0;
                steel2_cb.SelectedIndex = 0;
                steelK_cb.SelectedIndex = 0;
                steelT_cb.SelectedIndex = 0;
                steelD_cb.SelectedIndex = 0;
                steelp_cb.SelectedIndex = 0;

            }
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

            chamberFlange_rb3.Enabled = cb.Checked;

            var a = firstTubePlate_gb.Controls
                .OfType<RadioButton>()
                .FirstOrDefault(r => r.Checked).Name.Last().ToString();

            if (a == "0") a = "1";

            var name = flange_gb.Controls
                .OfType<RadioButton>()
                .FirstOrDefault(r => r.Checked)?.Name.Last().ToString();

            name = (Convert.ToInt32(name) + 1).ToString();

            chamberFlange_pb.Image = (Bitmap)new ImageConverter()
                    .ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject("ConnToFlange" + a + type + name));
        }

        private void HeatExchengerWithFixedTubePlatesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is not HeatExchengerWithFixedTubePlatesForm) return;

            if (this.Owner is MainForm { heatExchangerWithFixedTubePlatesForm: { } } main)
            {
                main.heatExchangerWithFixedTubePlatesForm = null;
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

                name = (Convert.ToInt32(name) + 1).ToString();

                chamberFlange_pb.Image = (Bitmap)new ImageConverter()
                        .ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject("ConnToFlange" + a + type + name));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            heatExchengerDataIn = new();

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

            //t0
            {
                if (double.TryParse(t0_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double t0))
                {
                    heatExchengerDataIn.t0 = t0;
                }
                else
                {
                    dataInErr.Add("t0 неверный ввод");
                }
            }

            //N
            {
                if (int.TryParse(N_tb.Text, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out int N))
                {
                    heatExchengerDataIn.N = N;
                }
                else
                {
                    dataInErr.Add("N неверный ввод");
                }
            }

            heatExchengerDataIn.IsWorkCondition = isWorkCondition_cb.Checked;

            heatExchengerDataIn.IsOneGo = !isNotOneGo_cb.Checked;

            heatExchengerDataIn.IsWithPartitions = isWithPartitions_cb.Checked;

            if (heatExchengerDataIn.IsWithPartitions)
            {
                //l1R
                {
                    if (double.TryParse(l1R_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out double l1R))
                    {
                        heatExchengerDataIn.l1R = l1R;
                    }
                    else
                    {
                        dataInErr.Add("l1R неверный ввод");
                    }
                }

                //l2R
                {
                    if (double.TryParse(l2R_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out double l2R))
                    {
                        heatExchengerDataIn.l2R = l2R;
                    }
                    else
                    {
                        dataInErr.Add("l2R неверный ввод");
                    }
                }

                //cper
                {
                    if (double.TryParse(cper_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out double cper))
                    {
                        heatExchengerDataIn.cper = cper;
                    }
                    else
                    {
                        dataInErr.Add("cper неверный ввод");
                    }
                }

                //sper
                {
                    if (double.TryParse(sper_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out double sper))
                    {
                        heatExchengerDataIn.sper = sper;
                    }
                    else
                    {
                        dataInErr.Add("sper неверный ввод");
                    }
                }
            }

            heatExchengerDataIn.FirstTubePlate.TubePlateType = (TubePlateType)Convert.ToInt32(firstTubePlate_gb.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Name.Last().ToString());


            heatExchengerDataIn.SecondTubePlate.TubePlateType = (TubePlateType)Convert.ToInt32(secondTubePlate_gb.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Name.Last().ToString());

            heatExchengerDataIn.FirstTubePlate.Steel2 = steel2_cb.Text;

            //h2
            {
                if (double.TryParse(h2_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double h2))
                {
                    heatExchengerDataIn.FirstTubePlate.h2 = h2;
                }
                else
                {
                    dataInErr.Add("h2 неверный ввод");
                }
            }

            heatExchengerDataIn.FirstTubePlate.SteelD = steelD_cb.Text;

            //s2
            {
                if (double.TryParse(s2_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double s2))
                {
                    heatExchengerDataIn.FirstTubePlate.s2 = s2;
                }
                else
                {
                    dataInErr.Add("s2 неверный ввод");
                }
            }

            heatExchengerDataIn.FirstTubePlate.Steel1 = steel1_cb.Text;

            //h1
            {
                if (double.TryParse(h1_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double h1))
                {
                    heatExchengerDataIn.FirstTubePlate.h1 = h1;
                }
                else
                {
                    dataInErr.Add("h1 неверный ввод");
                }
            }

            //s1
            {
                if (double.TryParse(s1_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double s1))
                {
                    heatExchengerDataIn.FirstTubePlate.s1 = s1;
                }
                else
                {
                    dataInErr.Add("s1 неверный ввод");
                }
            }

            //DH
            {
                if (double.TryParse(DH_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double DH))
                {
                    heatExchengerDataIn.FirstTubePlate.DH = DH;
                }
                else
                {
                    dataInErr.Add("s1 неверный ввод");
                }
            }

            heatExchengerDataIn.FirstTubePlate.IsChamberFlangeSkirt = isChamberFlangeScirt_cb.Checked;

            heatExchengerDataIn.FirstTubePlate.FlangeFace =
                (FlangeFaceType)Convert.ToInt32(flange_gb.Controls
                                                .OfType<RadioButton>()
                                                .FirstOrDefault(r => r.Checked)
                                                .Name.Last().ToString());

            heatExchengerDataIn.FirstTubePlate.Steelp = steelp_cb.Text;

            //sp
            {
                if (double.TryParse(sp_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double sp))
                {
                    heatExchengerDataIn.FirstTubePlate.sp = sp;
                }
                else
                {
                    dataInErr.Add("sp неверный ввод");
                }
            }

            //c
            {
                if (double.TryParse(c_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double c))
                {
                    heatExchengerDataIn.FirstTubePlate.c = c;
                }
                else
                {
                    dataInErr.Add("c неверный ввод");
                }
            }

            heatExchengerDataIn.IsNeedCheckHardnessTube = isNeedCheckTHardnessTube_cb.Checked;

            heatExchengerDataIn.IsDifferentTubePlate = isDifferentTubePlate_cb.Checked;

            //a1
            {
                if (double.TryParse(a1_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double a1))
                {
                    heatExchengerDataIn.a1 = a1;
                }
                else
                {
                    dataInErr.Add("a1 неверный ввод");
                }
            }

            //DE
            {
                if (double.TryParse(DE_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double DE))
                {
                    heatExchengerDataIn.DE = DE;
                }
                else
                {
                    dataInErr.Add("DE неверный ввод");
                }
            }

            //i
            {
                if (int.TryParse(i_tb.Text, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out int i))
                {
                    heatExchengerDataIn.i = i;
                }
                else
                {
                    dataInErr.Add("i неверный ввод");
                }
            }

            //d0
            {
                if (double.TryParse(d0_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double d0))
                {
                    heatExchengerDataIn.d0 = d0;
                }
                else
                {
                    dataInErr.Add("d0 неверный ввод");
                }
            }

            //tp
            {
                if (double.TryParse(tp_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double tp))
                {
                    heatExchengerDataIn.tp = tp;
                }
                else
                {
                    dataInErr.Add("tp неверный ввод");
                }
            }

            heatExchengerDataIn.IsNeedCheckHardnessTube = isNeedCheckTHardnessTube_cb.Checked;

            heatExchengerDataIn.SteelT = steelT_cb.Text;

            //dT
            {
                if (double.TryParse(dT_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double dT))
                {
                    heatExchengerDataIn.dT = dT;
                }
                else
                {
                    dataInErr.Add("dT неверный ввод");
                }
            }

            //sT
            {
                if (double.TryParse(sT_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double sT))
                {
                    heatExchengerDataIn.sT = sT;
                }
                else
                {
                    dataInErr.Add("sT неверный ввод");
                }
            }

            //l
            {
                if (double.TryParse(l_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out double l))
                {
                    heatExchengerDataIn.l = l;
                }
                else
                {
                    dataInErr.Add("l неверный ввод");
                }
            }

            var tubeRolling = tubeFirstTubePlateFix_gb.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Name;

            switch (tubeRolling)
            {
                case "rollingWithout_rb":
                    heatExchengerDataIn.FirstTubePlate.FixTubeInTubePlate = isWithWeld_cb.Checked
                        ? FixTubeInTubePlateType.RollingWithWelding :
                        FixTubeInTubePlateType.OnlyRolling;
                    heatExchengerDataIn.FirstTubePlate.TubeRolling = TubeRollingType.RollingWithoutGroove;
                    break;
                case "rollingWithOne_rb":
                    heatExchengerDataIn.FirstTubePlate.FixTubeInTubePlate = isWithWeld_cb.Checked
                        ? FixTubeInTubePlateType.RollingWithWelding :
                        FixTubeInTubePlateType.OnlyRolling;
                    heatExchengerDataIn.FirstTubePlate.TubeRolling = TubeRollingType.RollingWithOneGroove;
                    break;
                case "rollingWithTwo_rb":
                    heatExchengerDataIn.FirstTubePlate.FixTubeInTubePlate = isWithWeld_cb.Checked
                        ? FixTubeInTubePlateType.RollingWithWelding :
                        FixTubeInTubePlateType.OnlyRolling;
                    heatExchengerDataIn.FirstTubePlate.TubeRolling = TubeRollingType.RollingWithMoreThenOneGroove;
                    break;
                case "welded_rb":
                    heatExchengerDataIn.FirstTubePlate.FixTubeInTubePlate = FixTubeInTubePlateType.OnlyWelding;
                    break;
            }

            if (heatExchengerDataIn.FirstTubePlate.FixTubeInTubePlate != FixTubeInTubePlateType.OnlyWelding)
            {
                //lB
                {
                    if (double.TryParse(lB_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out double lB))
                    {
                        heatExchengerDataIn.FirstTubePlate.lB = lB;
                    }
                    else
                    {
                        dataInErr.Add("lB неверный ввод");
                    }
                }
            }

            if (heatExchengerDataIn.FirstTubePlate.FixTubeInTubePlate == FixTubeInTubePlateType.OnlyWelding || isWithWeld_cb.Checked)
            {
                //delta
                {
                    if (double.TryParse(delta_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out double delta))
                    {
                        heatExchengerDataIn.FirstTubePlate.delta = delta;
                    }
                    else
                    {
                        dataInErr.Add("delta неверный ввод");
                    }
                }
            }

            heatExchengerDataIn.FirstTubePlate.IsWithGroove = isWithPartitionFirstTubePlate_cb.Checked;

            if (!heatExchengerDataIn.IsOneGo && heatExchengerDataIn.FirstTubePlate.IsWithGroove)
            {
                //sn
                {
                    if (double.TryParse(sn_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out double sn))
                    {
                        heatExchengerDataIn.FirstTubePlate.sn = sn;
                    }
                    else
                    {
                        dataInErr.Add("sn неверный ввод");
                    }
                }

                //s1p
                {
                    if (double.TryParse(s1p_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out double s1p))
                    {
                        heatExchengerDataIn.FirstTubePlate.s1p = s1p;
                    }
                    else
                    {
                        dataInErr.Add("s1p неверный ввод");
                    }
                }

                //BP
                {
                    if (double.TryParse(BP_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out double BP))
                    {
                        heatExchengerDataIn.FirstTubePlate.BP = BP;
                    }
                    else
                    {
                        dataInErr.Add("BP неверный ввод");
                    }
                }

                //tP
                {
                    if (double.TryParse(partition_tP_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out double tP))
                    {
                        if (tP > heatExchengerDataIn.tp)
                        {
                            heatExchengerDataIn.tP = tP;
                        }
                        else
                        {
                            dataInErr.Add("tP должно быть больше tp");
                        }
                    }
                    else
                    {
                        dataInErr.Add("tP неверный ввод");
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

        //private void SetValue()
    }
}
