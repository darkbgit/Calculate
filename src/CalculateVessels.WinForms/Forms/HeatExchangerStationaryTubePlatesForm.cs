using CalculateVessels.Core.Elements.HeatExchangers.Enums;
using CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerStationaryTubePlates;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Forms.MiddleForms;
using CalculateVessels.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels.Forms;

public sealed partial class HeatExchangerStationaryTubePlatesForm : HeatExchangerStationaryTubePlatesFormMiddle
{
    public HeatExchangerStationaryTubePlatesForm(IEnumerable<ICalculateService<HeatExchangerStationaryTubePlatesInput>> calculateServices,
        IPhysicalDataService physicalDataService)
        : base(calculateServices, physicalDataService)
    {
        InitializeComponent();
    }

    protected override string GetServiceName()
    {
        return Gost_cb.Text;
    }

    protected override void LoadInputData()
    {
        throw new NotImplementedException();
    }

    protected override bool CollectDataForPreliminarilyCalculation()
    {
        List<string> dataInErr = new();

        InputData = new HeatExchangerStationaryTubePlatesInput
        {
            SteelK = steelK_cb.Text,
            D = Parameters.GetParam<double>(D_tb.Text, "D", dataInErr),
            sK = Parameters.GetParam<double>(sK_tb.Text, "sK", dataInErr),
            cK = Parameters.GetParam<double>(cK_tb.Text, "cK", dataInErr),
            pM = Parameters.GetParam<double>(pM_tb.Text, "pM", dataInErr),
            tK = Parameters.GetParam<double>(tK_tb.Text, "tK", dataInErr),
            TK = Parameters.GetParam<double>(TCalculateK_tb.Text, "TK", dataInErr),
            pT = Parameters.GetParam<double>(pT_tb.Text, "pT", dataInErr),
            tT = Parameters.GetParam<double>(tT_tb.Text, "tT", dataInErr),
            TT = Parameters.GetParam<double>(TCalculateT_tb.Text, "TT", dataInErr),
            t0 = Parameters.GetParam<double>(t0_tb.Text, "t0", dataInErr),
            N = Parameters.GetParam<int>(N_tb.Text, "N", dataInErr, NumberStyles.Integer),
            IsWorkCondition = isWorkCondition_cb.Checked,
            IsOneGo = !isNotOneGo_cb.Checked,
            IsWithPartitions = isWithPartitions_cb.Checked,
        };

        if (InputData.IsWithPartitions)
        {
            InputData.l1R = Parameters.GetParam<double>(l1R_tb.Text, "l1R", dataInErr);
            InputData.l2R = Parameters.GetParam<double>(l2R_tb.Text, "l2R", dataInErr);
            InputData.cper = Parameters.GetParam<double>(cper_tb.Text, "cper", dataInErr);
            InputData.sper = Parameters.GetParam<double>(sper_tb.Text, "sper", dataInErr);
        }



        InputData.FirstTubePlate.TubePlateType =
            (TubePlateType)Convert.ToInt32(firstTubePlate_gb.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked)
            ?.Name.LastOrDefault().ToString());

