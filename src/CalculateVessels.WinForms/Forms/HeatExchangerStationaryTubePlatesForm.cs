using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Elements.HeatExchangers.Enums;
using CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerStationaryTubePlates;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Forms.MiddleForms;
using CalculateVessels.Helpers;
using FluentValidation;

namespace CalculateVessels.Forms;

public sealed partial class HeatExchangerStationaryTubePlatesForm : HeatExchangerStationaryTubePlatesFormMiddle
{
    public HeatExchangerStationaryTubePlatesForm(IEnumerable<ICalculateService<HeatExchangerStationaryTubePlatesInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<HeatExchangerStationaryTubePlatesInput> validator)
        : base(calculateServices, physicalDataService, validator)
    {
        InitializeComponent();
    }

    protected override void LoadInputData(HeatExchangerStationaryTubePlatesInput inputData)
    {
        throw new NotImplementedException();
    }

    protected override string GetServiceName()
    {
        return Gost_cb.Text;
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

    private void HeatExchangerWithFixedTubePlatesForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (sender is not HeatExchangerStationaryTubePlatesForm) return;

        if (Owner is not MainForm { HeatExchangerStationaryTubePlatesForm: not null } main) return;

        main.HeatExchangerStationaryTubePlatesForm = null;
    }

    private void Cancel_btn_Click(object sender, EventArgs e)
    {
        Hide();
    }

    protected override bool TryCollectInputData(out HeatExchangerStationaryTubePlatesInput inputData)
    {
        List<string> dataInErr = new();

        inputData = new HeatExchangerStationaryTubePlatesInput
        {
            Name = Name_tb.Text,
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

        if (inputData.IsWithPartitions)
        {
            inputData.l1R = Parameters.GetParam<double>(l1R_tb.Text, "l1R", dataInErr);
            inputData.l2R = Parameters.GetParam<double>(l2R_tb.Text, "l2R", dataInErr);
            inputData.cper = Parameters.GetParam<double>(cper_tb.Text, "cper", dataInErr);
            inputData.sper = Parameters.GetParam<double>(sper_tb.Text, "sper", dataInErr);
        }



        inputData.FirstTubePlate.TubePlateType =
            (TubePlateType)Convert.ToInt32(firstTubePlate_gb.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked)
            ?.Name.LastOrDefault().ToString());

        inputData.SecondTubePlate.TubePlateType =
            (TubePlateType)Convert.ToInt32(secondTubePlate_gb.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked)
            ?.Name.LastOrDefault().ToString());

        inputData.FirstTubePlate.Steel2 = steel2_cb.Text;


        inputData.FirstTubePlate.h2 = Parameters.GetParam<double>(h2_tb.Text, "h2", dataInErr);
        inputData.FirstTubePlate.SteelD = steelD_cb.Text;

        inputData.FirstTubePlate.s2 = Parameters.GetParam<double>(s2_tb.Text, "s2", dataInErr);
        inputData.FirstTubePlate.Steel1 = steel1_cb.Text;

        inputData.FirstTubePlate.h1 = Parameters.GetParam<double>(h1_tb.Text, "h1", dataInErr);
        inputData.FirstTubePlate.s1 = Parameters.GetParam<double>(s1_tb.Text, "s1", dataInErr);

        inputData.FirstTubePlate.DH = Parameters.GetParam<double>(DH_tb.Text, "DH", dataInErr);

        inputData.FirstTubePlate.IsChamberFlangeSkirt = isChamberFlangeScirt_cb.Checked;

        inputData.FirstTubePlate.FlangeFace =
            (FlangeFaceType)Convert.ToInt32(flange_gb.Controls
                .OfType<RadioButton>()
                .FirstOrDefault(r => r.Checked)
                ?.Name.Last().ToString());

        inputData.FirstTubePlate.Steelp = steelp_cb.Text;
        inputData.FirstTubePlate.sp = Parameters.GetParam<double>(sp_tb.Text, "sp", dataInErr);

        inputData.FirstTubePlate.c = Parameters.GetParam<double>(c_tb.Text, "c", dataInErr);


        inputData.IsNeedCheckHardnessTube = isNeedCheckTHardnessTube_cb.Checked;
        inputData.IsDifferentTubePlate = isDifferentTubePlate_cb.Checked;

        inputData.a1 = Parameters.GetParam<double>(a1_tb.Text, "a1", dataInErr);
        inputData.DE = Parameters.GetParam<double>(DE_tb.Text, "DE", dataInErr);
        inputData.i = Parameters.GetParam<int>(i_tb.Text, "i", dataInErr, NumberStyles.Integer);
        inputData.d0 = Parameters.GetParam<double>(d0_tb.Text, "d0", dataInErr);
        inputData.tp = Parameters.GetParam<double>(tp_tb.Text, "tp", dataInErr);

