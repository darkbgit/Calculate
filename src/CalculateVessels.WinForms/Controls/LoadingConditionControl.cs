using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace CalculateVessels.Controls;

[Designer(typeof(ControlDesigner))]
public partial class LoadingConditionControl : UserControl
{
    public LoadingConditionControl()
    {
        InitializeComponent();
    }

    public LoadingCondition? GetLoadingCondition()
    {
        var dataInErr = new List<string>();

        var loadingCondition = new LoadingCondition
        {
            OrdinalNumber = 1,
            IsPressureIn = !isPressureOutsideCheckBox.Checked,
            t = Parameters.GetParam<double>(temperatureTextBox.Text, "t", dataInErr, NumberStyles.Integer),
            p = Parameters.GetParam<double>(pressureTextBox.Text, "p", dataInErr),
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

    public void SetLoadingCondition(LoadingCondition loadingCondition)
    {

        isPressureOutsideCheckBox.Checked = !loadingCondition.IsPressureIn;
        temperatureTextBox.Text = loadingCondition.t.ToString(CultureInfo.CurrentCulture);
        pressureTextBox.Text = loadingCondition.p.ToString(CultureInfo.CurrentCulture);
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