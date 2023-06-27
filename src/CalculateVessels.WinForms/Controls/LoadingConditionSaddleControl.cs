using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Enums;
using CalculateVessels.Helpers;

namespace CalculateVessels.Controls;

public partial class LoadingConditionSaddleControl : UserControl
{
    public LoadingConditionSaddleControl()
    {
        InitializeComponent();
    }

    public LoadingConditionSaddle? GetLoadingCondition()
    {
        var dataInErr = new List<string>();

        var loadingCondition = new LoadingConditionSaddle
        {
            OrdinalNumber = 1,
            PressureType = isPressureOutsideCheckBox.Checked ? PressureType.Outside : PressureType.Inside,
            t = Parameters.GetParam<double>(temperatureTextBox.Text, "t", dataInErr, NumberStyles.Integer),
            p = Parameters.GetParam<double>(pressureTextBox.Text, "p", dataInErr),
            N = Parameters.GetParam<int>(NComboBox.Text, "N", dataInErr),
            G = Parameters.GetParam<double>(GTextBox.Text, "G", dataInErr),
            IsAssembly = isAssemblyCheckBox.Checked,
            SigmaAllow = isSigmaAllowHandleCheckBox.Checked
                ? Parameters.GetParam<double>(sigmaAllowTextBox.Text, "[σ]", dataInErr)
                : default,
            EAllow = isEAllowHandleCheckBox.Checked
                ? Parameters.GetParam<double>(EAllowTextBox.Text, "E", dataInErr)
                : default
        };


        if (!dataInErr.Any())
        {
            return loadingCondition;
        }

        MessageBox.Show(string.Join(Environment.NewLine, dataInErr));
        return null;
    }

    public void SetLoadingCondition(LoadingConditionSaddle loadingCondition)
    {

        isPressureOutsideCheckBox.Checked = loadingCondition.PressureType == PressureType.Outside;
        temperatureTextBox.Text = loadingCondition.t.ToString(CultureInfo.CurrentCulture);
        pressureTextBox.Text = loadingCondition.p.ToString(CultureInfo.CurrentCulture);
        NComboBox.Text = loadingCondition.N.ToString();
        GTextBox.Text = loadingCondition.G.ToString(CultureInfo.CurrentCulture);
        isAssemblyCheckBox.Checked = loadingCondition.IsAssembly;
        if (loadingCondition.SigmaAllow > 0)
        {
            isSigmaAllowHandleCheckBox.Checked = true;
            sigmaAllowTextBox.Text = loadingCondition.SigmaAllow.ToString(CultureInfo.CurrentCulture);
        }
        if (loadingCondition.EAllow > 0)
        {
            isEAllowHandleCheckBox.Checked = true;
            EAllowTextBox.Text = loadingCondition.EAllow.ToString(CultureInfo.CurrentCulture);
        }
    }

    private void SigmaAllowHandleCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        if (sender is CheckBox cb)
        {
            FormHelpers.EnabledIfCheck(sigmaAllowTextBox, cb.Checked);
        }
    }

    private void EHandleCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        if (sender is CheckBox cb)
        {
            FormHelpers.EnabledIfCheck(EAllowTextBox, cb.Checked);
        }
    }
}