        InputData.SecondTubePlate.TubePlateType =
            (TubePlateType)Convert.ToInt32(secondTubePlate_gb.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked)
            ?.Name.LastOrDefault().ToString());

        InputData.FirstTubePlate.Steel2 = steel2_cb.Text;


        InputData.FirstTubePlate.h2 = Parameters.GetParam<double>(h2_tb.Text, "h2", dataInErr);
        InputData.FirstTubePlate.SteelD = steelD_cb.Text;

        InputData.FirstTubePlate.s2 = Parameters.GetParam<double>(s2_tb.Text, "s2", dataInErr);
        InputData.FirstTubePlate.Steel1 = steel1_cb.Text;

        InputData.FirstTubePlate.h1 = Parameters.GetParam<double>(h1_tb.Text, "h1", dataInErr);
        InputData.FirstTubePlate.s1 = Parameters.GetParam<double>(s1_tb.Text, "s1", dataInErr);

        InputData.FirstTubePlate.DH = Parameters.GetParam<double>(DH_tb.Text, "DH", dataInErr);

        InputData.FirstTubePlate.IsChamberFlangeSkirt = isChamberFlangeScirt_cb.Checked;

        InputData.FirstTubePlate.FlangeFace =
            (FlangeFaceType)Convert.ToInt32(flange_gb.Controls
                .OfType<RadioButton>()
                .FirstOrDefault(r => r.Checked)
                ?.Name.Last().ToString());

        InputData.FirstTubePlate.Steelp = steelp_cb.Text;
        InputData.FirstTubePlate.sp = Parameters.GetParam<double>(sp_tb.Text, "sp", dataInErr);

        InputData.FirstTubePlate.c = Parameters.GetParam<double>(c_tb.Text, "c", dataInErr);


        InputData.IsNeedCheckHardnessTube = isNeedCheckTHardnessTube_cb.Checked;
        InputData.IsDifferentTubePlate = isDifferentTubePlate_cb.Checked;

        InputData.a1 = Parameters.GetParam<double>(a1_tb.Text, "a1", dataInErr);
        InputData.DE = Parameters.GetParam<double>(DE_tb.Text, "DE", dataInErr);
        InputData.i = Parameters.GetParam<int>(i_tb.Text, "i", dataInErr, NumberStyles.Integer);
        InputData.d0 = Parameters.GetParam<double>(d0_tb.Text, "d0", dataInErr);
        InputData.tp = Parameters.GetParam<double>(tp_tb.Text, "tp", dataInErr);

        InputData.IsNeedCheckHardnessTube = isNeedCheckTHardnessTube_cb.Checked;
        InputData.SteelT = steelT_cb.Text;

        InputData.dT = Parameters.GetParam<double>(dT_tb.Text, "dT", dataInErr);
        InputData.sT = Parameters.GetParam<double>(sT_tb.Text, "sT", dataInErr);
        InputData.l = Parameters.GetParam<double>(l_tb.Text, "l", dataInErr);

        var tubeRolling = tubeFirstTubePlateFix_gb.Controls
            .OfType<RadioButton>()
            .FirstOrDefault(r => r.Checked)
            ?.Name;

        switch (tubeRolling)
        {
            case nameof(rollingWithout_rb):
                InputData.FirstTubePlate.FixTubeInTubePlate = isWithWeld_cb.Checked
                    ? FixTubeInTubePlateType.RollingWithWelding :
                    FixTubeInTubePlateType.OnlyRolling;
                InputData.FirstTubePlate.TubeRolling = TubeRollingType.RollingWithoutGroove;
                break;
            case nameof(rollingWithOne_rb):
                InputData.FirstTubePlate.FixTubeInTubePlate = isWithWeld_cb.Checked
                    ? FixTubeInTubePlateType.RollingWithWelding :
                    FixTubeInTubePlateType.OnlyRolling;
                InputData.FirstTubePlate.TubeRolling = TubeRollingType.RollingWithOneGroove;
                break;
            case nameof(rollingWithTwo_rb):
                InputData.FirstTubePlate.FixTubeInTubePlate = isWithWeld_cb.Checked
                    ? FixTubeInTubePlateType.RollingWithWelding :
                    FixTubeInTubePlateType.OnlyRolling;
                InputData.FirstTubePlate.TubeRolling = TubeRollingType.RollingWithMoreThenOneGroove;
                break;
            case nameof(welded_rb):
                InputData.FirstTubePlate.FixTubeInTubePlate = FixTubeInTubePlateType.OnlyWelding;
                break;
            default:
                dataInErr.Add("Error get tube rolling type.");
                break;
        }

        if (InputData.FirstTubePlate.FixTubeInTubePlate != FixTubeInTubePlateType.OnlyWelding)
        {
            InputData.FirstTubePlate.lB = Parameters.GetParam<double>(lB_tb.Text, "lB", dataInErr);
        }

        if (InputData.FirstTubePlate.FixTubeInTubePlate == FixTubeInTubePlateType.OnlyWelding || isWithWeld_cb.Checked)
        {
            InputData.FirstTubePlate.delta = Parameters.GetParam<double>(delta_tb.Text, "delta", dataInErr);
        }

        InputData.FirstTubePlate.IsWithGroove = isWithPartitionFirstTubePlate_cb.Checked;

        if (InputData is { IsOneGo: false, FirstTubePlate.IsWithGroove: true })
        {
            InputData.FirstTubePlate.sn = Parameters.GetParam<double>(sn_tb.Text, "sn", dataInErr);
            InputData.FirstTubePlate.s1p = Parameters.GetParam<double>(s1p_tb.Text, "s1p", dataInErr);
            InputData.FirstTubePlate.BP = Parameters.GetParam<double>(BP_tb.Text, "BP", dataInErr);
            InputData.tP = Parameters.GetParam<double>(partition_tP_tb.Text, "tP", dataInErr);

            if (InputData.tP > InputData.tp)
            {
                InputData.tP = InputData.tp;
            }
            else
            {
                dataInErr.Add("tP должно быть больше tp");
            }
        }


        var isNoError = !dataInErr.Any() && InputData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(InputData.ErrorList)));
        }

        return isNoError;
    }

    protected override bool CollectDataForFinishCalculation()
    {
        if (InputData == null) throw new InvalidOperationException();

        var dataInErr = new List<string>();

        InputData.Name = Name_tb.Text;

        var isNoError = !dataInErr.Any() && InputData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(InputData.ErrorList)));
        }

        return isNoError;
    }

    private void HeatExchangerWithFixedTubePlatesForm_Load(object sender, EventArgs e)
    {
        firstTubePlate_pb.Image = FormHelpers.GetBitmapFromBytes(Resources.ConnToFlange1);
        secondTubePlate_pb.Image = FormHelpers.GetBitmapFromBytes(Resources.ConnToFlange1);
        shell_pb.Image = FormHelpers.GetBitmapFromBytes(Resources.Fixed_2_2);
        chamberFlange_pb.Image = FormHelpers.GetBitmapFromBytes(Resources.ConnToFlange1_Flat1);

        LoadSteelsToComboBox(steel1_cb, SteelSource.G34233D1);
        LoadSteelsToComboBox(steel2_cb, SteelSource.G34233D1);
        LoadSteelsToComboBox(steelK_cb, SteelSource.G34233D1);
        LoadSteelsToComboBox(steelT_cb, SteelSource.G34233D1);
        LoadSteelsToComboBox(steelD_cb, SteelSource.G34233D1);
        LoadSteelsToComboBox(steelp_cb, SteelSource.G34233D1);
        LoadCalculateServicesNamesToComboBox(Gost_cb);
    }

    private void label3_Click(object sender, EventArgs e)
    {

    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {

    }

    private void IsWithPartitions_cb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is CheckBox cb) partitions_gb.Enabled = cb.Checked;
    }

    private void FirstTubePlate_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        var a = rb.Name.Last().ToString();

        firstTubePlate_pb.Image = FormHelpers.GetBitmapFromResource("ConnToFlange" + a);

        var one = a switch
        {
            "0" => "1",
            _ => "2"
        };

        var b = secondTubePlate_gb.Controls.OfType<RadioButton>()
            .FirstOrDefault(r => r.Checked)
            ?.Name.LastOrDefault().ToString();

        var two = b switch
        {
            "0" => "1",
            _ => "2"
        };

        shell_pb.Image = FormHelpers.GetBitmapFromResource("Fixed_" + one + "_" + two);
    }

    private void SecondTubePlate_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        var b = rb.Name.Last().ToString();

        secondTubePlate_pb.Image = FormHelpers.GetBitmapFromResource("ConnToFlange" + b);

        var two = b switch
        {
            "0" => "1",
            _ => "2"
        };

        var a = firstTubePlate_gb.Controls.OfType<RadioButton>()
            .FirstOrDefault(r => r.Checked)
            ?.Name.LastOrDefault().ToString();

        var one = a switch
        {
            "0" => "1",
            _ => "2"
        };

        shell_pb.Image = FormHelpers.GetBitmapFromResource("Fixed_" + one + "_" + two);
    }

    private void IsChamberFlangeSkirt_cb_CheckedChanged(object sender, EventArgs e)
    {
        var cb = sender as CheckBox;
        var type = cb.Checked ? "_Butt" : "_Flat";

        chamberFlange_rb3.Enabled = cb.Checked;

        var a = firstTubePlate_gb.Controls
            .OfType<RadioButton>()
            .FirstOrDefault(r => r.Checked)?.Name.LastOrDefault().ToString();

        if (a == "0") a = "1";

        var name = flange_gb.Controls
            .OfType<RadioButton>()
            .FirstOrDefault(r => r.Checked)?.Name.Last().ToString();

        name = (Convert.ToInt32(name) + 1).ToString();

        chamberFlange_pb.Image = FormHelpers.GetBitmapFromResource("ConnToFlange" + a + type + name);
    }

    private void HeatExchangerWithFixedTubePlatesForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (sender is not HeatExchangerStationaryTubePlatesForm) return;

        if (Owner is not MainForm { HeatExchangerStationaryTubePlatesForm: not null } main) return;

        main.HeatExchangerStationaryTubePlatesForm = null;
    }

    private void ChamberFlange_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        var a = firstTubePlate_gb.Controls
            .OfType<RadioButton>()
            .FirstOrDefault(r => r.Checked)
            ?.Name.LastOrDefault().ToString();

        if (a == "0") a = "1";

        var type = isChamberFlangeScirt_cb.Checked ? "_Butt" : "_Flat";

        var name = rb.Name.Last().ToString();

        name = (Convert.ToInt32(name) + 1).ToString();

        chamberFlange_pb.Image = FormHelpers.GetBitmapFromResource("ConnToFlange" + a + type + name);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (!CollectDataForPreliminarilyCalculation()) return;

        if (!CollectDataForFinishCalculation()) return;

        var heatExchanger = Calculate();

        if (heatExchanger == null) return;

        SetCalculatedElementToStorage(Owner, heatExchanger);

        MessageBox.Show(Resources.CalcComplete);
        Close();
    }
}