        inputData.IsNeedCheckHardnessTube = isNeedCheckTHardnessTube_cb.Checked;
        inputData.SteelT = steelT_cb.Text;

        inputData.dT = Parameters.GetParam<double>(dT_tb.Text, "dT", dataInErr);
        inputData.sT = Parameters.GetParam<double>(sT_tb.Text, "sT", dataInErr);
        inputData.l = Parameters.GetParam<double>(l_tb.Text, "l", dataInErr);

        var tubeRolling = tubeFirstTubePlateFix_gb.Controls
            .OfType<RadioButton>()
            .FirstOrDefault(r => r.Checked)
            ?.Name;

        switch (tubeRolling)
        {
            case nameof(rollingWithout_rb):
                inputData.FirstTubePlate.FixTubeInTubePlate = isWithWeld_cb.Checked
                    ? FixTubeInTubePlateType.RollingWithWelding :
                    FixTubeInTubePlateType.OnlyRolling;
                inputData.FirstTubePlate.TubeRolling = TubeRollingType.RollingWithoutGroove;
                break;
            case nameof(rollingWithOne_rb):
                inputData.FirstTubePlate.FixTubeInTubePlate = isWithWeld_cb.Checked
                    ? FixTubeInTubePlateType.RollingWithWelding :
                    FixTubeInTubePlateType.OnlyRolling;
                inputData.FirstTubePlate.TubeRolling = TubeRollingType.RollingWithOneGroove;
                break;
            case nameof(rollingWithTwo_rb):
                inputData.FirstTubePlate.FixTubeInTubePlate = isWithWeld_cb.Checked
                    ? FixTubeInTubePlateType.RollingWithWelding :
                    FixTubeInTubePlateType.OnlyRolling;
                inputData.FirstTubePlate.TubeRolling = TubeRollingType.RollingWithMoreThenOneGroove;
                break;
            case nameof(welded_rb):
                inputData.FirstTubePlate.FixTubeInTubePlate = FixTubeInTubePlateType.OnlyWelding;
                break;
            default:
                dataInErr.Add("Error get tube rolling type.");
                break;
        }

        if (inputData.FirstTubePlate.FixTubeInTubePlate != FixTubeInTubePlateType.OnlyWelding)
        {
            inputData.FirstTubePlate.lB = Parameters.GetParam<double>(lB_tb.Text, "lB", dataInErr);
        }

        if (inputData.FirstTubePlate.FixTubeInTubePlate == FixTubeInTubePlateType.OnlyWelding || isWithWeld_cb.Checked)
        {
            inputData.FirstTubePlate.delta = Parameters.GetParam<double>(delta_tb.Text, "delta", dataInErr);
        }

        inputData.FirstTubePlate.IsWithGroove = isWithPartitionFirstTubePlate_cb.Checked;

        if (inputData is { IsOneGo: false, FirstTubePlate.IsWithGroove: true })
        {
            inputData.FirstTubePlate.sn = Parameters.GetParam<double>(sn_tb.Text, "sn", dataInErr);
            inputData.FirstTubePlate.s1p = Parameters.GetParam<double>(s1p_tb.Text, "s1p", dataInErr);
            inputData.FirstTubePlate.BP = Parameters.GetParam<double>(BP_tb.Text, "BP", dataInErr);
            inputData.tP = Parameters.GetParam<double>(partition_tP_tb.Text, "tP", dataInErr);

            if (inputData.tP > inputData.tp)
            {
                inputData.tP = inputData.tp;
            }
            else
            {
                dataInErr.Add("tP должно быть больше tp");
            }
        }


        if (!dataInErr.Any()) return true;

        MessageBox.Show(string.Join(Environment.NewLine, dataInErr));
        return false;
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
        if (sender is not CheckBox cb) return;

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

    private void PreCalculateButton_Click(object sender, EventArgs e)
    {
        if (!TryCalculate(out var heatExchanger)) return;

        if (heatExchanger == null) throw new NullReferenceException();

        calculateButton.Enabled = true;
        MessageBox.Show(Resources.CalcComplete);
    }

    private void CalculateButton_Click(object sender, EventArgs e)
    {
        if (!TryCalculate(out var heatExchanger)) return;

        if (heatExchanger == null) throw new NullReferenceException();

        SetCalculatedElementToStorage(Owner, heatExchanger);

        MessageBox.Show(Resources.CalcComplete);
        Close();
    }
}