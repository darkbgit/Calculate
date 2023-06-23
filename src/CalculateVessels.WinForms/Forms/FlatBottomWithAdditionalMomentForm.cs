using CalculateVessels.Core.Elements.Bottoms.Enums;
using CalculateVessels.Core.Elements.Bottoms.FlatBottomWithAdditionalMoment;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels.Forms;

public partial class FlatBottomWithAdditionalMomentForm : Form
{
    private readonly IEnumerable<ICalculateService<FlatBottomWithAdditionalMomentInput>> _calculateServices;
    private readonly IPhysicalDataService _physicalDataService;

    private FlatBottomWithAdditionalMomentInput? _inputData;

    public FlatBottomWithAdditionalMomentForm(IEnumerable<ICalculateService<FlatBottomWithAdditionalMomentInput>> calculateServices,
        IPhysicalDataService physicalDataService)
    {
        InitializeComponent();
        _calculateServices = calculateServices;
        _physicalDataService = physicalDataService;
    }

    private void Cancel_b_Click(object sender, EventArgs e)
    {
        Hide();
    }

    private void FlatBottomWithAdditionalMoment_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (sender is not FlatBottomWithAdditionalMomentForm) return;

        if (Owner is MainForm { FlatBottomWithAdditionalMomentForm: { } } main)
        {
            main.FlatBottomWithAdditionalMomentForm = null;
        }
    }

    private void FlatCover_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton rb) return;

        if (rb.Checked)
        {
            grooveCover_cb.Enabled = true;
            grooveCover_cb.Checked = false;
            cover_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.FlatBottomWithMoment)
                ?? throw new InvalidOperationException());
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
        var steels = _physicalDataService.GetSteels(SteelSource.G34233D1)
            .Select(s => s as object)
            .ToArray();

        steel_cb.Items.AddRange(steels);
        steel_cb.SelectedIndex = 0;

        flangeSteel_cb.Items.AddRange(steels);
        flangeSteel_cb.SelectedIndex = 0;

        var serviceNames = _calculateServices
            .Select(s => s.Name as object)
            .ToArray();
        Gost_cb.Items.AddRange(serviceNames);
        Gost_cb.SelectedIndex = 0;

        cover_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.FlatBottomWithMoment)
            ?? throw new InvalidOperationException());
        flange_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.fl2_a)
            ?? throw new InvalidOperationException());

        var gaskets = _physicalDataService.Gost34233D4GetGasketsList()
            .Select(s => s as object)
            .ToArray();
        gasketType_cb.Items.AddRange(gaskets);
        gasketType_cb.SelectedIndex = 0;


        var ds = _physicalDataService.Gost34233D4GetScrewDs()
            .Select(s => s as object)
            .ToArray();
        screwd_cb.Items.AddRange(ds);
        screwd_cb.SelectedIndex = 0;


        var screwSteels = _physicalDataService.GetSteels(SteelSource.G34233D4Screw)
            .Select(s => s as object)
            .ToArray();
        screwSteel_cb.Items.AddRange(screwSteels);
        screwSteel_cb.SelectedIndex = 0;


        var washerSteels = _physicalDataService.GetSteels(SteelSource.G34233D4Washer)
            .Select(s => s as object)
            .ToArray();
        washerSteel_cb.Items.AddRange(washerSteels);
        washerSteel_cb.SelectedIndex = 0;
    }

    private void GrooveCover_cb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not CheckBox cb) return;

        cover_pb.Image = cb.Checked
            ? (Bitmap)(new ImageConverter().ConvertFrom(Resources.FlatBottomWithMomentWithGroove)
                        ?? throw new InvalidOperationException())
            : (Bitmap)(new ImageConverter().ConvertFrom(Resources.FlatBottomWithMoment)
                        ?? throw new InvalidOperationException());

        s4_1_l.Visible = cb.Checked;
        s4_2_l.Visible = cb.Checked;
        s4_tb.Visible = cb.Checked;
    }

    private void Hole_cb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is CheckBox cb) hole_gb.Enabled = cb.Checked;
    }

    private void Flange_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        var i = rb.Name[0].ToString();
        var type = scirtFlange_cb.Checked ? "fl1_" : "fl2_";
        flange_pb.Image =
            (Bitmap)(new ImageConverter().ConvertFrom(Resources.ResourceManager.GetObject(type + i)
                                                       ?? throw new InvalidOperationException())
                      ?? throw new InvalidOperationException());
    }

    private void SkirtFlange_cb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not CheckBox { Checked: true } cb) return;

        if (d4_flange_rb.Checked)
        {
            a1_flange_rb.Checked = true;
        }

        var name = flange_gb.Controls
            .OfType<RadioButton>()
            .FirstOrDefault(r => r.Checked)?.Name[0].ToString();

        d4_flange_rb.Enabled = cb.Checked;
        var type = cb.Checked ? "fl1_" : "fl2_";
        flange_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.ResourceManager.GetObject(type + name)
                                                                    ?? throw new InvalidOperationException())
                                   ?? throw new InvalidOperationException());

        s_1_l.Visible = !cb.Checked;
        s_2_l.Visible = !cb.Checked;
        s_tb.Visible = !cb.Checked;

        s0_1_l.Visible = cb.Checked;
        s0_2_l.Visible = cb.Checked;
        S0_tb.Visible = cb.Checked;

        s1_1_l.Visible = cb.Checked;
        s1_2_l.Visible = cb.Checked;
        s1scirt_tb.Visible = cb.Checked;

        l_1_l.Visible = cb.Checked;
        l_2_l.Visible = cb.Checked;
        l_tb.Visible = cb.Checked;

        //UNDONE
    }

    private void IsWasher_cb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not CheckBox cb) return;

        washerSteel_cb.Visible = cb.Checked;
        washerSteel_l.Visible = cb.Checked;

        hsh_1_l.Visible = cb.Checked;
        hsh_2_l.Visible = cb.Checked;
        hsh_tb.Visible = cb.Checked;
    }

    private bool CollectDataForPreliminarilyCalculation()
    {
        var dataInErr = new List<string>();

        _inputData = new FlatBottomWithAdditionalMomentInput
        {
            t = Parameters.GetParam<double>(t_tb.Text, "t", dataInErr, NumberStyles.Integer),
            CoverSteel = steel_cb.Text,
            FlangeSteel = flangeSteel_cb.Text,
            ScrewSteel = screwSteel_cb.Text,
            IsPressureIn = !pressureOut_cb.Checked,
            IsCoverFlat = flatCover_rb.Checked,
            IsCoverWithGroove = grooveCover_cb.Checked,
            IsFlangeFlat = !scirtFlange_cb.Checked,
            IsFlangeIsolated = isolatedFlange_cb.Checked,
            IsScrewWithGroove = isScrewWithGroove_cb.Checked,
            IsStud = isStud_cb.Checked,
            IsWasher = isWasher_cb.Checked,
            GasketType = gasketType_cb.Text,

            p = Parameters.GetParam<double>(p_tb.Text, "p", dataInErr),
            F = Parameters.GetParam<double>(F_tb.Text, "F", dataInErr),
            fi = Parameters.GetParam<double>(fi_tb.Text, "φ", dataInErr),
            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", dataInErr),
            Screwd = Parameters.GetParam<int>(screwd_cb.Text, "dscrew", dataInErr, NumberStyles.Integer),
            s1 = Parameters.GetParam<double>(s1_tb.Text, "s1", dataInErr),
            s2 = Parameters.GetParam<double>(s2_tb.Text, "s2", dataInErr),
            s3 = Parameters.GetParam<double>(s3_tb.Text, "s3", dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", dataInErr),
            D2 = Parameters.GetParam<double>(D2_tb.Text, "D2", dataInErr),
            D3 = Parameters.GetParam<double>(D3_tb.Text, "D3", dataInErr),
            Dn = Parameters.GetParam<double>(Dn_tb.Text, "Dn", dataInErr),
            Db = Parameters.GetParam<double>(Db_tb.Text, "Db", dataInErr),
            Dcp = Parameters.GetParam<double>(Dcp_tb.Text, "Dcp", dataInErr),
            d = Parameters.GetParam<double>(screwd_cb.Text, "d", dataInErr),
            M = Parameters.GetParam<double>(M_tb.Text, "M", dataInErr),
            h = Parameters.GetParam<double>(h_tb.Text, "h", dataInErr),
            hp = Parameters.GetParam<double>(hp_tb.Text, "hp", dataInErr),
            bp = Parameters.GetParam<double>(bp_tb.Text, "bp", dataInErr),
            Lb0 = Parameters.GetParam<double>(Lb0_tb.Text, "Lb0", dataInErr),
            n = Parameters.GetParam<int>(n_tb.Text, "n", dataInErr, NumberStyles.Integer),

            FlangeFace = (FlangeFaceType)
                (Parameters.GetParam<int>(flange_gb.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked)?.Name[1].ToString(),
                    "Уплотнительная поверхность фланца", dataInErr, NumberStyles.Integer)
                    - 1),

            SigmaAllow = sigmaHandle_cb.Checked
                ? Parameters.GetParam<double>(sigma_d_tb.Text, "[σ]", dataInErr)
                : default,
            E = EHandle_cb.Checked
                ? Parameters.GetParam<double>(E_tb.Text, "E", dataInErr)
                : default
        };

        if (_inputData.IsCoverWithGroove)
        {
            _inputData.s4 = Parameters.GetParam<double>(s4_tb.Text, "s4", dataInErr);
        }

        if (!hole_cb.Checked)
        {
            _inputData.Hole = HoleInFlatBottom.WithoutHole;
        }
        else
        {
            var d = Parameters.GetParam<double>(holed_tb.Text, "d", dataInErr);
            if (oneHole_rb.Checked)
            {
                _inputData.Hole = HoleInFlatBottom.OneHole;
                _inputData.d = d;
            }
            else
            {
                _inputData.Hole = HoleInFlatBottom.MoreThenOneHole;
                _inputData.di = d;
            }
        }

        if (_inputData.IsFlangeFlat)
        {
            _inputData.s = Parameters.GetParam<double>(s_tb.Text, "s", dataInErr);
        }
        else
        {
            _inputData.S0 = Parameters.GetParam<double>(S0_tb.Text, "S0", dataInErr);
            _inputData.S1 = Parameters.GetParam<double>(s1scirt_tb.Text, "S1", dataInErr);
            _inputData.l = Parameters.GetParam<double>(l_tb.Text, "l", dataInErr);
        }

        if (isWasher_cb.Checked)
        {
            _inputData.WasherSteel = washerSteel_cb.Text;
            _inputData.hsh = Parameters.GetParam<double>(hsh_tb.Text, "hsh", dataInErr);
        }

        var isNoError = !dataInErr.Any() && _inputData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_inputData.ErrorList)));
        }

        return isNoError;
    }

    private bool CollectDataForFinishCalculation()
    {
        if (_inputData == null) throw new InvalidOperationException();

        var dataInErr = new List<string>();

        _inputData.Name = name_tb.Text;

        var isNoError = !dataInErr.Any() && _inputData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_inputData.ErrorList)));
        }

        return isNoError;
    }

    private void PreCalc_b_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;
        calc_b.Enabled = false;

        if (!CollectDataForPreliminarilyCalculation()) return;

        ICalculatedElement bottom;

        try
        {
            bottom = GetCalculateService().Calculate(_inputData
                ?? throw new InvalidOperationException());
        }
        catch (CalculateException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        if (bottom.ErrorList.Any())
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, bottom.ErrorList));
        }

        calc_b.Enabled = true;
        scalc_l.Text = $@"sp={((FlatBottomWithAdditionalMomentCalculated)bottom).s1:f3} мм";
        p_d_l.Text =
            $@"pd={((FlatBottomWithAdditionalMomentCalculated)bottom).p_d:f2} МПа";

        MessageBox.Show(Resources.CalcComplete);
    }

    private void Calc_b_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;

        if (!CollectDataForFinishCalculation()) return;

        ICalculatedElement bottom;

        try
        {
            bottom = GetCalculateService().Calculate(_inputData
                ?? throw new InvalidOperationException());
        }
        catch (CalculateException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        if (bottom.ErrorList.Any())
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, bottom.ErrorList));
        }

        calc_b.Enabled = true;
        scalc_l.Text = $@"sp={((FlatBottomWithAdditionalMomentCalculated)bottom).s1:f3} мм";
        p_d_l.Text = $@"pd={((FlatBottomWithAdditionalMomentCalculated)bottom).p_d:f2} МПа";

        if (Owner is not MainForm main)
        {
            MessageBox.Show($"{nameof(MainForm)} error");
            return;
        }

        main.calculatedElementsControl.AddElement(bottom);

        MessageBox.Show(Resources.CalcComplete);
        Close();
    }

    private void SigmaHandle_cb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is CheckBox cb)
        {
            sigma_d_tb.Enabled = cb.Checked;
        }
    }

    private void EHandle_cb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is CheckBox cb)
        {
            E_tb.Enabled = cb.Checked;
        }
    }

    private ICalculateService<FlatBottomWithAdditionalMomentInput> GetCalculateService()
    {
        return _calculateServices
                   .FirstOrDefault(s => s.Name == Gost_cb.Text)
               ?? throw new InvalidOperationException("Service wasn't found.");
    }
}